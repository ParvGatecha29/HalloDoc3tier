
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;

namespace HalloDocDAL.Contacts
{
    public interface IRequestRepository
    {
        Task<bool> CreateRequest(Request model);
        List<Dashboard> GetRequestByEmail(string email);
        List<AdminDashboardData> GetAllRequests();
        Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber, int region, string search, bool all = false, int physicianid = 0);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool AddNotes(AdminDashboardData data);
        bool UpdateNotes(AdminDashboardData data);
        bool transferRequest(AdminDashboardData data, int newstate);
        bool AddBlockRequest(AdminDashboardData data);
        List<Region> GetRegions();
        bool EditCase(AdminDashboard model);
        bool EditEncounterForm(EncounterForm model);
        EncounterForm GetEncounterForm(int requestId);
        bool EmailLog(string to, string subject, string body);

        Task<PagedList<EmailLog>> GetEmailLogs(int roleid, string receiverName, string Email, DateTime createdDate, DateTime sentDate, int pageNumber);
    }
}
