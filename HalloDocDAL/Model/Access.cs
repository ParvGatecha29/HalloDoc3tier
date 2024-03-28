using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class Access
    {
        public List<Role> Roles { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Rolemenu> RoleMenus { get; set; }
        public List<int> selectedMenus { get; set; }
        public string roleName { get; set; }
        public int accountType { get; set; }
        public string userid { get; set; }
        public int edit {  get; set; }
        public int roleid {  get; set; }
    }
}
