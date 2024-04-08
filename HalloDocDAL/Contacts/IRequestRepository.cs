
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Contacts
{
    public interface IRequestRepository
    {
        Task<bool> CreateRequest(Request model);
        List<Dashboard> GetRequestByEmail(string email);
        List<AdminDashboardData> GetAllRequests();
        Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber, int region, string search,bool all=false);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool AddNotes(AdminDashboardData data);
        bool UpdateNotes(AdminDashboardData data);
        bool transferRequest(AdminDashboardData data,int newstate);
        bool AddBlockRequest(AdminDashboardData data);
        List<Region> GetRegions();
        bool EditCase(AdminDashboard model);
        bool EditEncounterForm(EncounterForm model);
        EncounterForm GetEncounterForm(int requestId);
        bool EmailLog(string to, string subject, string body);
    }
}
