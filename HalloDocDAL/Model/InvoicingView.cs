using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class InvoicingView
    {
        public List<TimesheetModel> timesheets {  get; set; }
        public List<InvoicingModel> invoicing { get; set; }
        public AdminInvoicing Sheet {  get; set; }
        public Physicianpayrate Physicianpayrate { get; set; }
        public bool isFinalize {  get; set; }
        public int PhysicianId {  get; set; }
        public DateTime startDate { get; set; }
    }
}
