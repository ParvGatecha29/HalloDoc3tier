using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class ViewUploads
    {
        public AdminDashboardData Request { get; set; }
        public List<Requestwisefile> Requestwisefiles { get; set; }
    }
}
