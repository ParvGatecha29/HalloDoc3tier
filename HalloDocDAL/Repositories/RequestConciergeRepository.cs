using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Repositories
{
    public class RequestConciergeRepository : IRequestConciergeRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestConciergeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddConciergeRequest(Requestconcierge con)
        {
            _context.Requestconcierges.Add(con);
            Debug.WriteLine(con.Id);
            Debug.WriteLine(con.Requestid);
            Debug.WriteLine(con.Conciergeid);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
