using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Globalization;
using Timesheet = HalloDocDAL.Models.Timesheet;

namespace HalloDocBAL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context, IRequestRepository requestRepository, IRequestWiseFilesRepository requestWiseFilesRepository)
        {
            _context = context;
            _requestRepository = requestRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
        }

        public async Task<Requestwisefile> GetDocument(string id)
        {
            return await _requestWiseFilesRepository.GetFile(id);
        }

        public List<Dashboard> PatientDashboard(String email)
        {
            return _requestRepository.GetRequestByEmail(email);
        }

        public async Task<bool> UploadDocument(IFormFileCollection files, int rid)
        {
            foreach (var file in files)
            {
                var filePath = "";
                if (file.Length > 0)
                {
                    var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", rid.ToString());
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }
                    filePath = Path.Combine(uploadsFolderPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                if (file.FileName.Length > 0)
                {
                    var requestWiseFile = new Requestwisefile
                    {
                        Requestid = rid,
                        Filename = file.FileName,
                        Createddate = DateTime.Now,
                        Ispatientrecords = true
                    };
                    Debug.WriteLine(file.FileName);
                    await _requestWiseFilesRepository.AddFiles(requestWiseFile);
                }
            }
            return true;
        }

        public List<Requestwisefile> ViewDocument(int id)
        {
            var obj = _requestWiseFilesRepository.GetFiles(id);
            return obj;
        }

        public List<TimesheetModel> SearchDataByRangeTimeSheet(DateTime startDate, string aspuserid)
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

        public bool CheckFinalize(DateTime startDate, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;
            var data = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (data != null)
            {
                return data.Isfinalize;
            }

            return false;
        }

        public List<InvoicingModel> SearchDataByRangeInvoicing(DateTime startDate, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;

            DateTime currentDate = startDate;
            List<InvoicingModel> model = new List<InvoicingModel>();

            for (var i = 0; i < 15; i++)
            {
                var item = new InvoicingModel();
                item.Date = currentDate.ToString("dd/MM/yyyy");

                var invoice = _context.Invoicings.FirstOrDefault(a => a.Startdate == startDate && a.Physicianid == Physicianid);

                if (invoice != null)
                {
                    item.OnCallHours = GetOnCallHours(invoice.Physicianid, currentDate);
                    var timesheet = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoice.Id && r.Date == currentDate);

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
                }

                model.Add(item);
                currentDate = currentDate.AddDays(1);
            }

            return model;
        }

        public List<InvoicingModel> SearchDataByRangeReimbursement(DateTime startDate, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;

            DateTime currentDate = startDate;
            List<InvoicingModel> model = new List<InvoicingModel>();

            for (var i = 0; i < 15; i++)
            {
                var item = new InvoicingModel();
                item.Date = currentDate.ToString("dd/MM/yyyy");

                var invoice = _context.Invoicings.FirstOrDefault(a => a.Startdate == startDate && a.Physicianid == Physicianid);

                if (invoice != null)
                {
                    var timesheet = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoice.Id && r.Date == currentDate);

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

        public int GetOnCallHours(int physicianId, DateTime date)
        {
            var data = _context.Shiftdetails.Where(r => r.Shift.Physicianid == physicianId && r.Shiftdate == date).ToList();

            if (data != null)
            {
                int hours = 0;
                foreach (var i in data)
                {
                    TimeSpan timeSpan = i.Endtime - i.Starttime;
                    int count = Convert.ToInt32(timeSpan.TotalHours);
                    hours += count;
                }
                return hours;
            }
            return 0;
        }

        public bool SaveTimeSheet(List<InvoicingModel> invoicingModels, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;
            var startDate = DateTime.ParseExact(invoicingModels.FirstOrDefault().Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var isInvoicing = _context.Invoicings.Any(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (!isInvoicing)
            {
                var data = new Invoicing
                {
                    Startdate = startDate,
                    Enddate = startDate.AddDays(14),
                    Physicianid = Physicianid,
                    Isfinalize = false,
                };
                _context.Invoicings.Add(data);
                _context.SaveChanges();
            }

            var invoicing = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            foreach (var i in invoicingModels)
            {
                if (i.TotalHours != 0 || i.HouseCall != 0 || i.Consult != 0)
                {
                    var isExists = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoicing.Id && r.Date == DateTime.ParseExact(i.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture));

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

        public bool SaveReimbursement(InvoicingModel invoicingModels, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;
            var startDate = invoicingModels.StartDate;

            var isInvoicing = _context.Invoicings.Any(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (!isInvoicing)
            {
                var data = new Invoicing
                {
                    Startdate = startDate,
                    Enddate = startDate.AddDays(14),
                    Physicianid = Physicianid,
                    Isfinalize = false,
                };
                _context.Invoicings.Add(data);
                _context.SaveChanges();
            }

            var invoicing = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (invoicingModels.Item != "" || invoicingModels.Amount != 0 || invoicingModels.Bill != null)
            {
                var isExists = _context.Timesheets.FirstOrDefault(r => r.Invoiceid == invoicing.Id && r.Date == DateTime.ParseExact(invoicingModels.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture));

                if (isExists != null)
                {
                    isExists.Invoiceid = invoicing.Id;
                    isExists.Date = DateTime.ParseExact(invoicingModels.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    isExists.Item = invoicingModels.Item;
                    isExists.Amount = invoicingModels.Amount;

                    if (invoicingModels.Bill != null)
                    {
                        isExists.Billname = invoicingModels.Bill.FileName;

                        var fileName = $"{invoicingModels.Date}.pdf";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/invoicing/" + Physicianid + "/" + fileName);

                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/invoicing/" + Physicianid);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            invoicingModels.Bill.CopyTo(stream);
                        }
                    }

                    _context.SaveChanges();
                }
                else
                {
                    var timesheet = new Timesheet
                    {
                        Invoiceid = invoicing.Id,
                        Date = DateTime.ParseExact(invoicingModels.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Item = invoicingModels.Item,
                        Amount = invoicingModels.Amount
                    };

                    if (invoicingModels.Bill != null)
                    {

                        var fileName = $"{invoicingModels.Date}.pdf";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/invoicing/" + Physicianid + "/" + fileName);

                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/invoicing/" + Physicianid);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            invoicingModels.Bill.CopyTo(stream);
                        }
                    }

                    _context.Timesheets.Add(timesheet);
                    _context.SaveChanges();
                }
            }

            return false;
        }

        public void FinalizeTimesheet(DateTime startDate, string aspuserid)
        {
            int Physicianid = _context.Physicians.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Physicianid;

            var invoicing = _context.Invoicings.FirstOrDefault(r => r.Startdate == startDate && r.Physicianid == Physicianid);

            if (invoicing != null)
            {
                invoicing.Isfinalize = true;
                invoicing.Modifiedby = Physicianid;
                invoicing.Modifieddate = DateTime.Now;

                _context.Invoicings.Update(invoicing);
                _context.SaveChanges();
            }
        }

        public ChatModel GetChatAdmin(int Adminid, string aspuserid)
        {
            var Userid = _context.Users.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Userid;

            var model = new ChatModel
            {
                Adminid = Adminid,
                Patientid = Userid,
                chatWith = "admin",
                isUser = true,
                SenderId = Userid,
                SenderType = "Patient",
                ReceiverId = Adminid,
                ReceiverType = "Admin"
            };
            return model;
        }

        public ChatModel GetChatPhysician(int Physicianid, string aspuserid)
        {
            var Userid = _context.Users.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Userid;
            var physician = _context.Physicians.FirstOrDefault(r => r.Physicianid == Physicianid);

            if (physician == null)
            {
                var model = new ChatModel
                {
                    Patientid = Userid,
                    Physicianid = Physicianid,
                    isUser = false
                };
                return model;
            }
            else
            {
                var model = new ChatModel
                {
                    Patientid = Userid,
                    Physicianid = Physicianid,
                    chatWith = "physician",
                    isUser = true,
                    PhysicianName = physician.Firstname + " " + physician.Lastname,
                    SenderId = Userid,
                    SenderType = "Patient",
                    ReceiverId = Physicianid,
                    ReceiverType = "Physician"
                };
                return model;
            }
        }

        public ChatModel GetGroupChat(int adminid, int Physicianid, string aspuserid)
        {
            var admin = _context.Admins.FirstOrDefault(r => r.Adminid == adminid);
            var physician = _context.Physicians.FirstOrDefault(r => r.Physicianid == Physicianid);
            var userid = _context.Users.FirstOrDefault(r => r.Aspnetuserid == aspuserid).Userid;

            var model = new ChatModel
            {
                Adminid = adminid,
                Physicianid = Physicianid,
                Patientid = userid,
                isUser = true,
                AdminName = admin.Firstname + " " + admin.Lastname,
                PhysicianName = physician.Firstname + " " + physician.Lastname,
                SenderId = userid,
                SenderType = "Patient"
            };
            return model;
        }


    }
}
