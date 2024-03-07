using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class Orders
    {
        public List<Healthprofessionaltype> healthprofessionaltypes { get; set; }

        public string contact {  get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string orderdetails { get; set; }
        public int refills { get; set; }
        public int requestid { get; set; }
    }
}
