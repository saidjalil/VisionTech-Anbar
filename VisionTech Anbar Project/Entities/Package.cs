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

        // One-to-Many relationship with PackageProduct
        public ICollection<PackageProduct> PackageProducts { get; set; } = new List<PackageProduct>();

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
