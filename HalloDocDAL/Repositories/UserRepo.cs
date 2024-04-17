using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;

namespace HalloDocDAL.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(User user)
        {
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditUser(User user)
        {
            _context.Users.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
