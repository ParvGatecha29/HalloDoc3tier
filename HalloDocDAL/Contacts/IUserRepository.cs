using HalloDocDAL.Model;
using HalloDocDAL.Models;
namespace HalloDocDAL.Contacts
{
    public interface IUserRepository
    {
        Task<bool> AddUser(Aspnetuser user);
        Task<Aspnetuser> FindByEmail(string email);
        bool IsUserBlocked(string email, string phone);
        Task<User> GetByEmail(string email);
        Task<bool> EditAspUser(Aspnetuser user);
        string GetToken(string email);
        bool storeToken(string email, string token);
        Admin GetAdminById(string id);
        List<int> GetAdminRegions(string id);
        bool UpdateAdminProfile(AdminProfile profile);
        bool AddProvider(Provider model, string adminId);
        List<Role> GetRoles();
        List<Menu> GetMenus(int AccountType);
        List<Rolemenu> GetRoleMenus(int roleid);
        bool AddRole(Access role);
        bool AddShift(Shift shift);
        bool AddShiftDetails(Shiftdetail shift);
        bool AddShiftDetailRegions(Shiftdetailregion shift);
        Shiftdetail FindShiftDetails(int detailid);
        bool UpdateShiftDetails(Shiftdetail details);
        List<ScheduleModel> GetEvents(int region, bool currentMonth);
        List<MappedEvents> GetMappedEvents(int region);
        List<ScheduleModel> GetReviewShifts(int region);
        List<User> GetUsers();
        List<Healthprofessionaltype> GetHealthProfessionalTypes();
        List<Partner> GetVendors(int Profession, string VendorSearch);
        string GetStateRegionId(int? regionid);
        bool AddBusiness(BusinessModel model);
        public BusinessModel EditBusiness(int vendorid);
        public bool UpdateBusiness(BusinessModel model);
        public bool DeleteBusiness(int vendorid);
        public bool DeleteProvider(int id);
    }
}