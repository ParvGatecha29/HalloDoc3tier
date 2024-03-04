using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
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
            return result>0;
        }

        public async Task<Aspnetuser> FindByEmail(string email)
        {
            return await _context.Aspnetusers.FirstOrDefaultAsync(x => x.Email == email);
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
            var em = _context.Tokens.FirstOrDefault(x=> x.Email == email);
            if(em == null)
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

        public bool IsUserBlocked(string email,string phone)
        {
            var block = _context.Blockrequests.FirstOrDefault(x => x.Email == email || x.Phonenumber == phone);
            if(block == null)
            {
                return false;
            }
            return (bool)!block.Isactive;
        }

        
    }
}
