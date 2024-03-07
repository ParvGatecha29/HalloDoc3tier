using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Contacts
{
    public interface IOrdersRepository
    {
        public List<Healthprofessionaltype> GetAllProfessions();
        public List<Healthprofessional> GetVendorsByProfession(int professionid);
        public Healthprofessional GetVendorById(int vendorid);
        public bool AddOrder(Orderdetail order);
    }
}
