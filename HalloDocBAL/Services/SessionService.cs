﻿using HalloDocDAL.Models;
using HalloDocDAL.Model;
using Microsoft.AspNetCore.Http;

namespace HalloDocBAL.Services
{
    public class SessionService
    {
        public static UserInfo GetLoggedInUser(ISession session)
        {
            UserInfo userInfo = null;

            if (!string.IsNullOrEmpty(session.GetString("userId")))
            {
                userInfo = new UserInfo();
                userInfo.Id = session.GetString("userId");
                userInfo.Email = session.GetString("Email");
                userInfo.Role = session.GetString("Role");
            }

            return userInfo;
        }

        public static void SetLoggedInUser(ISession session, Aspnetuser user)
        {
            if (user != null)
            {
                session.SetString("userId", user.Id);
                session.SetString("Email", user.Email);
                session.SetString("Role", user.Aspnetuserroles.FirstOrDefault().RoleId);
            }
        }
    }
}
