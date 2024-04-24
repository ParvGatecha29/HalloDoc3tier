using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentDAL.Model
{
    public class TableView
    {
        public int Patientid { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        public string Firstname { get; set;}

        [Required(ErrorMessage = "Please enter Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Age")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Please enter Phone Number")]
        public string? Phonenumber { get; set; }

        [Required(ErrorMessage = "Please enter Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter Disease")]
        public string Disease{ get; set; }

        [Required(ErrorMessage = "Please enter Doctor")]
        public string Doctor{ get; set; }
    }
}
