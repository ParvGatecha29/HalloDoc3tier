using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;

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
        public IFormFile ICA { get; set; }
        public IFormFile BackgroundCheck { get; set; }
        public IFormFile Hippa { get; set; }
        public IFormFile NDA { get; set; }
        public int formtype { get; set; }
    }
}
