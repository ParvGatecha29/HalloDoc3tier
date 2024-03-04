using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocDAL.Model;
using HalloDocBAL.Interfaces;
using HalloDocDAL.Models;
using HalloDocBAL.Services;

namespace HalloDoc.Controllers;

public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IEmailService _emailService;
    int[] newcase = { 1 };
    int[] pendingcase = { 2 };
    int[] activecase = { 4,5 };
    int[] concludecase = { 6 };
    int[] toclosecase = { 3,7,8 };
    int[] unpaidcase = { 9 };
    public AdminDashboardController(IAdminDashboardService adminDashboardService, IEmailService emailService)
    {
        _adminDashboardService = adminDashboardService;
        _emailService = emailService;
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
        dash.Data = _adminDashboardService.GetRequestsByStatus(newcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable",dash);
    }
    public IActionResult PendingCasePartial()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequestsByStatus(pendingcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ActiveCasePartial()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequestsByStatus(activecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ConcludeCasePartial()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequestsByStatus(concludecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ToCloseCasePartial()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequestsByStatus(toclosecase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult UnpaidCasePartial()
    {
        var dash = new AdminDashboard();
        dash.Data = _adminDashboardService.GetRequestsByStatus(unpaidcase);
        dash.regions = _adminDashboardService.GetAllRegions();
        return PartialView("_CaseTable", dash);
    }
    public IActionResult ViewCase(int requestid)
    {
        var dash = _adminDashboardService.GetRequestById(requestid);
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

    public JsonResult SendLink(IFormCollection formcollection)
    {
        var email = formcollection["email"];
        var link = "https://localhost:44319/SubmitRequest/SubmitRequest";
        var subject = "Submit Request";
        var body = $"You can submit a request <a href='{link}'>here</a>.";

        _emailService.SendEmail(email, subject, body);
        return Json(new { success = true });
    }
}
