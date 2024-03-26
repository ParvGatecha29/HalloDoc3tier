using HalloDocDAL.Model;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HalloDocDAL.Contacts
{
    public interface IUserRepository
    {
        Task<bool> AddUser(Aspnetuser user);
        Task<Aspnetuser> FindByEmail(string email);
        bool IsUserBlocked (string email, string phone);
        Task<User> GetByEmail(string email);
        Task<bool> EditAspUser(Aspnetuser user);
        string GetToken(string email);

        bool storeToken(string email, string token);
        Admin GetAdminById(string id);
        List<int> GetAdminRegions(string id);

        bool UpdateAdminProfile(AdminProfile profile);
        bool AddProvider(Provider model, string adminId);
    }
}