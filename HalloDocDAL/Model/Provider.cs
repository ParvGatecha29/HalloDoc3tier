using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class Provider
    {
        public Aspnetuser aspphy {  get; set; }
        public Physician physician { get; set; }
        public List<Physician> physicians {  get; set; }
        public List<Region> regions { get; set; }
        public List<Physicianregion> phyregions { get; set; }

        public List<int> selectedRegions { get; set; }
        public IFormFile sign { get; set; }
        public int formtype { get; set; }
    }
}
