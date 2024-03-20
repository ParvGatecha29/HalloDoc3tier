using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocDAL.Model;
using HalloDocBAL.Interfaces;
using HalloDocDAL.Models;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using System.Net.Mime;
using Microsoft.AspNetCore.Routing.Constraints;
using HalloDocDAL.Repositories;
using System.Globalization;
using Rotativa.AspNetCore;
using System.Security.Policy;
using OfficeOpenXml;

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
    int[] newcase = { 1 };
    int[] pendingcase = { 2 };
    int[] activecase = { 4, 5 };
    int[] concludecase = { 6 };
    int[] toclosecase = { 3, 7, 8 };
    int[] unpaidcase = { 9 };
    public AdminDashboardController(IAdminDashboardService adminDashboardService, IEmailService emailService, IDashboardService dashboardService, IRequestWiseFilesRepository requestWiseFilesRepository, IOrderService orderService, IUserService userService, IRequestService requestService)
    {
        _adminDashboardService = adminDashboardService;
        _emailService = emailService;
        _dashboardService = dashboardService;
        _requestWiseFilesRepository = requestWiseFilesRepository;
        _orderService = orderService;
        _userService = userService;
        _requestService = requestService;
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
        return View("ViewCase", dash);
    }
    public IActionResult ViewNotes(int requestid)
    {
        var dash = _adminDashboardService.GetNotes(requestid);
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

    public IActionResult Provider()
    {
        return View();
    }
}

