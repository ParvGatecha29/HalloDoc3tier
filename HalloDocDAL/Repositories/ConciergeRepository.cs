using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;

namespace HalloDocDAL.Repositories
{
    public class ConciergeRepository : IConciergeRepository
    {
        private readonly ApplicationDbContext _context;

        public ConciergeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddConcierge(Concierge con)
        {
            _context.Concierges.Add(con);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
