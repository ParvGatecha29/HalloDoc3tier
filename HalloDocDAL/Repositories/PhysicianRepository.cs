using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public Physician GetPhysicianById(int id)
        {
            var data = _context.Physicians.Include(x => x.Physicianregions).AsQueryable() ;

            return data.FirstOrDefault(x => x.Physicianid == id);
        }

        public Physician GetPhysicianByEmail(string email)
        {
            return _context.Physicians.Include(x => x.Physicianregions).FirstOrDefault(p => p.Email == email);
        }

        public List<Physicianregion> GetPhysicianRegions(int id)
        {
            return _context.Physicianregions.Where(x=> x.Physicianid==id).ToList();
        }

        public bool EditPhysician(Provider model)
        {
            Physician p = _context.Physicians.FirstOrDefault(x => x.Physicianid == model.physician.Physicianid);
            if (model.formtype == 2)
            {
                p.Firstname = model.physician.Firstname;
                p.Lastname = model.physician.Lastname;
                p.Email = model.physician.Email;
                p.Mobile = model.physician.Lastname;
                p.Medicallicense = model.physician.Medicallicense;
                p.Npinumber = model.physician.Npinumber;
                foreach (var region in model.selectedRegions)
                {
                    if (_context.Physicianregions.FirstOrDefault(x => x.Physicianid == model.physician.Physicianid && x.Regionid == region) == null)
                    {
                        Physicianregion physicianregion = new Physicianregion
                        {
                            Physicianid = p.Physicianid,
                            Regionid = region
                        };
                        _context.Physicianregions.Add(physicianregion);
                    }

                }
                List<Physicianregion> remove = _context.Physicianregions.Where(x => !model.selectedRegions.Contains(x.Regionid)).ToList();
                _context.Physicianregions.RemoveRange(remove);
            }
            else if(model.formtype == 3)
            {
                p.Address1 = model.physician.Address1;
                p.Address2 = model.physician.Address2;
                p.City = model.physician.City;
                p.Zip = model.physician.Zip;
                p.Altphone = model.physician.Altphone;
            }
            _context.Physicians.Update(p);
            _context.SaveChanges();
            return true;
        }
    }
}
