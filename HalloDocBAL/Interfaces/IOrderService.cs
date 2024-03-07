using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IOrderService
    {
        public List<Healthprofessionaltype> GetAllPrfessions();
        public List<Healthprofessional> GetVendorsByProfessions(int professionid);
        public Healthprofessional GetVendorById(int vendoridid);
        public bool CreateOrder(Orderdetail order);
    }
}
