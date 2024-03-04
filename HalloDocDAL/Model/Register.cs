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
    }
}
