using HalloDocDAL.Model;

namespace HalloDocBAL.Interfaces
{
    public interface IRequestService
    {
        Task<bool> PatientRequest(Req model);
        Task<bool> ConciergeRequest(Req model);
        Task<bool> BusinessRequest(Req model);
        bool AddNotes(AdminDashboardData data);
        bool AcceptCase(int requestid);
        bool TransferCase(int requestid, string description, int status = 0);
        bool UnblockRequest(int requestid);
    }
}
