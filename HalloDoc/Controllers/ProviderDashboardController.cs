using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Globalization;

namespace HalloDoc.Controllers
{
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
        
        public ProviderDashboardController(IUserRepository userRepository, IAdminDashboardService adminDashboardService, IEmailService emailService, IDashboardService dashboardService, IRequestWiseFilesRepository requestWiseFilesRepository, IOrderService orderService, IUserService userService, IRequestService requestService, IRecordsRepository recordsRepository)
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
            dash.pagedList = await _adminDashboardService.GetRequestsByStatus(status, int.Parse(reqtype), int.Parse(pageNumber), int.Parse(region), search, false,physician.Physicianid);
            
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

        public IActionResult ViewCase(int requestid)
        {
            var dash = new AdminDashboard();

            dash.regions = _adminDashboardService.GetAllRegions();
            dash.request = _adminDashboardService.GetRequestById(requestid);
            return View("ViewCase", dash);
        }

        public IActionResult ViewNotes(int requestid)
        {
            var dash = _adminDashboardService.GetNotes(requestid);
            return PartialView("ViewNotes", dash);
        }
        public JsonResult UpdateNotes(int requestid, string notes)
        {
            var dash = _adminDashboardService.UpdateNotes(requestid, notes,true);
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
            return PartialView(viewUploads);
        }

        public JsonResult ConcludeRequest(int Requestid)
        {
            _requestService.TransferCase(Requestid, "", 8);
            return Json(new { success = true});
        }
    }
}
