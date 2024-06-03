using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Transactions;

namespace HalloDoc.Controllers
{
    [AuthManager("3")]
    public class ProviderDashboardController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IEmailService _emailService;
        private readonly IDashboardService _dashboardService;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IRequestService _requestService;
        private readonly IUserRepository _userRepository;
        private readonly IRecordsRepository _recordsRepository;
        private readonly IJwtService _jwtService;

        public ProviderDashboardController(IJwtService jwtService, IUserRepository userRepository, IAdminDashboardService adminDashboardService, IEmailService emailService, IDashboardService dashboardService, IRequestWiseFilesRepository requestWiseFilesRepository, IOrderService orderService, IUserService userService, IRequestService requestService, IRecordsRepository recordsRepository)
        {
            _adminDashboardService = adminDashboardService;
            _emailService = emailService;
            _dashboardService = dashboardService;
            _requestWiseFilesRepository = requestWiseFilesRepository;
            _orderService = orderService;
            _userService = userService;
            _requestService = requestService;
            _userRepository = userRepository;
            _recordsRepository = recordsRepository;
            _jwtService = jwtService;
        }
        public IActionResult ProviderDashboard()
        {
            UserInfo user = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(user.Email);
            var dash = new AdminDashboard();
            dash.Data = _adminDashboardService.GetRequests();
            dash.Data = dash.Data.Where(x => x.physicianId == physician.Physicianid).ToList();
            dash.regions = _adminDashboardService.GetAllRegions();
            if (HttpContext.Session.GetString("state") == null)
            {
                HttpContext.Session.SetString("state", "1");
            };
            if (HttpContext.Session.GetString("pageNumber") == null)
            {
                HttpContext.Session.SetString("pageNumber", "1");
            };
            return View(dash);
        }

        public async Task<IActionResult> CaseTable(int state)
        {
            var reqtype = HttpContext.Session.GetString("reqtype");
            var pageNumber = HttpContext.Session.GetString("pageNumber");
            var region = HttpContext.Session.GetString("region") != null ? HttpContext.Session.GetString("region") : "0";
            var search = HttpContext.Session.GetString("search") != null ? HttpContext.Session.GetString("search") : "";
            if (reqtype == null)
            {
                reqtype = "0";
            }
            if (pageNumber == null || HttpContext.Session.GetString("state") != state.ToString())
            {
                pageNumber = "1";
            }
            var dash = new AdminDashboard();
            dash.state = state;
            int[] status;
            switch (state)
            {
                case 1:
                    status = new int[] { 1 };
                    break;
                case 2:
                    status = new int[] { 2 };
                    break;
                case 3:
                    status = new int[] { 4, 5 };
                    break;
                case 4:
                    status = new int[] { 6 };
                    break;
                default: status = new int[] { 1 }; break;

            }
            UserInfo user = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(user.Email);
            dash.pagedList = await _adminDashboardService.GetRequestsByStatus(status, int.Parse(reqtype), int.Parse(pageNumber), int.Parse(region), search, false, physician.Physicianid);

            dash.regions = _adminDashboardService.GetAllRegions();
            HttpContext.Session.SetString("state", state.ToString());
            return PartialView("_CaseTable", dash);
        }
        public string FilterRequest(int reqtype)
        {
            HttpContext.Session.SetString("reqtype", reqtype.ToString());
            HttpContext.Session.SetString("pageNumber", "1");
            return HttpContext.Session.GetString("state");
        }

        public string RegionFilter(int region)
        {
            HttpContext.Session.SetString("region", region.ToString());
            HttpContext.Session.SetString("pageNumber", "1");
            return HttpContext.Session.GetString("state");
        }

        public string Search(string search = "")
        {
            HttpContext.Session.SetString("search", search);
            HttpContext.Session.SetString("pageNumber", "1");

            return HttpContext.Session.GetString("state");
        }

        public string ChangePage(int pageNumber)
        {
            HttpContext.Session.SetString("pageNumber", pageNumber.ToString());
            return HttpContext.Session.GetString("state");
        }

        public JsonResult AcceptCase(int requestid)
        {
            _requestService.AcceptCase(requestid);
            return Json(new { success = true });
        }

        public bool CurrentView(string view)
        {
            HttpContext.Session.SetString("view", view);
            return true;
        }

        public IActionResult ViewCase(int requestid)
        {
            var user = SessionService.GetLoggedInUser(HttpContext.Session);
            var dash = new AdminDashboard();

            dash.regions = _adminDashboardService.GetAllRegions();
            dash.request = _adminDashboardService.GetRequestById(requestid);
            if (dash.request == null)
            {
                return View("Invalid");
            }
            return View("ViewCase", dash);
        }

        public IActionResult ViewNotes(int requestid)
        {
            var dash = _adminDashboardService.GetNotes(requestid);
            return PartialView("ViewNotes", dash);
        }
        public JsonResult UpdateNotes(int requestid, string notes)
        {
            var dash = _adminDashboardService.UpdateNotes(requestid, notes, true);
            return Json(new { success = true });
        }

        public JsonResult SendLink(IFormCollection formcollection)
        {
            var email = formcollection["email"];
            var link = "https://localhost:44319/SubmitRequest/SubmitRequest";
            var subject = "Submit Request";
            var body = $"You can submit a request <a href='{link}'>here</a>.";

            _emailService.SendEmail(email, subject, body);
            return Json(new { success = true });
        }

        public JsonResult SendAgreement(IFormCollection formcollection)
        {
            var email = formcollection["sendemail"];
            var requestid = formcollection["requestId"];
            var link = "https://localhost:44319/Login/PatientLogin?RequestId=" + requestid + "&review=true";
            var subject = "Agreement";
            var body = $"You can view your agreement <a href='{link}'>here</a>.";

            _emailService.SendEmail(email, subject, body);
            return Json(new { success = true });
        }

        public IActionResult ViewUploads(int requestid)
        {
            ViewUploads viewUploads = new ViewUploads();
            viewUploads.Request = _adminDashboardService.GetRequestById(requestid);
            viewUploads.Requestwisefiles = _dashboardService.ViewDocument(requestid);
            if (viewUploads == null)
            {
                return View("Invalid");
            }
            return View(viewUploads);
        }

        public JsonResult TransferCase(int requestid, string description)
        {
            _requestService.TransferCase(requestid, description);
            return Json(new { success = true });
        }

        public IActionResult Orders(int requestid)
        {
            var orders = new Orders();
            orders.requestid = requestid;
            orders.healthprofessionaltypes = _orderService.GetAllPrfessions();
            return View(orders);
        }

        public ActionResult<IEnumerable<Healthprofessional>> GetVendorsByProfession(int profession)
        {
            var vendors = _orderService.GetVendorsByProfessions(profession);
            return vendors;
        }

        public ActionResult<Healthprofessional> GetVendorById(int vendorid)
        {
            var vendor = _orderService.GetVendorById(vendorid);
            return vendor;
        }

        public JsonResult CreateOrder(IFormCollection formcollection)
        {
            var user = SessionService.GetLoggedInUser(HttpContext.Session);
            var od = new Orderdetail
            {
                Vendorid = int.Parse(formcollection["business"]),
                Requestid = int.Parse(formcollection["requestid"]),
                Faxnumber = formcollection["fax"],
                Email = formcollection["email"],
                Businesscontact = formcollection["contact"],
                Prescription = formcollection["prescription"],
                Noofrefill = int.Parse(formcollection["noofrefill"]),
                Createddate = DateTime.Now,
                Createdby = user.Name
            };
            _orderService.CreateOrder(od);
            return Json(new { success = true });
        }

        public JsonResult ChangeStatus(int Requestid, int status)
        {
            _requestService.TransferCase(Requestid, "", status);
            return Json(new { success = true, });
        }

        public IActionResult Encounter(int requestid)
        {
            string[] format = { "dd/MMMM/yyyy", "d/MMMM/yyyy" };
            AdminDashboardData ad = _adminDashboardService.GetRequestById(requestid);
            if (ad == null)
            {
                return View("Invalid");
            }
            EncounterForm ef = _adminDashboardService.GetEncounterForm(requestid);
            if (ef != null)
            {
                var data = new ViewEncounterForm
                {
                    FirstName = ad.firstName,
                    LastName = ad.lastName,
                    Location = ad.address,
                    DateOfBirth = DateTime.ParseExact(ad.dobdate + "/" + ad.dobmonth + "/" + ad.dobyear, format, CultureInfo.InvariantCulture),
                    DateOfService = ad.reqdate.ToString(),
                    Email = ad.email,
                    PhoneNumber = ad.phone,
                    HistoryOfPresentIllness = ef.HistoryOfPresentIllnessOrInjury,
                    MedicalHistory = ef.MedicalHistory,
                    Medications = ef.Medications,
                    Allergies = ef.Allergies,
                    Temperature = ef.Temp,
                    HR = ef.Hr,
                    RR = ef.Rr,
                    BPSystolic = ef.BloodPressureSystolic,
                    BPDiastolic = ef.BloodPressureDiastolic,
                    O2 = ef.O2,
                    Pain = ef.Pain,
                    Heent = ef.Heent,
                    CV = ef.Cv,
                    Chest = ef.Chest,
                    ABD = ef.Abd,
                    Extr = ef.Extremeties,
                    Skin = ef.Skin,
                    Neuro = ef.Neuro,
                    Other = ef.Other,
                    Diagnosis = ef.Diagnosis,
                    TreatmentPlan = ef.TreatmentPlan,
                    MedicationDispensed = ef.MedicationsDispensed,
                    Procedures = ef.Procedures,
                    FollowUp = ef.FollowUp
                };
                return View(data);

            }
            else
            {
                var data = new ViewEncounterForm
                {
                    FirstName = ad.firstName,
                    LastName = ad.lastName,
                    Location = ad.address,
                    DateOfBirth = DateTime.ParseExact(ad.dobdate + "/" + ad.dobmonth + "/" + ad.dobyear, format, CultureInfo.InvariantCulture),
                    DateOfService = ad.reqdate.ToString(),
                    Email = ad.email,
                    PhoneNumber = ad.phone,
                };
                return View(data);
            }

        }

        public JsonResult EditEncounterForm(ViewEncounterForm model)
        {
            _adminDashboardService.UpdateEncounterForm(model);
            return Json(new { success = true });
        }

        public IActionResult GeneratePDF(int requestid)
        {
            string[] format = { "dd/MMMM/yyyy", "d/MMMM/yyyy" };
            AdminDashboardData ad = _adminDashboardService.GetRequestById(requestid);
            EncounterForm ef = _adminDashboardService.GetEncounterForm(requestid);
            var encounterFormView = new ViewEncounterForm
            {
                FirstName = ad.firstName,
                LastName = ad.lastName,
                Location = ad.address,
                DateOfBirth = DateTime.ParseExact(ad.dobdate + "/" + ad.dobmonth + "/" + ad.dobyear, format, CultureInfo.InvariantCulture),
                DateOfService = ad.reqdate.ToString(),
                Email = ad.email,
                PhoneNumber = ad.phone,
                HistoryOfPresentIllness = ef.HistoryOfPresentIllnessOrInjury,
                MedicalHistory = ef.MedicalHistory,
                Medications = ef.Medications,
                Allergies = ef.Allergies,
                Temperature = ef.Temp,
                HR = ef.Hr,
                RR = ef.Rr,
                BPSystolic = ef.BloodPressureSystolic,
                BPDiastolic = ef.BloodPressureDiastolic,
                O2 = ef.O2,
                Pain = ef.Pain,
                Heent = ef.Heent,
                CV = ef.Cv,
                Chest = ef.Chest,
                ABD = ef.Abd,
                Extr = ef.Extremeties,
                Skin = ef.Skin,
                Neuro = ef.Neuro,
                Other = ef.Other,
                Diagnosis = ef.Diagnosis,
                TreatmentPlan = ef.TreatmentPlan,
                MedicationDispensed = ef.MedicationsDispensed,
                Procedures = ef.Procedures,
                FollowUp = ef.FollowUp
            };

            if (encounterFormView == null)
            {
                return NotFound();
            }
            //return View("EncounterFormDetails", encounterFormView);
            return new ViewAsPdf("EncounterFormDetails", encounterFormView)
            {
                FileName = "Encounter_Form.pdf"
            };

        }

        public IActionResult ProviderProfile()
        {
            Provider prov = new Provider();
            UserInfo user = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(user.Email);
            prov.physician = _adminDashboardService.GetPhysiciansById(physician.Physicianid);
            prov.regions = _adminDashboardService.GetAllRegions();
            prov.phyregions = _adminDashboardService.GetPhysicianRegions(physician.Physicianid);
            return View(prov);
        }

        public IActionResult ConcludeCare(int requestid)
        {
            ViewUploads viewUploads = new ViewUploads();
            viewUploads.Request = _adminDashboardService.GetRequestById(requestid);
            viewUploads.Requestwisefiles = _dashboardService.ViewDocument(requestid);
            if (viewUploads == null)
            {
                return View("Invalid");
            }
            return PartialView(viewUploads);
        }

        public JsonResult ConcludeRequest(int Requestid)
        {
            _requestService.TransferCase(Requestid, "", 8);
            return Json(new { success = true });
        }

        public IActionResult MySchedule()
        {
            UserInfo user = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(user.Email);
            var region = _adminDashboardService.GetAllRegions();
            ViewBag.regions = region;
            ScheduleModel model = new ScheduleModel();
            model.Physicianid = physician.Physicianid;
            model.Regionid = physician.Regionid;
            model.regions = region;
            model.physicians = _adminDashboardService.GetPhysiciansByRegion((int)model.Regionid);
            return View(model);
        }

        public IActionResult CreateShift(ScheduleModel model)
        {
            var admin = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(admin.Email);
            model.Physicianid = physician.Physicianid;
            model.Regionid = physician.Regionid;
            var events = _userRepository.GetMappedEvents(0);
            bool slotAvailable = true;
            foreach (var e in events)
            {
                if (e.resourceId == model.Physicianid)
                {
                    if (model.Startdate == DateOnly.FromDateTime(e.start.Date))
                        if ((TimeOnly.FromDateTime(e.start) <= model.Starttime && model.Starttime <= TimeOnly.FromDateTime(e.end)) ||
                     (TimeOnly.FromDateTime(e.start) <= model.Endtime && model.Endtime <= TimeOnly.FromDateTime(e.end)))
                        {
                            slotAvailable = false;
                            return Json(new { success = false });
                        }
                }
            }
            using (var transaction = new TransactionScope())
            {
                Shift shift = new Shift();
                shift.Physicianid = model.Physicianid;
                shift.Repeatupto = model.Repeatupto;
                shift.Startdate = model.Startdate;
                shift.Createdby = admin.Id;
                shift.Createddate = DateTime.Now;
                shift.Isrepeat = model.Isrepeat;
                shift.Repeatupto = model.Repeatupto;
                _userRepository.AddShift(shift);

                Shiftdetail sd = new Shiftdetail();
                sd.Shiftid = shift.Shiftid;
                sd.Shiftdate = new DateTime(model.Startdate.Year, model.Startdate.Month, model.Startdate.Day);
                sd.Starttime = model.Starttime;
                sd.Endtime = model.Endtime;
                sd.Regionid = model.Regionid;
                sd.Status = model.Status;
                sd.Isdeleted = false;


                _userRepository.AddShiftDetails(sd);

                Shiftdetailregion sr = new Shiftdetailregion();
                sr.Shiftdetailid = sd.Shiftdetailid;
                sr.Regionid = (int)model.Regionid;
                sr.Isdeleted = false;
                _userRepository.AddShiftDetailRegions(sr);

                if (model.checkWeekday != null)
                {

                    List<int> day = model.checkWeekday.Split(',').Select(int.Parse).ToList();

                    foreach (int d in day)
                    {
                        DayOfWeek desiredDayOfWeek = (DayOfWeek)d;
                        DateTime today = DateTime.Today;
                        DateTime nextOccurrence = new DateTime(model.Startdate.Year, model.Startdate.Month, model.Startdate.Day);
                        int occurrencesFound = 0;
                        while (occurrencesFound < model.Repeatupto)
                        {
                            if (nextOccurrence.DayOfWeek == desiredDayOfWeek)
                            {

                                Shiftdetail sdd = new Shiftdetail();
                                sdd.Shiftid = shift.Shiftid;
                                sdd.Shiftdate = nextOccurrence;
                                sdd.Starttime = model.Starttime;
                                sdd.Endtime = model.Endtime;
                                sdd.Regionid = model.Regionid;
                                sdd.Status = (short)model.Status;
                                sdd.Isdeleted = false;
                                _userRepository.AddShiftDetails(sdd);

                                Shiftdetailregion srr = new Shiftdetailregion();
                                srr.Shiftdetailid = sdd.Shiftdetailid;
                                srr.Regionid = (int)model.Regionid;
                                srr.Isdeleted = false;
                                _userRepository.AddShiftDetailRegions(srr);
                                occurrencesFound++;
                            }
                            nextOccurrence = nextOccurrence.AddDays(1);
                        }
                    }
                }

                transaction.Complete();
            }
            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult GetPhysicianShift(int region)
        {
            // Retrieve physicians associated with the specified region
            var physicians = _adminDashboardService.GetPhysiciansByRegion(region);

            return Ok(physicians);
        }

        [HttpGet]
        public IActionResult GetEvents(int region, int Physicianid)
        {
            var mappedEvents = _userRepository.GetMappedEvents(region);
            mappedEvents = mappedEvents.Where(x => x.resourceId == Physicianid).ToList();
            return Ok(mappedEvents);
        }

        [HttpPost]
        public IActionResult SaveShift(int shiftDetailId, DateTime startDate, TimeOnly startTime, TimeOnly endTime)
        {
            // Find the shift detail by its ID
            Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);
            var admin = SessionService.GetLoggedInUser(HttpContext.Session);
            Physician physician = _adminDashboardService.GetPhysiciansByEmail(admin.Email);

            var events = _userRepository.GetMappedEvents(0);
            bool slotAvailable = true;
            foreach (var e in events)
            {
                if (shiftDetailId != e.ShiftDetailId)
                {
                    if (e.resourceId == shiftdetail.Shift.Physicianid)
                    {
                        if (startDate == e.start.Date)
                            if ((TimeOnly.FromDateTime(e.start) <= startTime && startTime <= TimeOnly.FromDateTime(e.end)) ||
                         (TimeOnly.FromDateTime(e.start) <= endTime && endTime <= TimeOnly.FromDateTime(e.end)))
                            {
                                slotAvailable = false;
                                return Json(new { error = true });
                            }
                    }
                }

            }
            // If shift detail is not found, return a 404 Not Found response
            if (shiftdetail == null)
            {
                return Json(new { error = true });
            }

            try
            {
                // Update the shift detail properties
                shiftdetail.Shiftdate = startDate;
                shiftdetail.Starttime = startTime;
                shiftdetail.Endtime = endTime;
                shiftdetail.Modifiedby = admin.Id;
                shiftdetail.Modifieddate = DateTime.Now;

                // Update the database
                _userRepository.UpdateShiftDetails(shiftdetail);
                return RedirectToAction("GetEvents", new { Physicianid = shiftdetail.Shift.Physicianid });
            }
            catch (Exception ex)
            {
                // Return a 400 Bad Request response with the error message
                return Json(new { success = false });
            }
        }

        public IActionResult DeleteShift(int shiftDetailId)
        {
            Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);
            var user = SessionService.GetLoggedInUser(HttpContext.Session);
            Admin? admin = _adminDashboardService.GetAdminById(user.Id);

            if (shiftdetail == null)
            {
                return NotFound("Shift detail not found.");
            }
            shiftdetail.Isdeleted = true;
            shiftdetail.Modifiedby = user.Id;
            shiftdetail.Modifieddate = DateTime.Now;
            _userRepository.UpdateShiftDetails(shiftdetail);
            var mappedEvents = _userRepository.GetMappedEvents(0);
            return RedirectToAction("GetEvents");
        }

        public JsonResult DeleteSelectedShift(int shiftDetailId)
        {
            Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);
            var user = SessionService.GetLoggedInUser(HttpContext.Session);
            Admin? admin = _adminDashboardService.GetAdminById(user.Id);

            if (shiftdetail == null)
            {
                return Json(new { success = false });
            }
            shiftdetail.Isdeleted = true;
            shiftdetail.Modifiedby = user.Id;
            shiftdetail.Modifieddate = DateTime.Now;
            _userRepository.UpdateShiftDetails(shiftdetail);
            var mappedEvents = _userRepository.GetMappedEvents(0);
            return Json(new { success = true });
        }

        public JsonResult ApproveSelectedShift(int shiftDetailId)
        {
            Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);
            var user = SessionService.GetLoggedInUser(HttpContext.Session);
            Admin? admin = _adminDashboardService.GetAdminById(user.Id);

            if (shiftdetail == null)
            {
                return Json(new { success = false });
            }
            shiftdetail.Status = 1;
            shiftdetail.Modifiedby = user.Id;
            shiftdetail.Modifieddate = DateTime.Now;
            _userRepository.UpdateShiftDetails(shiftdetail);
            var mappedEvents = _userRepository.GetMappedEvents(0);
            return Json(new { success = true });
        }

        public IActionResult ReturnShift(int shiftDetailId)
        {
            Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);

            // If shift detail is not found, return a 404 Not Found response
            if (shiftdetail == null)
            {
                return NotFound("Shift detail not found.");
            }
            shiftdetail.Status = (short)((shiftdetail.Status == 0) ? 1 : 0);

            _userRepository.UpdateShiftDetails(shiftdetail);
            return RedirectToAction("GetEvents", new { Physicianid = shiftdetail.Shift.Physicianid });

        }

        public IActionResult Invoicing()
        {
            return View();
        }
        public IActionResult SearchDataByRange(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            InvoicingView model = new InvoicingView();
            model.timesheets = _dashboardService.SearchDataByRangeTimeSheet(startDate, aspuserid);
            model.isFinalize = _dashboardService.CheckFinalize(startDate, aspuserid);

            return PartialView("_timeSheet", model);
        }

        public IActionResult LoadFinalizeInvoicing(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            InvoicingView model = new InvoicingView();
            model.invoicing = _dashboardService.SearchDataByRangeInvoicing(startDate, aspuserid);
            model.PhysicianId = _adminDashboardService.GetPhysiciansByAspId(aspuserid).Physicianid;
            model.startDate = startDate;

            return PartialView("_finalizeInvoicing", model);
        }

        public IActionResult LoadTimeSheetReimbursement(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            InvoicingView model = new InvoicingView();
            model.PhysicianId = _adminDashboardService.GetPhysiciansByAspId(aspuserid).Physicianid;
            model.invoicing = _dashboardService.SearchDataByRangeReimbursement(startDate, aspuserid);

            return PartialView("_timeSheetReimbursement", model);
        }

        public IActionResult SaveTimeSheet(List<InvoicingModel> invoicingModels)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            _dashboardService.SaveTimeSheet(invoicingModels, aspuserid);
            return Json(new { success = true });
        }

        public IActionResult SaveReimbursement(InvoicingModel invoicingModels)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            _dashboardService.SaveReimbursement(invoicingModels, aspuserid);
            return Json(new { success = true });
        }

        public IActionResult FinalizeTimesheet(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            _dashboardService.FinalizeTimesheet(startDate, aspuserid);
            return Json(new { success = true });
        }

        public IActionResult LoadChatPatient(int Patientid)
        {
            var token = Request.Cookies["jwt"];
            var aspnetid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            ChatModel model = _userService.GetChatPatient(Patientid, aspnetid);

            return PartialView("_chat", model);
        }

        public IActionResult LoadChatAdmin(int Adminid)
        {
            var token = Request.Cookies["jwt"];
            var aspnetid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            ChatModel model = _userService.GetChatAdmin(Adminid, aspnetid);

            return PartialView("_chat", model);
        }

        public IActionResult LoadGroupChat(int Patientid, int Adminid)
        {
            var token = Request.Cookies["jwt"];
            var aspnetid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            }

            ChatModel model = _userService.GetGroupChat(Adminid, Patientid, aspnetid);

            return PartialView("_groupChat", model);
        }
    }
}
