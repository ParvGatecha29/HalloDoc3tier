using HalloDocDAL.Model;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IUserService
    {
        Task<Aspnetuser> Login(Login model);
        Task<bool> SignUp(Register model);
        Task<Aspnetuser> CheckUser(string email);
        Task<User> GetUser(string email);
        Task<bool> AddUser(User model);
        Task<bool> EditUser(User model);
        Task<bool> EditAspNetUser(Aspnetuser user);
        bool IsUserBlocked(string email, string phone);
    }
}
