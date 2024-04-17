using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;

namespace HalloDocDAL.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly ApplicationDbContext _context;

        public BusinessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBusiness(Business bus)
        {
            _context.Businesses.Add(bus);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
