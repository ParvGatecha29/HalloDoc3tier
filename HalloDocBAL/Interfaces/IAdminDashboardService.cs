
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;

namespace HalloDocBAL.Interfaces
{
    public interface IAdminDashboardService
    {
        List<AdminDashboardData> GetRequests();
        Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype = 0, int pageNumber = 1, int region = 0, string search = "", bool all = false, int physicianid = 0);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool UpdateNotes(int id, string notes, bool physician = false);
        bool CancelRequest(AdminDashboardData data);
        bool AssignRequest(AdminDashboardData data);
        bool AgreeRequest(AdminDashboardData data);
        bool BlockRequest(AdminDashboardData data);
        bool ClearRequest(AdminDashboardData data);
        bool CloseRequest(AdminDashboardData data);
        List<Physician> GetPhysiciansByRegion(int regionid);
        Physician GetPhysiciansById(int id);
        Physician GetPhysiciansByAspId(string id);
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
        public List<TimesheetModel> SearchDataByRangeTimeSheet(DateTime startDate, int Physicianid);
        public List<InvoicingModel> SearchDataByRangeReimbursement(DateTime startDate, int Physicianid);
        public AdminInvoicing CheckApproved(DateTime startDate, int Physicianid);
        public List<InvoicingModel> SearchDataById(int Id);
        public bool SaveTimeSheet(List<InvoicingModel> invoicingModels, int Physicianid, string aspuserid);
        public void ApproveTimesheet(DateTime startDate, int Physicianid, string aspuserid, int bonus, string adminDescription);
        public Physicianpayrate GetPayrateData(int Physicianid);
        public ChatModel getChatPatient(int Patientid, string aspuserid);
        public ChatModel getChatPhysician(int Physicianid, string aspuserid);
        public ChatModel getChatAdmin(int AdminId, string aspuserid);
        public ChatModel GetGroupChat(int Patientid, int Physicianid, string aspuserid);


    }
}
