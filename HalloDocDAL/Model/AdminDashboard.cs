using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class AdminDashboard
    {
        public PagedList<AdminDashboardData> pagedList {  get; set; }
        public List<AdminDashboardData> Data { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Region> regions { get; set; }
        public AdminDashboardData request { get; set; }

        public int state { get; set; }
    }
}
