using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using HalloDocDAL.Models;
using HalloDocDAL.Model;
using HalloDocBAL.Interfaces;

namespace HalloDoc.Controllers;

public class SubmitRequestController : Controller
{
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly IEmailService _emailService;
    private readonly IAdminDashboardService _adminDashboardService;

    public SubmitRequestController(IUserService userService, IRequestService requestService, IEmailService emailService, IAdminDashboardService adminDasbhoardService)
    {
        _userService = userService;
        _requestService = requestService;
        _emailService = emailService;
        _adminDashboardService = adminDasbhoardService;
    }

    public IActionResult SubmitRequest()
    {
        return View();
    }

    public IActionResult PatientRequest()
    {
        var region = _adminDashboardService.GetAllRegions();
        ViewBag.regions = region;
        return View();
    }

    public IActionResult ConciergeRequest()
    {
        var region = _adminDashboardService.GetAllRegions();
        ViewBag.regions = region;
        return View();
    }

    public IActionResult BusinessRequest()
    {
        var region = _adminDashboardService.GetAllRegions();
        ViewBag.regions = region;
        return View();
    }

    public IActionResult FamilyRequest()
    {
        var region = _adminDashboardService.GetAllRegions();
        ViewBag.regions = region;
        return View();
    }

    [HttpPost]
    public async Task<bool> CheckEmailExists(string email)
    {
        var emailExists = await _userService.CheckUser(email);
        if (emailExists != null)
        {
            return true;
        }
        return false;
    }

    [HttpPost]
    public async Task<JsonResult> PatRequest(IFormCollection formcollection, Req model)
    {
        model.email = formcollection["patientEmail"];
        model.cfirstName = formcollection["cfname"];
        model.clastName = formcollection["clname"];
        model.cphone = formcollection["ctel"];
        model.cemail = formcollection["cemail"];
        model.propbus = formcollection["cprop"];
        model.symptoms = formcollection["symptoms"];
        model.firstName = formcollection["fname"];
        model.lastName = formcollection["lname"];
        var d = DateTime.Parse(formcollection["dob"]);
        model.year = d.Year;
        model.month = d.ToString("MMMM");
        model.date = d.Day;
        model.password = formcollection["password"];
        model.phone = formcollection["patientTel"];
        model.street = formcollection["street"];
        model.city = formcollection["city"];
        model.state = formcollection["state"];
        model.zipcode = formcollection["zipcode"];
        model.room = formcollection["roomNum"];
        model.typeid = 1;
        model.document = formcollection.Files;
       
        if (!_userService.IsUserBlocked(model.email, model.phone))
        {
            var aspuser = new Aspnetuser();
            var user = new User();
            if (!(await CheckEmailExists(model.email)))
            {
                var reg = new Register
                {
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    Date = model.date,
                    Month = model.month,
                    Year = model.year,
                    Phone = model.phone,
                    State = model.state,
                    City = model.city,
                    Street = model.street,
                    Zipcode = model.zipcode,
                    Email = model.email,
                    password = model.password
                };
                var result = await _userService.SignUp(reg);
                aspuser = await _userService.CheckUser(model.email);

                user.Email = model.email;
                user.Firstname = model.firstName;
                user.Lastname = model.lastName;
                user.Aspnetuserid = aspuser.Id;
                user.Street = model.street;
                user.City = model.city;
                user.State = model.state;
                user.Mobile = model.phone;
                await _userService.AddUser(user);
            }
            aspuser = await _userService.CheckUser(model.email);
            await _requestService.PatientRequest(model);

            return Json(new { success = true, redirectUrl = Url.Action("SubmitRequest", "SubmitRequest") });
        }

        return Json(new { success = false, message = "User is blocked" });
    }

    [HttpPost]
    public async Task<JsonResult> FamRequest(IFormCollection formcollection, Req model)
    {
        model.email = formcollection["patientEmail"];
        model.cfirstName = formcollection["cfname"];
        model.clastName = formcollection["clname"];
        model.cphone = formcollection["ctel"];
        model.cemail = formcollection["cemail"];
        model.propbus = formcollection["cprop"];
        model.symptoms = formcollection["symptoms"];
        model.firstName = formcollection["fname"];
        model.lastName = formcollection["lname"];
        model.password = formcollection["password"];
        var d = DateTime.Parse(formcollection["dob"]);
        model.year = d.Year;
        model.month = d.ToString("MMMM");
        model.date = d.Day;
        model.phone = formcollection["patientTel"];
        model.street = formcollection["street"];
        model.city = formcollection["city"];
        model.state = formcollection["state"];
        model.zipcode = formcollection["zip"];
        model.room = formcollection["roomNum"];
        model.typeid = 2;
        if (!_userService.IsUserBlocked(model.cemail, model.cphone))
        {
            var user = await _userService.CheckUser(model.email);
            if (user == null)
            {
                var link = "https://localhost:44319/Login/PatientCreate";
                var subject = "Register Yourself";
                var body = $"Please register yourself <a href='{link}'>here</a>.";

                _emailService.SendEmail(model.email, subject, body);
            }
            user = await _userService.CheckUser(model.cemail);
            await _requestService.PatientRequest(model);

            return Json(new { success = true, redirectUrl = Url.Action("SubmitRequest", "SubmitRequest") });
        }
        return Json(new { success = false, message = "User is blocked" });
    }

    [HttpPost]
    public async Task<JsonResult> ConRequest([FromBody] Req model)
    {
        if (ModelState.IsValid)
        {
            if (!_userService.IsUserBlocked(model.cemail, model.cphone))
            {
                var user = await _userService.CheckUser(model.email);
                if (user == null)
                {
                    var link = "https://localhost:44319/Login/PatientCreate";
                    var subject = "Register Yourself";
                    var body = $"Please register yourself <a href='{link}'>here</a>.";

                    _emailService.SendEmail(model.email, subject, body);
                }
                user = await _userService.CheckUser(model.cemail);
                await _requestService.ConciergeRequest(model);

                return Json(new { success = true, redirectUrl = Url.Action("SubmitRequest", "SubmitRequest") });
            }
            return Json(new { success = false, message = "User is blocked" });
        }

        return Json(new { success = false, message = "Invalid Input" });
    }

    [HttpPost]
    public async Task<JsonResult> BusRequest([FromBody] Req model)
    {
        if (ModelState.IsValid)
        {
            if (!_userService.IsUserBlocked(model.cemail, model.cphone))
            {
                var user = await _userService.CheckUser(model.email);
                if (user == null)
                {
                    var link = "https://localhost:44319/Login/PatientCreate";
                    var subject = "Register Yourself";
                    var body = $"Please register yourself <a href='{link}'>here</a>.";

                    _emailService.SendEmail(model.email, subject, body);
                }
                user = await _userService.CheckUser(model.cemail);
                await _requestService.BusinessRequest(model);

                return Json(new { success = true, redirectUrl = Url.Action("SubmitRequest", "SubmitRequest") });
            }
            return Json(new { success = false, message = "User is blocked" });
        }

        return Json(new { success = false, message = "Invalid Input" });
    }

    public async Task<IActionResult> OtherRequest()
    {
        var email = HttpContext.Session.GetString("email");
        ViewData["email"] = email;
        User user = new User();
        user = await _userService.GetUser(email);
        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
