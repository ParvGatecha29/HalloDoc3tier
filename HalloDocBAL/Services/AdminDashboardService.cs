
using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using System.Globalization;

namespace HalloDocBAL.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public AdminDashboardService(ApplicationDbContext context, IRequestRepository requestRepository, IPhysicianRepository physicianRepository, IRequestWiseFilesRepository requestWiseFilesRepository, IUserRepository userRepository)
        {
            _context = context;
            _requestRepository = requestRepository;
            _physicianRepository = physicianRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
            _userRepository = userRepository;
        }
        
        public List<AdminDashboardData> GetRequests()
        {
            var data = _requestRepository.GetAllRequests();
            return data;
        }

        public async Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber, int region, string search, bool all, int physicianid)
        {
            var data = await _requestRepository.GetRequestsByStatus(status, reqtype, pageNumber, region, search, all,physicianid);
            return data;
        }
      
        public AdminDashboardData GetRequestById(int id)
        {
            var data = _requestRepository.GetRequestById(id);
            return data;
        }

        public AdminDashboardData GetNotes(int id)
        {
            var data = _requestRepository.GetNotes(id);
            if (data == null)
            {
                data = new AdminDashboardData
                {
                    requestId = id,
                    adminNotes = "",
                    physicianNotes = "",
                    transferNotes = null,
                    confirmationNo = ""
                };
                return data;
            }
            return data;
        }

        public bool UpdateNotes(int id, string notes, bool physician)
        {
            var data = _requestRepository.GetNotes(id);
            if (data == null)
            {
                if (physician)
                {
                    data = new AdminDashboardData
                    {
                        requestId = id,
                        adminNotes = "",
                        physicianNotes = notes,
                        transferNotes = null
                    };
                }
                else
                {
                    data = new AdminDashboardData
                    {
                        requestId = id,
                        adminNotes = notes,
                        physicianNotes = "",
                        transferNotes = null
                    };
                }
                _requestRepository.AddNotes(data);
                return true;
            }
            if (physician)
            {
                data.physicianNotes = notes;
            }
            else
            {
                data.adminNotes = notes;
            }
            _requestRepository.UpdateNotes(data);
            return true;
        }

        public bool CancelRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 3);
            return true;
        }

        public List<Physician> GetPhysiciansByRegion(int regionid)
        {
            return _physicianRepository.GetPhysicians(regionid);
        }

        public Physician GetPhysiciansById(int id)
        {
            return _physicianRepository.GetPhysicianById(id);
        }

        public Physician GetPhysiciansByAspId(string id)
        {
            return _physicianRepository.GetPhysicianByAspId(id);
        }

        public Physician GetPhysiciansByEmail(string email)
        {
            return _physicianRepository.GetPhysicianByEmail(email);
        }

        public List<Physicianregion> GetPhysicianRegions(int id)
        {
            return _physicianRepository.GetPhysicianRegions(id);
        }

        public bool AssignRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 2);
            return true;
        }

        public bool AgreeRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 4);
            return true;
        }

        public bool BlockRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 11);
            _requestRepository.AddBlockRequest(data);
            return true;
        }

        public bool ClearRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 10);
            return true;
        }

        public bool CloseRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 9);
            return true;
        }

        public List<Region> GetAllRegions()
        {
            return _requestRepository.GetRegions();
        }

        public bool DeleteFile(int fileid)
        {
            _requestWiseFilesRepository.DeleteFile(fileid);
            return true;
        }

        public bool UpdateEncounterForm(ViewEncounterForm data)
        {
            var model = new EncounterForm
            {
                RequestId = data.RequestId,
                HistoryOfPresentIllnessOrInjury = data.HistoryOfPresentIllness,
                MedicalHistory = data.MedicalHistory,
                Medications = data.Medications,
                Allergies = data.Allergies,
                Temp = data.Temperature,
                Hr = data.HR,
                Rr = data.RR,
                BloodPressureSystolic = data.BPSystolic,
                BloodPressureDiastolic = data.BPDiastolic,
                O2 = data.O2,
                Pain = data.Pain,
                Heent = data.Heent,
                Cv = data.CV,
                Chest = data.Chest,
                Abd = data.ABD,
                Extremeties = data.Extr,
                Skin = data.Skin,
                Neuro = data.Neuro,
                Other = data.Other,
                Diagnosis = data.Diagnosis,
                TreatmentPlan = data.TreatmentPlan,
                MedicationsDispensed = data.MedicationDispensed,
                Procedures = data.Procedures,
                FollowUp = data.FollowUp,
                IsFinalize = data.IsFinalizeDB
            };
            _requestRepository.EditEncounterForm(model);
            return true;
        }

        public EncounterForm GetEncounterForm(int requestId)
        {
            var data = _requestRepository.GetEncounterForm(requestId);
            return data;
        }

        public Admin GetAdminById(string id)
        {
            return _userRepository.GetAdminById(id);
        }

        public bool UpdateProfile(AdminProfile model)
        {
            _userRepository.UpdateAdminProfile(model);
            return true;
        }

        public List<int> GetAdminRegions(string id)
        {
            return _userRepository.GetAdminRegions(id);
        }

        public bool EditCase(AdminDashboard model)
        {
            _requestRepository.EditCase(model);
            return true;
        }

        public bool AddProvider(Provider model, string adminId)
        {
            bool add = _userRepository.AddProvider(model, adminId);
            return add;
        }

        public bool EditPhysician(Provider model)
        {
            bool add = _physicianRepository.EditPhysician(model);
            return add;
        }

        public List<Physicianlocation> GetProviders()
        {
            var add = _physicianRepository.GetProviders();
            return add;
        }

        public List<Role> GetRoles()
        {
            var roles = _userRepository.GetRoles();
            return roles;
        }

        public List<Menu> GetMenus(int AccountType)
        {
            var menus = _userRepository.GetMenus(AccountType);
            return menus;
        }

        public List<Rolemenu> GetRoleMenus(int roleid)
        {
            var menus = _userRepository.GetRoleMenus(roleid);
            return menus;
        }

        public bool CreateAccess(Access model)
        {
            bool role = _userRepository.AddRole(model);
            return role;
        }

        public List<User> GetAllUsers()
        {
            var role = _userRepository.GetUsers();
            return role;
        }

        public List<TimesheetModel> SearchDataByRangeTimeSheet(DateTime startDate, int Physicianid)
        {
            DateTime currentDate = startDate;
            List<TimesheetModel> model = new List<TimesheetModel>();

            for (var i = 0; i < 15; i++)
            {
                var item = new TimesheetModel();
                item.Date = currentDate.ToString("dd/MM/yyyy");

                model.Add(item);
                currentDate = currentDate.AddDays(1);
            }

            return model;
        }

        public List<InvoicingModel> SearchDataByRangeReimbursement(DateTime startDate, int Physicianid)
        {
            DateTime currentDate = startDate;
            List<InvoicingModel> model = new List<InvoicingModel>();

            for (var i = 0; i < 15; i++)
            {
                var item = new InvoicingModel();
                item.Date = currentDate.ToString("dd/MM/yyyy");

                var invoice = _context.Invoicings.FirstOrDefault(a => a.Startdate == startDate && a.Physicianid == Physicianid);

                if (invoice != null)
                {
                    var timesheet = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoice.Id && r.Date == currentDate && r.Isdeleted != true);

                    if (timesheet != null)
                    {
                        item.BillName = timesheet.Billname ?? "";
                        item.Item = timesheet.Item ?? "";
                        item.Amount = timesheet.Amount ?? 0;
                    }
                }

                if (item.BillName != "" || item.Item != "" || item.Amount != 0)
                {
                    model.Add(item);
                }
                currentDate = currentDate.AddDays(1);
            }

            return model;
        }

        public AdminInvoicing CheckApproved(DateTime startDate, int Physicianid)
        {
            var data = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (data != null)
            {
                var model = new AdminInvoicing
                {
                    Id = data.Id,
                    PhysicianId = Physicianid,
                    StartDate = data.Startdate.ToString("dd-MM-yyyy"),
                    EndDate = data.Enddate.ToString("dd-MM-yyyy"),
                    status = "pending",
                    isApproved = data.Isapproved,
                    isFinalize = data.Isfinalize,
                    physicianName = _context.Physicians.FirstOrDefault(r => r.Physicianid == Physicianid).Firstname + " " + _context.Physicians.FirstOrDefault(r => r.Physicianid == Physicianid).Lastname
                };
                return model;
            }

            return null;
        }

        public List<InvoicingModel> SearchDataById(int Id)
        {
            var invoice = _context.Invoicings.FirstOrDefault(a => a.Id == Id);

            if (invoice != null)
            {
                List<InvoicingModel> model = new List<InvoicingModel>();
                DateTime currentDate = invoice.Startdate;

                for (var i = 0; i < 15; i++)
                {
                    var item = new InvoicingModel();
                    item.Date = currentDate.ToString("dd/MM/yyyy");

                    var timesheet = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoice.Id && r.Date == currentDate && r.Isdeleted != true);

                    if (timesheet != null)
                    {
                        item.isWeekEnd = timesheet.Weekend ?? false;
                        item.TotalHours = timesheet.Totalhours ?? 0;
                        item.HouseCall = timesheet.Housecall ?? 0;
                        item.Consult = timesheet.Consult ?? 0;
                        item.Item = timesheet.Item ?? "";
                        item.Amount = timesheet.Amount ?? 0;
                        item.BillName = timesheet.Billname ?? "";
                        item.IsDeleted = timesheet.Isdeleted ?? false;
                    }

                    model.Add(item);
                    currentDate = currentDate.AddDays(1);
                }
                return model;
            }

            return null;
        }

        public bool SaveTimeSheet(List<InvoicingModel> invoicingModels, int Physicianid, string aspuserid)
        {
            int Adminid = _context.Admins.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Adminid;
            var startDate = DateTime.ParseExact(invoicingModels.FirstOrDefault().Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var invoicing = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);
            var count = 0;

            foreach (var i in invoicingModels)
            {
                if (count == 15)
                {
                    break;
                }
                count++;
                if (i.TotalHours != 0 || i.HouseCall != 0 || i.Consult != 0)
                {
                    var isExists = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoicing.Id && r.Date == DateTime.ParseExact(i.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture) && r.Isdeleted != true);

                    if (isExists != null)
                    {
                        isExists.Invoiceid = invoicing.Id;
                        isExists.Date = DateTime.ParseExact(i.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        isExists.Totalhours = i.TotalHours;
                        isExists.Weekend = i.isWeekEnd;
                        isExists.Housecall = i.HouseCall;
                        isExists.Consult = i.Consult;

                        _context.SaveChanges();
                    }
                    else
                    {
                        var timesheet = new HalloDocDAL.Models.Timesheet
                        {
                            Invoiceid = invoicing.Id,
                            Date = DateTime.ParseExact(i.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            Totalhours = i.TotalHours,
                            Weekend = i.isWeekEnd,
                            Housecall = i.HouseCall,
                            Consult = i.Consult
                        };
                        _context.Timesheets.Add(timesheet);
                        _context.SaveChanges();
                    }
                }
            }
            return false;
        }

        public void ApproveTimesheet(DateTime startDate, int Physicianid, string aspuserid, int bonus, string adminDescription)
        {
            int Adminid = _context.Admins.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Adminid;
            var invoicing = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (invoicing != null)
            {
                invoicing.Isapproved = true;
                invoicing.Bonus = bonus;
                invoicing.Admindescription = adminDescription;
                invoicing.Modifiedby = Adminid;
                invoicing.Modifieddate = DateTime.Now;

                _context.SaveChanges();
            }
        }

        public Physicianpayrate GetPayrateData(int Physicianid)
        {
            var data = _context.Physicianpayrates.FirstOrDefault(r => r.PhysicianId == Physicianid);

            if (data != null)
            {
                var model = new Physicianpayrate
                {
                    PhysicianId = (int)data.PhysicianId,
                    NightShiftWeekend = data.NightShiftWeekend ?? 0,
                    Shift = data.Shift ?? 0,
                    HouseCallNightWeekend = data.HouseCallNightWeekend ?? 0,
                    PhoneConsult = data.PhoneConsult ?? 0,
                    PhoneConsultNightWeekend = data.PhoneConsultNightWeekend ?? 0,
                    BatchTesting = data.BatchTesting ?? 0,
                    HouseCall = data.HouseCall ?? 0
                };
                return model;
            }
            else
            {
                var model = new Physicianpayrate
                {
                    PhysicianId = Physicianid,
                    NightShiftWeekend = 0,
                    Shift = 0,
                    HouseCallNightWeekend = 0,
                    PhoneConsult = 0,
                    PhoneConsultNightWeekend = 0,
                    BatchTesting = 0,
                    HouseCall = 0
                };
                return model;
            }
        }
    }
}
