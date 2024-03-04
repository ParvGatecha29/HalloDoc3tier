using System.ComponentModel.DataAnnotations;

namespace HalloDocDAL.Model
{
    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
