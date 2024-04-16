using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class AdminDashboardData
    {
        public int requestId {  get; set; }
        public string name {  get; set; }
        public string firstName {  get; set; }
        public string lastName {  get; set; }
        public int? dobyear { get; set; }
        public string dobmonth { get; set; }
        public int? dobdate { get; set; }
        public string requestor { get; set; }
        public DateTime reqdate { get; set; }
        public string phone { get; set; }
        public string cphone { get; set; }
        public string address { get; set; }
        public int requesttype { get; set; }
        public int status { get; set; }
        public string email { get; set; }
        public DateTime requestDate { get; set; }
        public int? regionId { get; set; }
        public string? region { get; set; }
        public string physician { get; set; }
        public string notes { get; set; }
        public string confirmationNo { get; set; }
        public List<string> transferNotes { get; set; } = new List<string>();
        public string adminNotes { get; set; }
        public string physicianNotes { get; set; }
        public string cancelreqid { get; set; }
        public string reason { get; set; }
        public string info { get; set; }
        public int physicianId { get; set; }
        public string pname { get; set; }
        public List<string> tnotes { get; set; }
        public bool isFinalized { get; set; } = false;
    }
}
