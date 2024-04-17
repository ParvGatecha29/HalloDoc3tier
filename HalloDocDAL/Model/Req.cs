using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocDAL.Model
{
    public class Req
    {
        [Required]
        public string? firstName { get; set; }

        
        public string? lastName { get; set; }

        public DateTime dob { get; set; }
        public int? date {  get; set; }
        
        public string? month { get; set; }

        public int? year { get; set; }

        [Required]
        public string? email { get; set; }
        public int? userid { get; set; }

     
        public string? phone { get; set; }

        public string? street { get; set;}

        public string? city { get; set; }

        public string? state { get; set; }
        public int? region { get; set; }

        public string? zipcode { get; set; }

        public string? room { get; set; }

        public int typeid { get; set; }

        public string? password { get; set; }

        public string? confirmpassword { get; set;}

        public string? cfirstName { get; set; }
        public string? clastName { get; set; }
        public string? cphone { get; set; }
        public string? cemail { get; set; }

        public string? propbus { get; set; }

        public IFormFileCollection? document { get; set; }
        public string? symptoms { get; set; }
        public int? createduserid { get; set; }
    }
}
