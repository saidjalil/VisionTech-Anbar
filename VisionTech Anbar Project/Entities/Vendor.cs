using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities
{
    public class Vendor : BaseItem
    {
        public string VendorName { get; set; }
        public string Description { get; set; }

        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
