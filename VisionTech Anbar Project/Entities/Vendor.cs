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
        public List<Package> Packages { get; set; }
    }
}
