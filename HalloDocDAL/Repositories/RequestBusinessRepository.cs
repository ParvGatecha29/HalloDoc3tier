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
    public class RequestBusinessRepository : IRequestBusinessRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestBusinessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBusinessRequest(Requestbusiness bus)
        {
            _context.Requestbusinesses.Add(bus);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
