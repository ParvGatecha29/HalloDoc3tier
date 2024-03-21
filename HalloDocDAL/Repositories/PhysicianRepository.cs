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
    public class PhysicianRepository : IPhysicianRepository
    {
        private readonly ApplicationDbContext _context;
        public PhysicianRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Physician> GetPhysicians(int region)
        {
            if(region == 0)
                return _context.Physicians.ToList();
            return _context.Physicians.Where(p => p.Regionid == region).ToList();
        }
    }
}
