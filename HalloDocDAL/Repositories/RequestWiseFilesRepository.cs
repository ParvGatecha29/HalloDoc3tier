using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Repositories
{
    public class RequestWiseFilesRepository : IRequestWiseFilesRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestWiseFilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFiles(Requestwisefile rwf)
        {
            _context.Requestwisefiles.Add(rwf);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public List<Requestwisefile> GetFiles(int requestid)
        {
            var obj = _context.Requestwisefiles.Where(u => u.Requestid == requestid && u.Isdeleted != true).ToList();
            return obj;
        }

        public async Task<Requestwisefile> GetFile(string id)
        {
            return await _context.Requestwisefiles.FirstOrDefaultAsync(u => u.Requestwisefileid.ToString() == id);
        }

        public bool DeleteFile(int id)
        {
            var file = _context.Requestwisefiles.Find(id);
            file.Isdeleted = true;
            _context.Update(file);
            _context.SaveChanges();
            return true;
        }
    }
}
