
using HalloDocDAL.Models;

namespace HalloDocDAL.Model
{
    public class AdminProfile
    {
        public int formtype {  get; set; }
        public int? adminId { get; set; }
        public string? aspId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
        public int? Roleid { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public string? Phone { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? Stateid { get; set; }
        public int? RegionId { get; set; }
        public string? Zip { get; set; }
        public string? Phone1 { get; set; }

        public List<Region> regions { get; set; }
        public List<int> adminregions { get; set; }
        public List<int> selectedRegions { get; set; }
        public List<Role> roles { get; set; }
      


    }
}
