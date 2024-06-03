using HalloDocBAL.Interfaces;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;

namespace HalloDoc.Controllers;

public class PatientDashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public PatientDashboardController(IDashboardService dashboardService, IUserService userService, IJwtService jwtService)
    {
        _dashboardService = dashboardService;
        _userService = userService;
        _jwtService = jwtService;
    }

    [AuthManager("2")]
    public async Task<IActionResult> PatientDashboard()
    {
        var email = HttpContext.Session.GetString("email");
        ViewData["email"] = email;
        if (email == null)
        {
            return RedirectToAction("PatientLogin", "RegisterPatient");
        }
        CustomModel model = new CustomModel();
        model.user = await _userService.GetUser(email);
        model.dashboards = _dashboardService.PatientDashboard(email);
        return View(model);
    }

    public async Task<IActionResult> Patientmerequest()
    {
        var email = HttpContext.Session.GetString("email");
        ViewData["email"] = email;
        User user = new User();
        user = await _userService.GetUser(email);
        return View(user);
    }

    public async Task<IActionResult> ViewDocument(int id)
    {
        ViewBag.rid = id;
        var email = HttpContext.Session.GetString("email");
        CustomModel model = new CustomModel();
        model.user = await _userService.GetUser(email);
        model.dashboards = _dashboardService.PatientDashboard(email);
        model.requests = _dashboardService.ViewDocument(id);
        return View(model);
    }

    public async Task<JsonResult> UploadDocument(IFormCollection formCollection, int rid)
    {
        var formFiles = formCollection.Files;
        Debug.WriteLine(formFiles.Count);
        Debug.WriteLine(rid);
        var i = await _dashboardService.UploadDocument(formFiles, rid);
        if (i != null)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public async Task<ActionResult> DownDoc([FromBody] string[] selectedDocuments)
    {
        var tempFileName = Guid.NewGuid().ToString() + ".zip";
        var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

        using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Create))
        {
            foreach (var document in selectedDocuments)
            {
                var doc = await _dashboardService.GetDocument(document);
                Debug.WriteLine(doc.Filename);
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", doc.Requestid.ToString());
                if (doc != null)
                {
                    var rid = doc.Requestid;
                    var docPath = Path.Combine(uploadsFolderPath, doc.Filename);
                    int indexOf = doc.Filename.LastIndexOf("\\") + 1;
                    var fileName = doc.Filename[indexOf..];
                    zip.CreateEntryFromFile(docPath, fileName);
                }
            }
        }
        TempData["DownloadToken"] = tempFileName;

        return Json(new { token = tempFileName });
    }

    public ActionResult Download(string token)
    {
        var tempFilePath = Path.Combine(Path.GetTempPath(), token);
        Debug.WriteLine(System.IO.File.Exists(tempFilePath));
        if (System.IO.File.Exists(tempFilePath))
        {
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            return File(bytes, "application/zip", "Documents.zip");
        }
        return NotFound();
    }

    public async Task<ActionResult> DownloadOne(string id)
    {
        var document = await _dashboardService.GetDocument(id);
        if (document != null)
        {
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", document.Requestid.ToString());
            var filePath = Path.Combine(uploadsFolderPath, document.Filename);
            if (System.IO.File.Exists(filePath))
            {
                var bytes = System.IO.File.ReadAllBytes(filePath);
                string contentType = "application/octet-stream";
                return File(bytes, contentType, document.Filename);
            }
            return NotFound();
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<JsonResult> EditProfile([FromBody] User model)
    {

        await _userService.EditUser(model);

        return Json(new { success = true, redirectUrl = Url.Action("PatientDashboard", "PatientDashboard") });

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult LoadChatAdmin(int Adminid)
    {
        var token = Request.Cookies["jwt"];
        var aspnetid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        ChatModel model = _dashboardService.GetChatAdmin(Adminid, aspnetid);

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

        ChatModel model = _dashboardService.GetChatPhysician(Physicianid, aspnetid);

        return PartialView("_chat", model);
    }

    public IActionResult LoadGroupChat(int Adminid, int Physicianid)
    {
        var token = Request.Cookies["jwt"];
        var aspnetid = "";
        if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
        {
            aspnetid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value;
        }

        ChatModel model = _dashboardService.GetGroupChat(Adminid, Physicianid, aspnetid);

        return PartialView("_groupChat", model);
    }
}
