using System.ComponentModel.DataAnnotations;

namespace HalloDocDAL.Model
{
    public class Register
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string? token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int ? Year { get; set; }
        public string ? Month { get; set; }
        public int ? Date { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
    }
}
