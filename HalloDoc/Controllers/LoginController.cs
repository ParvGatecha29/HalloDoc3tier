using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDocDAL.Data;
using Newtonsoft.Json;
using HalloDocDAL.Models;
using System.ComponentModel.DataAnnotations;
using HalloDocDAL.Model;
using HalloDocBAL.Interfaces;
using Microsoft.AspNetCore.Http;
using HalloDocBAL.Services;
using System.Net.Mail;
using HalloDocDAL.Repositories;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;

namespace HalloDoc.Controllers;

public class LoginController : Controller
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;

    public LoginController(IUserService userService, ITokenService tokenService, IEmailService emailService, IJwtService jwtService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _emailService = emailService;
        _jwtService = jwtService;
    }

    public async Task<IActionResult> PatientLogin()
    {
        if(HttpContext.Session.GetString("email") != null)
        {
            var user = await _userService.CheckUser(HttpContext.Session.GetString("email"));
            var token = Request.Cookies["jwt"];
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
                var role = "Login";
                switch (roleClaim.Value)
                {
                    case "1":
                        role = "AdminDashboard";
                        break;
                    case "2":
                        role = "PatientDashboard";
                        break;
                    case "3":
                        role = "ProviderDashboard";
                        break;
                }
                SessionService.SetLoggedInUser(HttpContext.Session, user);
                return RedirectToAction(role, role);
            }
        }
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult PatientCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> PatientLogin([FromBody] Login model)
    {
        var result = await _userService.Login(model);
        if (result != null) {
            var jwtToken = _jwtService.GenerateToken(result);
            Response.Cookies.Append("jwt", jwtToken);
            HttpContext.Session.SetString("email", model.Email);
            var user = await _userService.CheckUser(model.Email);
            SessionService.SetLoggedInUser(HttpContext.Session, user);
            string role = "";
            switch (result.Aspnetuserroles.FirstOrDefault().RoleId)
            {
                case "1":
                    role = "AdminDashboard";
                    break;
                case "2":
                    role = "PatientDashboard";
                    break;
                case "3":
                    role = "ProviderDashboard";
                    break;
            }
            return Json(new { success = true, redirectUrl = Url.Action(role, role) });
        }
        else
        {
            return Json(new { success = false, message = "Invalid Username or Password" });
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        Response.Cookies.Delete("jwt");
        return RedirectToAction("PatientLogin","Login");
    }

    public async Task<JsonResult> PatientSignUp([FromBody] Register model)
    {
        if(ModelState.IsValid)
        {
            if(model.password != model.confirmpassword)
            {
                return Json(new { success = false, message = "Passwords donot match" });
            }

            var result = await _userService.SignUp(model);
            if (!result) { 
                return Json(new { success = false, message = "User already registered" });
            }
            HttpContext.Session.SetString("email", model.Email);
            return Json(new { success = true, redirectUrl = Url.Action("PatientDashboard", "PatientDashboard") });
        }

        return Json(new { success = false, message = "Invalid Input" });
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> SendResetEmail(string toEmail)
    {
        var token = _tokenService.GenerateToken(toEmail);
        _tokenService.StoreToken(toEmail, token);
        var link = "https://localhost:44319/Login/ResetPassword?token="+token;
        var subject = "Reset Your Password";
        var body = $"Please reset your password by clicking <a href='{link}'>here</a>.";
        
        _emailService.SendEmail(toEmail, subject, body);
        return Json(new { success = true, redirectUrl = @Url.Action("PatientLogin", "Login") });
    }

    [HttpPost]
    public async Task<JsonResult> ResetPass([FromBody] Register model)
    {
        var (email, isValid) = _tokenService.ValidateToken(model.token);
        if (isValid && model.password == model.confirmpassword)
        {
            var user = await _userService.CheckUser(email);
            user.Passwordhash = model.confirmpassword;
            await _userService.EditAspNetUser(user);
            return Json(new { success = true, redirectUrl = @Url.Action("PatientLogin", "Login") });
        }

        return Json(new { success = false, message ="Reset Failed" });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
