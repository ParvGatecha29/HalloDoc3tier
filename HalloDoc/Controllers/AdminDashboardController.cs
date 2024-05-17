using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Transactions;

namespace HalloDoc.Controllers;
[AuthManager("1")]
public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IEmailService _emailService;
    private readonly IDashboardService _dashboardService;
    private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRecordsRepository _recordsRepository;
    private readonly IJwtService _jwtService;
    int[] newcase = { 1 };
    int[] pendingcase = { 2 };
    int[] activecase = { 4, 5 };
    int[] concludecase = { 6 };
    int[] toclosecase = { 3, 7, 8 };
    int[] unpaidcase = { 9 };
    public AdminDashboardController(IJwtService jwtService,IRequestRepository requestRepository, IUserRepository userRepository, IAdminDashboardService adminDashboardService, IEmailService emailService, IDashboardService dashboardService, IRequestWiseFilesRepository requestWiseFilesRepository, IOrderService orderService, IUserService userService, IRequestService requestService, IRecordsRepository recordsRepository)
    {
        _requestRepository = requestRepository;
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
  
    public IActionResult AdminDashboard()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequests();
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
            HttpContext.Session.SetString("pageNumber", "1");
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
            case 5:
                status = new int[] { 3, 7, 8 };
                break;
            case 6:
                status = new int[] { 9 };
                break;
            default: status = new int[] { 1 }; break;

        }

        dash.pagedList = await _adminDashboardService.GetRequestsByStatus(status, int.Parse(reqtype), int.Parse(pageNumber), int.Parse(region), search);

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

    public IActionResult CreateRequest()
    {
        return PartialView();
    }

    public IActionResult ViewCase(int requestid)
    {
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
        if (dash == null)
        {
            return View("Invalid");
        }
        return PartialView("ViewNotes", dash);
    }

    public JsonResult UpdateNotes(int requestid, string notes)
    {
        var dash = _adminDashboardService.UpdateNotes(requestid, notes);
        return Json(new { success = true });
    }

    public JsonResult CancelCase(int Requestid, string reason, string info)
    {
        var dash = new AdminDashboardData
        {
            requestId = Requestid,
            reason = reason,
            notes = info
        };
        _adminDashboardService.CancelRequest(dash);
        return Json(new { success = true });
    }

    public ActionResult<IEnumerable<Physician>> GetPhysiciansByRegion(int region)
    {
        var physicians = _adminDashboardService.GetPhysiciansByRegion(region);
        return physicians;
    }

    public JsonResult AssignCase(int Requestid, int physicianSelect, string info)
    {
        var dash = new AdminDashboardData
        {
            requestId = Requestid,
            physicianId = physicianSelect,
            notes = info
        };
        _adminDashboardService.AssignRequest(dash);
        return Json(new { success = true });
    }

    public JsonResult AgreeCase(int Requestid)
    {
        var dash = new AdminDashboardData
        {
            requestId = Requestid,
        };
        _adminDashboardService.AgreeRequest(dash);
        return Json(new { success = true });
    }

    public JsonResult BlockCase(int Requestid, string info)
    {
        var dash = new AdminDashboardData
        {
            requestId = Requestid,
            notes = info
        };
        _adminDashboardService.BlockRequest(dash);
        return Json(new { success = true });
    }

    public JsonResult ClearCase(int requestid)
    {
        var dash = new AdminDashboardData
        {
            requestId = requestid,
            notes = ""
        };
        _adminDashboardService.ClearRequest(dash);
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

    public JsonResult DeleteFile(int fileid)
    {
        _adminDashboardService.DeleteFile(fileid);
        return Json(new { success = true });
    }

    public async Task<JsonResult> SendFiles(List<string> selectedDocuments, int requestid, string email)
    {
        var subject = "Submit Request";
        var body = $"Here are your selected files";

        var filesToSend = new List<string>();
        foreach (var document in selectedDocuments)
        {
            var file = await _requestWiseFilesRepository.GetFile(document);

            filesToSend.Add(Path.Combine("wwwroot", "documents", requestid.ToString(), file.Filename));
        }

        _emailService.SendEmailWithAttachment(email, subject, body, filesToSend);
        return Json(new { success = true });
    }

    public IActionResult Orders(int requestid)
    {
        var orders = new Orders();
        orders.requestid = requestid;
        orders.healthprofessionaltypes = _orderService.GetAllPrfessions();
        if (orders == null)
        {
            return View("Invalid");
        }
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

    public IActionResult CloseCase(int requestid)
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

    public IActionResult ConfirmClose(int requestid)
    {
        var dash = new AdminDashboardData
        {
            requestId = requestid,
            notes = ""
        };
        _adminDashboardService.CloseRequest(dash);
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

    public IActionResult AdminProfile()
    {
        Admin admin = _adminDashboardService.GetAdminById(HttpContext.Session.GetString("userId"));

        AdminProfile profile = new AdminProfile
        {
            adminId = admin.Adminid,
            Username = admin.Firstname,
            Status = admin.Status.ToString(),
            Role = admin.Roleid.ToString(),
            FirstName = admin.Firstname,
            LastName = admin.Lastname,
            Email = admin.Email,
            ConfirmEmail = admin.Email,
            Phone = admin.Mobile,
            Address1 = admin.Address1,
            Address2 = admin.Address2,
            City = admin.City,
            Zip = admin.Zip,
            RegionId = admin.Regionid,
            Phone1 = admin.Altphone
        };
        profile.regions = _adminDashboardService.GetAllRegions();
        profile.adminregions = _adminDashboardService.GetAdminRegions(HttpContext.Session.GetString("userId"));
        return View(profile);
    }

    public IActionResult EditProfile(AdminProfile profile)
    {
        _adminDashboardService.UpdateProfile(profile);
        return RedirectToAction("AdminProfile");
    }

    public async Task<JsonResult> SaveRequest(Req model)
    {
        model.month = model.dob.ToString("MMMM");
        model.date = model.dob.Day;
        model.year = model.dob.Year;
        var user = await _userService.CheckUser(model.email);
        if (user == null)
        {
            var link = "https://localhost:44319/Login/PatientCreate";
            var subject = "Register Yourself";
            var body = $"Please register yourself <a href='{link}'>here</a>.";

            _emailService.SendEmail(model.email, subject, body);
        }
        await _requestService.PatientRequest(model);

        return Json(new { success = true });
    }
    
    public async Task<IActionResult> Export(bool all)
    {
        var reqtype = HttpContext.Session.GetString("reqtype");
        var pageNumber = HttpContext.Session.GetString("pageNumber");
        var state = int.Parse(HttpContext.Session.GetString("state"));
        var region = HttpContext.Session.GetString("region") != null ? HttpContext.Session.GetString("region") : "0";
        var search = HttpContext.Session.GetString("search") != null ? HttpContext.Session.GetString("search") : "";
        if (reqtype == null)
        {
            reqtype = "0";
        }
        if (pageNumber == null)
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
            case 5:
                status = new int[] { 3, 7, 8 };
                break;
            case 6:
                status = new int[] { 9 };
                break;
            default: status = new int[] { 1 }; break;

        }
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        dash.pagedList = await _adminDashboardService.GetRequestsByStatus(status, int.Parse(reqtype), int.Parse(pageNumber), int.Parse(region), search, all);

        // Create a new Excel package
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            int row = 2;
            switch (state)
            {
                case 1:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Date of Birth";
                    worksheet.Cells[1, 3].Value = "Requestor";
                    worksheet.Cells[1, 4].Value = "Requested Date";
                    worksheet.Cells[1, 5].Value = "Phone";
                    worksheet.Cells[1, 6].Value = "Address";
                    worksheet.Cells[1, 7].Value = "Notes";

                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.dobdate + "/" + data.dobmonth + "/" + data.dobyear;
                        worksheet.Cells[row, 3].Value = data.requestor;
                        worksheet.Cells[row, 4].Value = data.reqdate;
                        worksheet.Cells[row, 5].Value = data.phone;
                        worksheet.Cells[row, 6].Value = data.address;
                        worksheet.Cells[row, 7].Value = data.notes;
                        row++;
                    }
                    break;
                case 2:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Date of Birth";
                    worksheet.Cells[1, 3].Value = "Requestor";
                    worksheet.Cells[1, 4].Value = "Physician Name";
                    worksheet.Cells[1, 5].Value = "Date of Service";
                    worksheet.Cells[1, 6].Value = "Phone";
                    worksheet.Cells[1, 7].Value = "Address";
                    worksheet.Cells[1, 8].Value = "Notes";

                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.dobdate + "/" + data.dobmonth + "/" + data.dobyear;
                        worksheet.Cells[row, 3].Value = data.requestor;
                        worksheet.Cells[row, 4].Value = data.physician;
                        worksheet.Cells[row, 5].Value = data.requestDate;
                        worksheet.Cells[row, 6].Value = data.phone;
                        worksheet.Cells[row, 7].Value = data.address;
                        worksheet.Cells[row, 8].Value = data.notes;
                        row++;
                    }
                    break;
                case 3:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Date of Birth";
                    worksheet.Cells[1, 3].Value = "Requestor";
                    worksheet.Cells[1, 4].Value = "Physician Name";
                    worksheet.Cells[1, 5].Value = "Date of Service";
                    worksheet.Cells[1, 6].Value = "Phone";
                    worksheet.Cells[1, 7].Value = "Address";
                    worksheet.Cells[1, 8].Value = "Notes";

                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.dobdate + "/" + data.dobmonth + "/" + data.dobyear;
                        worksheet.Cells[row, 3].Value = data.requestor;
                        worksheet.Cells[row, 4].Value = data.physician;
                        worksheet.Cells[row, 5].Value = data.requestDate;
                        worksheet.Cells[row, 6].Value = data.phone;
                        worksheet.Cells[row, 7].Value = data.address;
                        worksheet.Cells[row, 8].Value = data.notes;
                        row++;
                    }
                    break;
                case 4:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Date of Birth";
                    worksheet.Cells[1, 3].Value = "Physician Name";
                    worksheet.Cells[1, 4].Value = "Date of Service";
                    worksheet.Cells[1, 5].Value = "Phone";
                    worksheet.Cells[1, 6].Value = "Address";

                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.dobdate + "/" + data.dobmonth + "/" + data.dobyear;
                        worksheet.Cells[row, 3].Value = data.physician;
                        worksheet.Cells[row, 4].Value = data.requestDate;
                        worksheet.Cells[row, 5].Value = data.phone;
                        worksheet.Cells[row, 6].Value = data.address;
                        row++;
                    }
                    break;
                case 5:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Date of Birth";
                    worksheet.Cells[1, 3].Value = "Region";
                    worksheet.Cells[1, 4].Value = "Physician Name";
                    worksheet.Cells[1, 5].Value = "Date of Service";
                    worksheet.Cells[1, 6].Value = "Address";
                    worksheet.Cells[1, 7].Value = "Notes";

                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.dobdate + "/" + data.dobmonth + "/" + data.dobyear;
                        worksheet.Cells[row, 3].Value = data.regionId;
                        worksheet.Cells[row, 4].Value = data.physician;
                        worksheet.Cells[row, 5].Value = data.requestDate;
                        worksheet.Cells[row, 6].Value = data.address;
                        worksheet.Cells[row, 7].Value = data.notes;
                        row++;
                    }
                    break;
                case 6:
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Physician Name";
                    worksheet.Cells[1, 3].Value = "Date of Service";
                    worksheet.Cells[1, 4].Value = "Phone";
                    worksheet.Cells[1, 5].Value = "Address";


                    foreach (var data in dash.pagedList)
                    {
                        worksheet.Cells[row, 1].Value = data.firstName + data.lastName;
                        worksheet.Cells[row, 2].Value = data.physician;
                        worksheet.Cells[row, 3].Value = data.requestDate;
                        worksheet.Cells[row, 4].Value = data.phone;
                        worksheet.Cells[row, 5].Value = data.address;
                        row++;
                    }
                    break;
                default: break;
            }

            // Populate Excel sheet with data


            // Convert package to a byte array
            var excelBytes = package.GetAsByteArray();

            // Return Excel file as a download
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
        }
    }

    public IActionResult Provider(int regionid = 0)
    {
        Provider provider = new Provider();
        provider.physicians = _adminDashboardService.GetPhysiciansByRegion(regionid);
        provider.regions = _adminDashboardService.GetAllRegions();
        return View(provider);
    }

    public IActionResult ProviderByRegion(int regionid)
    {
        Provider prov = new Provider();
        prov.physicians = _adminDashboardService.GetPhysiciansByRegion(regionid);
        return PartialView("ProviderTable", prov);
    }

    public bool CurrentView(string view)
    {
        HttpContext.Session.SetString("view", view);
        return true;
    }
    
    public IActionResult EditProvider(int physicianid)
    {
        Provider prov = new Provider();
        prov.physician = _adminDashboardService.GetPhysiciansById(physicianid);
        prov.regions = _adminDashboardService.GetAllRegions();
        prov.phyregions = _adminDashboardService.GetPhysicianRegions(physicianid);
        if (prov.physician == null)
        {
            return View("Invalid");
        }
        return PartialView(prov);
    }

    public IActionResult CreateProvider()
    {
        Provider prov = new Provider();
        prov.regions = _adminDashboardService.GetAllRegions();
        return PartialView("CreatePhysician", prov);
    }

    public JsonResult SaveProvider(Provider model)
    {
        var provider = _adminDashboardService.GetPhysiciansByEmail(model.physician.Email);
        if (provider != null)
        {
            return Json(new { success = false });
        }
        var user = SessionService.GetLoggedInUser(HttpContext.Session);
        var adminId = user.Id;
        var add = _adminDashboardService.AddProvider(model, adminId);

        if (add)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    public JsonResult DeleteProvider(int id)
    {
        _userRepository.DeleteProvider(id);
        return Json(new { success = true });
    }
    public JsonResult EditPhysician(Provider model)
    {
        var edit = _adminDashboardService.EditPhysician(model);
        return Json(new { success = true });
    }

    [AuthManager("1", "ProviderLocation")]
    public IActionResult ProviderLocation()
    {
        return View();
    }

    public List<Physicianlocation> GetProviders()
    {
        return _adminDashboardService.GetProviders();
    }

    public IActionResult Access()
    {
        var access = new Access();
        access.Roles = _adminDashboardService.GetRoles();
        return View(access);
    }

    public IActionResult CreateAccess(int id = 0)
    {
        var access = new Access();
        access.Roles = _adminDashboardService.GetRoles();
        if (id != 0)
        {
            access.edit = 1;
            access.roleid = id;
            access.roleName = _adminDashboardService.GetRoles().Where(x => x.Roleid == id).FirstOrDefault().Name;
        }
        return PartialView(access);
    }

    public IActionResult AccessMenu(int AccountType, int roleid = 0)
    {
        var access = new Access();
        access.Menus = _adminDashboardService.GetMenus(AccountType);
        access.RoleMenus = _adminDashboardService.GetRoleMenus(roleid);
        return PartialView(access);
    }

    public JsonResult CreateAccessRole(Access model)
    {
        var user = SessionService.GetLoggedInUser(HttpContext.Session);
        model.userid = user.Id;
        _adminDashboardService.CreateAccess(model);
        return Json(new { success = true });
    }

    public JsonResult EditCase(AdminDashboard model)
    {
        _adminDashboardService.EditCase(model);
        return Json(new { success = true });
    }

    public IActionResult UserAccess()
    {
        Access access = new Access();
        access.Users = _adminDashboardService.GetAllUsers();
        access.Users = access.Users.Where(x => (x.Aspnetuser != null ? x.Aspnetuser.Aspnetuserroles.Any(y => y.RoleId == "1") : false) || (x.Aspnetuser != null ? x.Aspnetuser.Aspnetuserroles.Any(y => y.RoleId == "2") : false)).ToList();
        return View(access);
    }

    public IActionResult Scheduling()
    {
        if (HttpContext.Session.GetString("view") == null)
        {
            HttpContext.Session.SetString("view", "resourceTimelineDay");
        }
        var region = _adminDashboardService.GetAllRegions();
        ViewBag.regions = region;
        ScheduleModel model = new ScheduleModel();
        model.regions = region;
        return View(model);
    }

    public IActionResult CreateShift(ScheduleModel model)
    {
        var user = SessionService.GetLoggedInUser(HttpContext.Session);
        Admin? admin = _adminDashboardService.GetAdminById(user.Id);
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
            shift.Createdby = admin.Aspnetuserid;
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
    public IActionResult GetEvents(int region)
    {
        var mappedEvents = _userRepository.GetMappedEvents(region);

        return Ok(mappedEvents);
    }

    [HttpPost]
    public IActionResult SaveShift(int shiftDetailId, DateTime startDate, TimeOnly startTime, TimeOnly endTime)
    {
        // Find the shift detail by its ID
        Shiftdetail? shiftdetail = _userRepository.FindShiftDetails(shiftDetailId);
        var user = SessionService.GetLoggedInUser(HttpContext.Session);
        Admin? admin = _adminDashboardService.GetAdminById(user.Id);

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
            shiftdetail.Modifiedby = user.aspId;
            shiftdetail.Modifieddate = DateTime.Now;

            // Update the database
            _userRepository.UpdateShiftDetails(shiftdetail);
            return RedirectToAction("GetEvents");
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
        return RedirectToAction("GetEvents");

    }

    [HttpPost]
    public IActionResult ApprovedShifts(List<int> selectedIds)
    {
        try
        {
            foreach (var id in selectedIds)
            {
                var shiftDetail = _userRepository.FindShiftDetails(id);
                if (shiftDetail != null)
                {
                    shiftDetail.Status = 0; // Change the state to 0
                    _userRepository.UpdateShiftDetails(shiftDetail);
                }
            }

            return Ok("Selected shifts have been successfully approved.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while approving selected shifts: " + ex.Message);
        }
    }

    public IActionResult ProviderOnCall()
    {
        List<HalloDocDAL.Models.Region> regions = _adminDashboardService.GetAllRegions();
        return PartialView(regions);
    }

    public IActionResult MDOnCall(int regionId)
    {
        List<Physician> physicians = _adminDashboardService.GetPhysiciansByRegion(regionId);
        return PartialView(physicians);
    }

    public IActionResult ShiftForReview()
    {
        ScheduleModel schedule = new ScheduleModel();
        schedule.regions = _adminDashboardService.GetAllRegions();
        return PartialView(schedule);
    }

    public IActionResult RequestedShiftTable(int RegionId, bool currentMonth)
    {
        ScheduleModel schedule = new ScheduleModel();
        schedule.DayList = _userRepository.GetEvents(RegionId, currentMonth);
        return PartialView(schedule);
    }

    public IActionResult Partners()
    {
        Partner partner = new Partner();
        partner.Professions = _userRepository.GetHealthProfessionalTypes();
        return View(partner);
    }

    public IActionResult GetVendors(int Profession, string VendorSearch)
    {
        List<Partner> vendorList = _userRepository.GetVendors(Profession, VendorSearch);
        return PartialView("VendorTable", vendorList);
    }

    public IActionResult AddBusiness()
    {
        ViewData["ViewName"] = "Partners";
        var regions = _adminDashboardService.GetAllRegions();
        var professions = _userRepository.GetHealthProfessionalTypes();
        ViewBag.professions = professions;
        ViewBag.regions = regions;
        return PartialView();
    }

    public IActionResult EditBusiness(int vendorid)
    {
        ViewData["ViewName"] = "Partners";
        BusinessModel business = _userRepository.EditBusiness(vendorid);
        var regions = _adminDashboardService.GetAllRegions();
        var professions = _userRepository.GetHealthProfessionalTypes();
        ViewBag.professions = professions;
        ViewBag.regions = regions;
        return PartialView(business);
    }

    public IActionResult CreateBusiness(BusinessModel model)
    {
        bool result = _userRepository.AddBusiness(model);
        return RedirectToAction("Partners", "AdminDashboard");
    }

    public IActionResult UpdateBusiness(BusinessModel model)
    {
        bool result = _userRepository.UpdateBusiness(model);
        return RedirectToAction("Partners", "AdminDashboard");
    }

    public bool DeleteVendor(int vendorid)
    {
        return _userRepository.DeleteBusiness(vendorid);
    }

    public IActionResult SearchRecords()
    {
        return View();
    }

    public void ChangePageSearch(int pageNumber)
    {
        if (pageNumber != 0)
        {
            HttpContext.Session.SetString("pagesearch", pageNumber.ToString());
        }
    }

    public void ClearSearchPage()
    {
        HttpContext.Session.SetString("pagesearch", "1");
    }

    public async Task<IActionResult> GetSearchRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS)
    {
        int pageNumber = int.Parse(HttpContext.Session.GetString("pagesearch") ?? "1");
        PagedList<SearchRecord> records = await _recordsRepository.GetSearchRecords(Email, FromDoS, Phone, Patient, Provider, RequestStatus, RequestType, ToDoS, pageNumber);
        return PartialView("SearchRecordsPartial", records);
    }

    public JsonResult DeletePatientRequest(int requestid)
    {
        if (_recordsRepository.DeletePatientRequest(requestid))
        {
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    public void ChangePagePatient(int pageNumber)
    {
        HttpContext.Session.SetString("pagepatient", pageNumber.ToString());
    }

    public void ChangePageBlock(int pageNumber)
    {
        HttpContext.Session.SetString("pagepatient", pageNumber.ToString());
    }

    public IActionResult PatientHistory()
    {
        return View();
    }

    public async Task<IActionResult> GetPatientRecords(string? FirstName, string? LastName, string? Phone, string? Email)
    {
        List<PatientHistory> records = new List<PatientHistory>();
        int pageNumber = int.Parse(HttpContext.Session.GetString("pagepatient") ?? "1");
        records = await _recordsRepository.GetPatients(FirstName, LastName, Phone, Email, pageNumber);
        return PartialView("PatientHistoryPartial", records);
    }

    public IActionResult ExplorePatient(int userid)
    {
        List<PatientHistory> records = new List<PatientHistory>();
        records = _recordsRepository.GetPatientRequests(userid);
        return PartialView("ExplorePatient", records);

    }

    public IActionResult BlockHistory()
    {
        return View();
    }

    public async Task<IActionResult> GetBlockedPatients(string? FirstName, string? LastName, string? Phone, string? Email)
    {
        int pageNumber = int.Parse(HttpContext.Session.GetString("pageblock") ?? "1");
        PagedList<PatientHistory> records = await _recordsRepository.GetBlockedPatients(FirstName, LastName, Phone, Email, pageNumber);
        return PartialView("BlockHistoryPartial", records);
    }

    public IActionResult ExportRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<SearchRecord> records = _recordsRepository.GetSearchRecordsAll();
        if (RequestStatus != 0)
        {
            records = records.Where(r => r.RequestStatus == RequestStatus).ToList();
        }
        if (Patient != null)
        {
            records = records.Where(r => r.PatientName.Contains(Patient)).ToList();
        }
        if (RequestType != 0)
        {
            records = records.Where(r => r.RequestTypeId == RequestType).ToList();
        }
        if (Provider != null)
        {
            records = records.Where(r => r.PhysicianName.Contains(Provider)).ToList();
        }
        if (Email != null)
        {
            records = records.Where(r => r.Email == Email).ToList();
        }
        if (Phone != null)
        {
            records = records.Where(r => r.PhoneNumber == Phone).ToList();
        }

        // Create a new Excel package
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            int row = 2;

            worksheet.Cells[1, 1].Value = "Patient Name";
            worksheet.Cells[1, 2].Value = "Requestor";
            worksheet.Cells[1, 3].Value = "Date of Service";
            worksheet.Cells[1, 4].Value = "Close Case Date";
            worksheet.Cells[1, 5].Value = "Email";
            worksheet.Cells[1, 6].Value = "Phone Number";
            worksheet.Cells[1, 7].Value = "Address";
            worksheet.Cells[1, 8].Value = "ZipCode";
            worksheet.Cells[1, 9].Value = "Request Status";
            worksheet.Cells[1, 10].Value = "Physician";
            worksheet.Cells[1, 11].Value = "Physician Notes";
            worksheet.Cells[1, 12].Value = "Cancelled by Provider Notes";
            worksheet.Cells[1, 13].Value = "Admin Notes";
            worksheet.Cells[1, 14].Value = "Patient Notes";

            foreach (var data in records)
            {
                worksheet.Cells[row, 1].Value = data.PatientName;
                worksheet.Cells[row, 2].Value = data.Requestor;
                worksheet.Cells[row, 3].Value = data.DateOfService;
                worksheet.Cells[row, 4].Value = data.DateofClose;
                worksheet.Cells[row, 5].Value = data.Email;
                worksheet.Cells[row, 6].Value = data.PhoneNumber;
                worksheet.Cells[row, 7].Value = data.Address;
                worksheet.Cells[row, 8].Value = data.Zip;
                worksheet.Cells[row, 9].Value = data.StatusName;
                worksheet.Cells[row, 10].Value = data.PhysicianName;
                worksheet.Cells[row, 11].Value = data.PhysicianNote;
                worksheet.Cells[row, 12].Value = data.CancelledByProvidor;
                worksheet.Cells[row, 13].Value = data.AdminNotes;
                worksheet.Cells[row, 14].Value = data.PatientNote;
                row++;
            }


            // Populate Excel sheet with data


            // Convert package to a byte array
            var excelBytes = package.GetAsByteArray();

            // Return Excel file as a download
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
        }
    }

    public JsonResult UnblockRequest(int requestid)
    {
        _requestService.UnblockRequest(requestid);
        return Json(new { success = true });
    }

    public IActionResult EmailLog()
    {
        return View();
    }

    public async Task<IActionResult> LoadEmailLogTable(int roleid, string receiverName, string Email, DateTime createdDate, DateTime sentDate, int pageNumber)
    {
        var logs = await _requestRepository.GetEmailLogs(roleid, receiverName, Email, createdDate, sentDate, pageNumber);
        return PartialView("EmailLogTable", logs);
    }

    public IActionResult CreateAdmin()
    {
        AdminProfile profile = new AdminProfile();
        profile.roles = _adminDashboardService.GetRoles();
        profile.regions = _adminDashboardService.GetAllRegions();
        profile.regions = _adminDashboardService.GetAllRegions();
        return View(profile);
    }

    public JsonResult RegisterAdmin(AdminProfile model)
    {
        var user = SessionService.GetLoggedInUser(HttpContext.Session);
        model.aspId = user.Id;
        _userRepository.RegisterAdmin(model);
        return Json(new { success = true, });
    }

    public async Task<IActionResult> EditUser(string id)
    {
        Aspnetuser user = await _userRepository.FindById(id);
        if (user != null)
        {
            if (user.Aspnetuserroles.Any(x => x.RoleId == "1"))
            {
                Admin admin = _adminDashboardService.GetAdminById(id);

                AdminProfile profile = new AdminProfile
                {
                    adminId = admin.Adminid,
                    Username = admin.Firstname,
                    Status = admin.Status.ToString(),
                    Role = admin.Roleid.ToString(),
                    FirstName = admin.Firstname,
                    LastName = admin.Lastname,
                    Email = admin.Email,
                    ConfirmEmail = admin.Email,
                    Phone = admin.Mobile,
                    Address1 = admin.Address1,
                    Address2 = admin.Address2,
                    City = admin.City,
                    Zip = admin.Zip,
                    RegionId = admin.Regionid,
                    Phone1 = admin.Altphone
                };
                profile.regions = _adminDashboardService.GetAllRegions();
                profile.adminregions = _adminDashboardService.GetAdminRegions(HttpContext.Session.GetString("userId"));
                return PartialView("EditAdmin", profile);
            }
            if (user.Aspnetuserroles.Any(x => x.RoleId == "2"))
            {
                var physician = _adminDashboardService.GetPhysiciansByAspId(id);
                Provider prov = new Provider();
                prov.physician = _adminDashboardService.GetPhysiciansById(physician.Physicianid);
                prov.regions = _adminDashboardService.GetAllRegions();
                prov.phyregions = _adminDashboardService.GetPhysicianRegions(physician.Physicianid);
                if (prov.physician == null)
                {
                    return View("Invalid");
                }
                return PartialView("EditProvider", prov);
            }
        }
        return NotFound();

    }

    public JsonResult DeleteRole(int id)
    {
        if (_userRepository.DeleteRole(id))
        {
            return Json(new { success = true, });
        }
        return Json(new { success = false, });
    }

    public JsonResult ContactProvider(int id, string message)
    {
        Physician physician = _adminDashboardService.GetPhysiciansById(id);
        _emailService.SendEmail(physician.Email, "Admin Message", message);
        return Json(new { success = true });
    }

    public IActionResult EnterPayRate(int id)
    {
        Payrate model = new Payrate();
        model = _userService.GetPayrateById(id);
        return PartialView("Payrate", model);
    }

    public JsonResult SavePayrate(int Physicianid, int rate, int type)
    {
        var suc = _userService.SavePayRate(Physicianid,rate, type);
        return Json(new {success = suc});
    }

    public IActionResult Invoicing()
    {
        List<Physician> list = new List<Physician>();
        list = _adminDashboardService.GetPhysiciansByRegion(0);

        return View("Invoicing", list);
    }

    public IActionResult SearchDataByRange(DateTime startDate, int Physicianid)
    {
        InvoicingView model = new InvoicingView();
        model.timesheets = _adminDashboardService.SearchDataByRangeTimeSheet(startDate, Physicianid);
        model.invoicing = _adminDashboardService.SearchDataByRangeReimbursement(startDate, Physicianid);
        model.Sheet = _adminDashboardService.CheckApproved(startDate, Physicianid);
        model.PhysicianId = Physicianid;

        return PartialView("_timeSheet", model);
    }

    public IActionResult LoadApproveInvoicing(int Id, int PhysicianId, string startDate)
    {
        var date = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        InvoicingView model = new InvoicingView();
        model.invoicing = _adminDashboardService.SearchDataById(Id);
        model.Physicianpayrate = _adminDashboardService.GetPayrateData(PhysicianId);
        model.Sheet = _adminDashboardService.CheckApproved(date, PhysicianId);
        model.PhysicianId = PhysicianId;

        return PartialView("_approveInvoicing", model);
    }

    public IActionResult SaveTimeSheet(List<InvoicingModel> invoicingModels, int Physicianid)
    {
        var token = Request.Cookies["jwt"];
        var aspuserid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        _adminDashboardService.SaveTimeSheet(invoicingModels, Physicianid, aspuserid);
        return Json(new { success = true });
    }

    public IActionResult ApproveTimesheet(DateTime startDate, int Physicianid, int bonus, string adminDescription)
    {
        var token = Request.Cookies["jwt"];
        var aspuserid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        _adminDashboardService.ApproveTimesheet(startDate, Physicianid, aspuserid, bonus, adminDescription);
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

        ChatModel model = _adminDashboardService.getChatPatient(Patientid, aspnetid);

        return PartialView("_chat", model);
    }

    public IActionResult LoadChatPhysician(int Physicianid)
    {
        var token = Request.Cookies["jwt"];
        var aspnetid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        ChatModel model = _adminDashboardService.getChatPhysician(Physicianid, aspnetid);

        return PartialView("_chat", model);
    }

    public IActionResult LoadGroupChat(int Patientid, int Physicianid)
    {
        var token = Request.Cookies["jwt"];
        var aspnetid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        ChatModel model = _adminDashboardService.GetGroupChat(Patientid, Physicianid, aspnetid);

        return PartialView("_groupChat", model);
    }
}

