using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                foreach(var region in profile.selectedRegions)
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
    }
}
