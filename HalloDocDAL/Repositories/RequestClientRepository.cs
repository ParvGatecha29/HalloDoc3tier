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
    public class RequestClientRepository : IRequestClientRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddRequestClient(Requestclient model)
        {
            _context.Requestclients.Add(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
