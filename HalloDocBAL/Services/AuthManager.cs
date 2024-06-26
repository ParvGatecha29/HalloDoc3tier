﻿
using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HalloDocDAL.Repositories
{
    public class AuthManager : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        private readonly string _access;
        public AuthManager(string role = "", string access = "")
        {
            _role = role;
            _access = access;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "PatientLogin" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "AjaxLogout" }));
                    return;
                }
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "Index" }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PatientLogin" }));
                return;
            }

            if (!string.IsNullOrEmpty(_role))
            {
                if (!(roleClaim.Value == _role))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                    return;
                }
            }

            var user = SessionService.GetLoggedInUser(context.HttpContext.Session);
            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            string menus = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Menus").ToString();
            if (_access != "")
            {
                List<string> access = menus.Split(',').ToList();
                if (!access.Contains(_access))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                    return;
                }
            }
        }
    }
}
