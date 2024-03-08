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

namespace HalloDoc.Controllers;
[AuthManager("1")]
public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IEmailService _emailService;
    private readonly IDashboardService _dashboardService;
    private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
    private readonly IOrderService _orderService;
    int[] newcase = { 1 };
    int[] pendingcase = { 2 };
    int[] activecase = { 4,5 };
    int[] concludecase = { 6 };
    int[] toclosecase = { 3,7,8 };
    int[] unpaidcase = { 9 };
    public AdminDashboardController(IAdminDashboardService adminDashboardService, IEmailService emailService, IDashboardService dashboardService, IRequestWiseFilesRepository requestWiseFilesRepository, IOrderService orderService)
    {
        _adminDashboardService = adminDashboardService;
        _emailService = emailService;
        _dashboardService = dashboardService;
        _requestWiseFilesRepository = requestWiseFilesRepository;
        _orderService = orderService;
    }

   
    public IActionResult AdminDashboard()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequests();
        dash.regions = _adminDashboardService.GetAllRegions();
        return View(dash);
    }
    public IActionResult NewCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 1;
        dash.Data = _adminDashboardService.GetRequestsByStatus(newcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable",dash);
    }
    public IActionResult PendingCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 2;
        dash.Data = _adminDashboardService.GetRequestsByStatus(pendingcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ActiveCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 3;
        dash.Data = _adminDashboardService.GetRequestsByStatus(activecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ConcludeCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 4;
        dash.Data = _adminDashboardService.GetRequestsByStatus(concludecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ToCloseCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 5;
        dash.Data = _adminDashboardService.GetRequestsByStatus(toclosecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult UnpaidCasePartial()
    {
        var dash = new AdminDashboard();
        dash.state = 6;
        dash.Data = _adminDashboardService.GetRequestsByStatus(unpaidcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
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
        return View("ViewNotes", dash);
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

    public JsonResult BlockCase(int Requestid,string info)
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

            filesToSend.Add(Path.Combine("wwwroot","documents",requestid.ToString(),file.Filename));
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
        return Json(new {success = true});
    }

    public JsonResult SendAgreement(IFormCollection formcollection)
    {
        var email = formcollection["sendemail"];
        var requestid = formcollection["requestId"];
        var link = "https://localhost:44319/Home/Agreement?RequestId="+requestid;
        var subject = "Agreement";
        var body = $"You can view your agreement <a href='{link}'>here</a>.";

        _emailService.SendEmail(email, subject, body);
        return Json(new {success = true});
    }
}
