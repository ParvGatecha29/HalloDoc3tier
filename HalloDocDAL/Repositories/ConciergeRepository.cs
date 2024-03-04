using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
