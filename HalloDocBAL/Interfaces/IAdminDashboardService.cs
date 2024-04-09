
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IAdminDashboardService
    {
        List<AdminDashboardData> GetRequests();
        Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype = 0, int pageNumber = 1, int region = 0, string search = "", bool all = false);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool UpdateNotes(int id, string notes);
        bool CancelRequest(AdminDashboardData data);
        bool AssignRequest(AdminDashboardData data);
        bool AgreeRequest(AdminDashboardData data);
        bool BlockRequest(AdminDashboardData data);
        bool ClearRequest(AdminDashboardData data);
        bool CloseRequest(AdminDashboardData data);
        List<Physician> GetPhysiciansByRegion(int regionid);
        Physician GetPhysiciansById(int id);
        Physician GetPhysiciansByEmail(string email);
        List<Physicianregion> GetPhysicianRegions(int id);
        List<Region> GetAllRegions();
        bool DeleteFile(int fileid);
        bool UpdateEncounterForm(ViewEncounterForm model);
        EncounterForm GetEncounterForm(int requestId);
        bool EditCase(AdminDashboard model);
        Admin GetAdminById(string id);
        List<int> GetAdminRegions(string id);
        bool UpdateProfile(AdminProfile model);
        bool AddProvider(Provider model, string adminId);
        bool EditPhysician(Provider model);
        List<Physicianlocation> GetProviders();
        List<Role> GetRoles();
        List<Menu> GetMenus(int AccountType = 0);
        List<Rolemenu> GetRoleMenus(int roleid = 0);
        bool CreateAccess(Access model);
        List<User> GetAllUsers();
    }
}
