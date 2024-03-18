﻿
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class AdminProfile
    {
        public int formtype {  get; set; }
        public int? adminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public string? Phone { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone1 { get; set; }

        public List<Region> regions { get; set; }
        public List<Role> role { get; set; }
      


    }
}
