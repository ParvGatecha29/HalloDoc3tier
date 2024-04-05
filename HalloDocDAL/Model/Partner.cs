using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class Partner
    {
        public List<Healthprofessionaltype> Professions { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string BusinessContact { get; set; }
        public int? ProfessionId { get; set; }
        public string Profession { get; set; }
    }
}
