using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDocDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(Aspnetuser user)
        {
            _context.Aspnetusers.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Aspnetuser> FindByEmail(string email)
        {
            return await _context.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> EditAspUser(Aspnetuser user)
        {
            _context.Aspnetusers.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public bool storeToken(string email, string token)
        {
            var em = _context.Tokens.FirstOrDefault(x => x.Email == email);
            if (em == null)
            {
                var tokens = new Token
                {
                    Email = email,
                    Token1 = token
                };
                _context.Tokens.Add(tokens);
                return _context.SaveChanges() > 0;
            }
            else
            {
                em.Token1 = token;
                _context.Update(em);
                return _context.SaveChanges() > 0;
            }
        }
        public string GetToken(string email)
        {
            var tokens = _context.Tokens.FirstOrDefault(x => x.Email == email);
            return tokens.Token1;
        }

        public bool IsUserBlocked(string email, string phone)
        {
            var block = _context.Blockrequests.FirstOrDefault(x => x.Email == email || x.Phonenumber == phone);
            if (block == null)
            {
                return false;
            }
            return (bool)!block.Isactive;
        }

        public Admin GetAdminById(string id)
        {
            return _context.Admins.FirstOrDefault(x => x.Aspnetuserid == id);
        }

        public List<int> GetAdminRegions(string id)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Aspnetuserid == id);

            return _context.Adminregions.Where(x => x.Adminid == admin.Adminid).Select(x => x.Regionid).ToList();
        }

        public bool UpdateAdminProfile(AdminProfile profile)
        {
            Admin admin = _context.Admins.FirstOrDefault(x => x.Adminid == profile.adminId);
            if (profile.formtype == 1)
            {
                admin.Firstname = profile.FirstName;
                admin.Lastname = profile.LastName;
                admin.Email = profile.Email;
                admin.Mobile = profile.Phone;
                foreach (var region in profile.selectedRegions)
                {
                    if (_context.Adminregions.FirstOrDefault(x => x.Adminid == profile.adminId && x.Regionid == region) == null)
                    {
                        Adminregion adminregion = new Adminregion
                        {
                            Adminid = admin.Adminid,
                            Regionid = region
                        };
                        _context.Adminregions.Add(adminregion);
                    }

                }
                List<Adminregion> remove = _context.Adminregions.Where(x => !profile.selectedRegions.Contains(x.Regionid)).ToList();
                _context.Adminregions.RemoveRange(remove);
            }
            else if (profile.formtype == 2)
            {
                admin.Address1 = profile.Address1;
                admin.Address2 = profile.Address2;
                admin.City = profile.City;
                admin.Zip = profile.Zip;
            }
            else if (profile.formtype == 3)
            {
                Aspnetuser user = _context.Aspnetusers.FirstOrDefault(x => x.Id == admin.Aspnetuserid);
                user.Passwordhash = profile.Password;
                _context.Aspnetusers.Update(user);
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return true;
        }

        public bool AddProvider(Provider model, string adminId)
        {
            var user = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Username = "MD." + model.physician.Firstname + "." + model.physician.Lastname,
                Passwordhash = model.aspphy.Passwordhash,
                Email = model.physician.Email,
                Phonenumber = model.physician.Mobile,
                Createddate = DateTime.Now
            };

            var role = new Aspnetuserrole()
            {
                UserId = user.Id,
                RoleId = "2"
            };
            _context.Aspnetusers.Add(user);
            _context.Aspnetuserroles.Add(role);
            _context.SaveChanges();

            var data = new Physician
            {
                Aspnetuserid = user.Id,
                Firstname = model.physician.Firstname,
                Lastname = model.physician.Lastname,
                Email = model.physician.Email,
                Mobile = model.physician.Mobile,
                Address1 = model.physician.Address1,
                Address2 = model.physician.Address2,
                City = model.physician.City,
                Createdby = adminId,
                Businessname = model.physician.Businessname,
                Businesswebsite = model.physician.Businesswebsite,
                Roleid = 1,
                Status = 1,
                Isdeleted = false,
                Npinumber = model.physician.Npinumber,
                //Syncemailaddress = model.Syncemailaddress,
                Medicallicense = model.physician.Medicallicense,
                Adminnotes = model.physician.Adminnotes,
                Zip = model.physician.Zip,
            };

            _context.Physicians.Add(data);
            _context.SaveChanges();

            var list = model.selectedRegions;

            foreach (var i in list)
            {
                if (!_context.Physicianregions.Any(a => a.Regionid == i))
                {
                    var a = new Physicianregion
                    {
                        Physicianid = data.Physicianid,
                        Regionid = i
                    };
                    _context.Physicianregions.Add(a);
                }
            }
            _context.SaveChanges();

            var notification = new Physiciannotification
            {
                Physicianid = data.Physicianid,
                Isnotificationstopped = false,
            };

            _context.Physiciannotifications.Add(notification);
            _context.SaveChanges();
            var file = model.sign;
            var filePath = "";
            if (file.Length > 0)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/Physician", data.Physicianid.ToString());
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                filePath = Path.Combine(uploadsFolderPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
            }
            return true;


        }
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public List<Menu> GetMenus(int AccountType)
        {
            
            var menu = _context.Menus.AsQueryable();
            if(AccountType != 0)
            {
                menu = menu.Where(x => x.Accounttype == AccountType).AsQueryable();
            }
            return menu.ToList();
        }

        public List<Rolemenu> GetRoleMenus(int roleid)
        {

            var rolemenu = _context.Rolemenus.AsQueryable();

            return rolemenu.Where(x => x.Roleid == roleid).ToList();
        }

        public bool AddRole(Access model)
        {
            if(model.edit != 1)
            {
                Role role = new Role
                {
                    Name = model.roleName,
                    Accounttype = (short)model.accountType,
                    Createdby = model.userid,
                    Createddate = DateTime.Now,
                };
                _context.Roles.Add(role);
                _context.SaveChanges();
            }
            foreach (var menu in model.selectedMenus)
            {
                if (_context.Rolemenus.FirstOrDefault(x => x.Roleid == model.roleid && x.Menuid == menu) == null)
                {
                    Rolemenu rolemenu = new Rolemenu
                    {
                        Roleid = model.roleid,
                        Menuid = menu
                    };
                    _context.Rolemenus.Add(rolemenu);
                }

            }
            List<Rolemenu> remove = _context.Rolemenus.Where(x => !model.selectedMenus.Contains(x.Menuid) && x.Roleid == model.roleid).ToList();
            _context.Rolemenus.RemoveRange(remove);
            _context.SaveChanges();
            return true;
        }

        public bool AddShift(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
            return true;
        }

        public bool AddShiftDetails(Shiftdetail shiftdetail)
        {
            _context.Shiftdetails.Add(shiftdetail);
            _context.SaveChanges();
            return true;
        }
        public bool AddShiftDetailRegions(Shiftdetailregion shiftdetailregion)
        {
            _context.Shiftdetailregions.Add(shiftdetailregion);
            _context.SaveChanges();
            return true;
        }

        public Shiftdetail FindShiftDetails(int detailId)
        {
            return _context.Shiftdetails.Find(detailId);
        }

        public bool UpdateShiftDetails(Shiftdetail details)
        {
            _context.Shiftdetails.Update(details);
            _context.SaveChanges();
            return true;
        }
        public List<ScheduleModel> GetEvents()
        {
            var eventswithoutdelet = (from s in _context.Shifts
                                      join pd in _context.Physicians on s.Physicianid equals pd.Physicianid
                                      join sd in _context.Shiftdetails on s.Shiftid equals sd.Shiftid into shiftGroup
                                      from sd in shiftGroup.DefaultIfEmpty()

                                      select new ScheduleModel
                                      {
                                          Shiftid = sd.Shiftdetailid,
                                          Status = sd.Status,
                                          Starttime = sd.Starttime,
                                          Endtime = sd.Endtime,
                                          Physicianid = pd.Physicianid,
                                          PhysicianName = pd.Firstname + ' ' + pd.Lastname,
                                          Shiftdate = sd.Shiftdate,
                                          ShiftDetailId = sd.Shiftdetailid,
                                          Regionid = sd.Regionid,
                                          ShiftDeleted = sd.Isdeleted
                                      }).ToList();
            var events = eventswithoutdelet.Where(item => !item.ShiftDeleted).ToList();
            return events;
        }
        public List<MappedEvents> GetMappedEvents()
        {
            var events = GetEvents();
            var mappedEvents = events.Select(e => new MappedEvents
            {
                id = e.Shiftid,
                resourceId = e.Physicianid,
                title = e.PhysicianName,
                start = new DateTime(e.Shiftdate.Value.Year, e.Shiftdate.Value.Month, e.Shiftdate.Value.Day, e.Starttime.Hour, e.Starttime.Minute, e.Starttime.Second),
                end = new DateTime(e.Shiftdate.Value.Year, e.Shiftdate.Value.Month, e.Shiftdate.Value.Day, e.Endtime.Hour, e.Endtime.Minute, e.Endtime.Second),
                ShiftDetailId = e.ShiftDetailId,
                region = _context.Regions.Where(i => i.Regionid == e.Regionid).ToList(),
                status = e.Status
            }).ToList();

            return mappedEvents;
        }
        public List<ScheduleModel> GetReviewShifts(int region)
        {
             return (from shiftis in _context.Shifts
             join shiftdetails in _context.Shiftdetails
             on shiftis.Shiftid equals shiftdetails.Shiftid
             join regionis in _context.Regions
             on shiftdetails.Regionid equals regionis.Regionid
             select new ScheduleModel
             {
                 Shiftid =  shiftis.Shiftid,
                 ShiftDetailId = shiftdetails.Shiftdetailid,
                 PhysicianName =  shiftis.Physician.Firstname+ shiftis.Physician.Lastname,

                 Shiftdate = shiftdetails.Shiftdate,
                 Starttime = shiftdetails.Starttime,
                 Endtime = shiftdetails.Endtime,
                 Regionid = shiftdetails.Regionid,
                 RegionName = regionis.Name,
                 Status = shiftdetails.Status
             }).Where(item => (region == 0 || item.Regionid == region) && item.Status == 1).ToList();
            
        }
    }
}
