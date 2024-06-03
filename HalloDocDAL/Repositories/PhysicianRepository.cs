using GoogleMaps.LocationServices;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using System.Data.Entity;
using System.Text;

namespace HalloDocDAL.Repositories
{
    public class PhysicianRepository : IPhysicianRepository
    {
        private readonly ApplicationDbContext _context;
        public PhysicianRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Physician> GetPhysicians(int region)
        {
            if (region == 0)
                return _context.Physicians.Include(_ => _.Physicianlocations).Where(x => x.Isdeleted != true).ToList();
            return _context.Physicians.Where(p => p.Regionid == region & p.Isdeleted != true).ToList();
        }

        public Physician GetPhysicianById(int id)
        {
            var data = _context.Physicians.Include(x => x.Physicianregions).AsQueryable();

            return data.FirstOrDefault(x => x.Physicianid == id);
        }

        public Physician GetPhysicianByAspId(string id)
        {
            var data = _context.Physicians.Include(x => x.Physicianregions).AsQueryable();

            return data.FirstOrDefault(x => x.Aspnetuserid == id);
        }

        public Physician GetPhysicianByEmail(string email)
        {
            return _context.Physicians.Include(x => x.Physicianregions).FirstOrDefault(p => p.Email == email);
        }

        public List<Physicianregion> GetPhysicianRegions(int id)
        {
            return _context.Physicianregions.Where(x => x.Physicianid == id).ToList();
        }

        public bool EditPhysician(Provider model)
        {
            Physician p = _context.Physicians.FirstOrDefault(x => x.Physicianid == model.physician.Physicianid);
            if (model.formtype == 1)
            {
                var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == p.Aspnetuserid);
                model.aspphy.Passwordhash = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.aspphy.Passwordhash));
                aspuser.Passwordhash = model.aspphy.Passwordhash;
                _context.Aspnetusers.Update(aspuser);
                _context.SaveChanges();
            }
            if (model.formtype == 2)
            {
                p.Firstname = model.physician.Firstname;
                p.Lastname = model.physician.Lastname;
                p.Email = model.physician.Email;
                p.Mobile = model.physician.Lastname;
                p.Medicallicense = model.physician.Medicallicense;
                p.Npinumber = model.physician.Npinumber;
                foreach (var region in model.selectedRegions)
                {
                    if (_context.Physicianregions.FirstOrDefault(x => x.Physicianid == model.physician.Physicianid && x.Regionid == region) == null)
                    {
                        Physicianregion physicianregion = new Physicianregion
                        {
                            Physicianid = p.Physicianid,
                            Regionid = region
                        };
                        _context.Physicianregions.Add(physicianregion);
                    }

                }
                List<Physicianregion> remove = _context.Physicianregions.Where(x => !model.selectedRegions.Contains(x.Regionid)).ToList();
                _context.Physicianregions.RemoveRange(remove);
            }
            else if (model.formtype == 3)
            {
                p.Address1 = model.physician.Address1;
                p.Address2 = model.physician.Address2;
                p.City = model.physician.City;
                p.Zip = model.physician.Zip;
                p.Altphone = model.physician.Altphone;
                var address = model.physician.Address1 + " " + model.physician.Address2 + " " + model.physician.City + " " + _context.Regions.FirstOrDefault(r => r.Regionid == model.physician.Regionid).Name;
                var locationService = new GoogleLocationService("AIzaSyARrk6kY-nnnSpReeWotnQxCAo_MoI4qbU");
                var point = locationService.GetLatLongFromAddress(address);

                var latitude = point.Latitude;
                var longitude = point.Longitude;

                var physicianLocation = _context.Physicianlocations.FirstOrDefault(x => x.Physicianid == model.physician.Physicianid);
                if (physicianLocation == null)
                {
                    physicianLocation = new Physicianlocation();
                    physicianLocation.Physicianid = model.physician.Physicianid;
                    physicianLocation.Createddate = DateTime.Now;
                }
                physicianLocation.Latitude = (decimal?)latitude;
                physicianLocation.Longitude = (decimal?)longitude;
                physicianLocation.Address = address;


                _context.Physicianlocations.Update(physicianLocation);
                _context.SaveChanges();
            }
            else if (model.formtype == 4)
            {
                p.Businessname = model.physician.Businessname;
                p.Businesswebsite = model.physician.Businesswebsite;
                p.Adminnotes = model.physician.Adminnotes;
            }

            else if (model.formtype == 5)
            {
                var file = model.sign;
                var filePath = "";
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", p.Physicianid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        filePath = Path.Combine(uploadsFolderPath, "Sign.png");
                        p.Photo = filePath;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyToAsync(stream);
                        }
                    }
                }

                var ICA = model.ICA;
                var ICAPath = "";
                if (ICA != null)
                {
                    if (ICA.Length > 0)
                    {
                        p.Isagreementdoc = true;
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", p.Physicianid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        ICAPath = Path.Combine(uploadsFolderPath, "ICA.pdf");
                        using (var stream = new FileStream(ICAPath, FileMode.Create))
                        {
                            ICA.CopyToAsync(stream);
                        }
                    }
                }

                var BackgroundCheck = model.BackgroundCheck;
                var BackgroundCheckPath = "";
                if (BackgroundCheck != null)
                {
                    if (BackgroundCheck.Length > 0)
                    {
                        p.Isbackgrounddoc = true;
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", p.Physicianid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        BackgroundCheckPath = Path.Combine(uploadsFolderPath, "BC.pdf");
                        using (var stream = new FileStream(BackgroundCheckPath, FileMode.Create))
                        {
                            BackgroundCheck.CopyToAsync(stream);
                        }
                    }
                }
                var Hippa = model.Hippa;
                var HippaPath = "";
                if (Hippa != null)
                {
                    if (Hippa.Length > 0)
                    {
                        p.Iscredentialdoc = true;
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", p.Physicianid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        HippaPath = Path.Combine(uploadsFolderPath, "CRED.pdf");
                        using (var stream = new FileStream(HippaPath, FileMode.Create))
                        {
                            Hippa.CopyToAsync(stream);
                        }
                    }
                }
                var NDA = model.NDA;
                var NDAPath = "";
                if (NDA != null)
                {
                    if (NDA.Length > 0)
                    {
                        p.Isnondisclosuredoc = true;
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", p.Physicianid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        NDAPath = Path.Combine(uploadsFolderPath, "NDA.pdf");
                        using (var stream = new FileStream(NDAPath, FileMode.Create))
                        {
                            NDA.CopyToAsync(stream);
                        }
                    }
                }
                _context.Physicians.Update(p);
                _context.SaveChanges();
            }
            _context.Physicians.Update(p);
            _context.SaveChanges();
            return true;
        }

        public List<Physicianlocation> GetProviders()
        {
            return _context.Physicianlocations.ToList();
        }

        public Physicianpayrate GetPayRate(int id)
        {
            return _context.Physicianpayrates.FirstOrDefault(x => x.PhysicianId == id);
        }
        public bool SavePayRate(int Physicianid, int rate, int type)
        {

            var check = _context.Physicianpayrates.Any(p => p.PhysicianId == Physicianid);

            if (!check)
            {
                var payrate = new Physicianpayrate
                {
                    PhysicianId = Physicianid
                };
                _context.Physicianpayrates.Add(payrate);
                _context.SaveChanges();
            }

            var data = _context.Physicianpayrates.FirstOrDefault(p => p.PhysicianId == Physicianid);
            switch (type)
            {
                case 1:
                    data.NightShiftWeekend = rate;
                    break;
                case 2:
                    data.Shift = rate;
                    break;
                case 3:
                    data.HouseCallNightWeekend = rate;
                    break;
                case 4:
                    data.PhoneConsult = rate;
                    break;
                case 5:
                    data.PhoneConsultNightWeekend = rate;
                    break;
                case 6:
                    data.BatchTesting = rate;
                    break;
                case 7:
                    data.HouseCall = rate;
                    break;
            }
            _context.Physicianpayrates.Update(data);
            _context.SaveChanges();
            return true;
        }
    }
}
