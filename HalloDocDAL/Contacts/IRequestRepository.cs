
using HalloDocDAL.Model;
using HalloDocDAL.Models;
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
        List<AdminDashboardData> GetRequestsByStatus(int[] status, int reqtype);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool AddNotes(AdminDashboardData data);
        bool UpdateNotes(AdminDashboardData data);
        bool transferRequest(AdminDashboardData data,int newstate);
        bool AddBlockRequest(AdminDashboardData data);
        List<Region> GetRegions();

        bool EditEncounterForm(EncounterForm model);
        EncounterForm GetEncounterForm(int requestId);
    }
}
