using HalloDocDAL.Models;
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
                userInfo.aspId = session.GetString("aspuserId");
                userInfo.Id = session.GetString("userId");
                userInfo.Email = session.GetString("Email");
                userInfo.Role = session.GetString("Role");
                userInfo.Name = session.GetString("Name");
            }

            return userInfo;
        }

        public static void SetLoggedInUser(ISession session, User user)
        {
            if (user != null)
            {
                session.SetString("aspuserId", user.Aspnetuserid);
                session.SetString("userId", user.Userid.ToString());
                session.SetString("Email", user.Email);
                session.SetString("Role", user.Aspnetuser.Aspnetuserroles.FirstOrDefault().RoleId);
                session.SetString("Name", user.Aspnetuser.Username);
            }
        }
    }
}
