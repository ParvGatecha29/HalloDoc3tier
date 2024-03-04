using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class CustomModel
    {
        public User user { get; set; }
        public List<Requestwisefile> requests {  get; set; }
        public List<Dashboard> dashboards {  get; set; }
    }
}
