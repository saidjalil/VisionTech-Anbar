using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;
using VisionTech_Anbar_Project.Entities.Categories;

namespace VisionTech_Anbar_Project.Entities
{
    public class Package : BaseItem
    {
        public List<Product> Products { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
