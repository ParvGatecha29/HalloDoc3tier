using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;

namespace HalloDocBAL.Interfaces
{
    public interface IDashboardService
    {
        List<Dashboard> PatientDashboard(String email);
        List<Requestwisefile> ViewDocument(int id);
        Task<bool> UploadDocument(IFormFileCollection files, int requestid);
        Task<Requestwisefile> GetDocument(string id);
        public List<TimesheetModel> SearchDataByRangeTimeSheet(DateTime startDate, string aspuserid);
        public bool CheckFinalize(DateTime startDate, string aspuserid);
        public List<InvoicingModel> SearchDataByRangeInvoicing(DateTime startDate, string aspuserid);
        public List<InvoicingModel> SearchDataByRangeReimbursement(DateTime startDate, string aspuserid);
        public int GetOnCallHours(int physicianId, DateTime date);
        public bool SaveTimeSheet(List<InvoicingModel> invoicingModels, string aspuserid);
        public bool SaveReimbursement(InvoicingModel invoicingModels, string aspuserid);
        public void FinalizeTimesheet(DateTime startDate, string aspuserid);
        public ChatModel GetChatAdmin(int Adminid, string aspuserid);
        public ChatModel GetChatPhysician(int Physicianid, string aspuserid);
        public ChatModel GetGroupChat(int adminid, int Physicianid, string aspuserid);
    }
}
