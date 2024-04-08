using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class PatientHistory
    {
        public string? FirstName { get; set; }
        public string? ClientName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }

        public int? UserId { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string? CreatedDate { get; set; }
        public DateTime? ConcludeDate { get; set; }
        public string? ProvideName { get; set; }
        public int? Status { get; set; }
        public string? StatusName
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "Unassigned";
                    case 2:
                        return "Accepted";
                    case 3:
                        return "Cancelled";
                    case 4:
                        return "MDEnRoute";
                    case 5:
                        return "MDONSite";
                    case 6:
                        return "Conclude";
                    case 7:
                        return "Cancelled";
                    case 8:
                        return "CancelledByPatient";
                    case 9:
                        return "Closed";
                    case 10:
                        return "Unpaid";
                    case 11:
                        return "Blocked";
                    default:
                        return "";
                }
            }
        }
        public int? RequestId { get; set; }
        public int? RequestClientId { get; set; }
        public bool? IsFinalize { get; set; }
        public bool? IsActive { get; set; }
    }
}
