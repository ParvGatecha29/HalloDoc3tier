
using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace HalloDocDAL.Repositories
{
    public class AuthManager : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        public AuthManager(string role="") {
            _role = role;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            if(jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if(token == null || !jwtService.ValidateToken(token,out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if(roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            if (!string.IsNullOrEmpty(_role))
            {
                if (!(roleClaim.Value == _role))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }
            }
            //var user = SessionService.GetLoggedInUser(context.HttpContext.Session);
            //if (user == null)
            //{
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary ( new { controller = "Home", action = "Index" } ));
            //    return;
            //}
            //if(!string.IsNullOrEmpty(_role))
            //{
            //    if(!(user.Role == _role)){
            //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //    }
            //}
        }
    }
}
