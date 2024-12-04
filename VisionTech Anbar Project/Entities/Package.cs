using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;
using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.ViewModel;

namespace VisionTech_Anbar_Project.Entities
{
    public class Package : BaseItem
    {

        // One-to-Many relationship with PackageProduct
        public ICollection<PackageProduct> PackageProducts { get; set; } = new List<PackageProduct>();


        public string Reciver { get; set; }
        public string Adress { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public Package()
        {
            this.Warehouse = new Warehouse();
            this.Vendor = new Vendor();

            Warehouse.WarehouseName = string.Empty;
            Vendor.VendorName = string.Empty;
            CreatedTime = DateTime.MinValue;
            // qebul eden 
    
        }
            
        public Package(DateTime createdTime, string vendorName, string warehouseName)
        {
            this.Warehouse = new Warehouse();
            this.Vendor = new Vendor();
            // PackageName = packageName;
            CreatedTime = createdTime;
            Vendor.VendorName = vendorName;
            Warehouse.WarehouseName = warehouseName;
        }

        public void Copy(Package package)
        {
            //PackageName = package.PackageName;
            CreatedTime = package.CreatedTime;
            Vendor.VendorName = package.Vendor.VendorName;
            Warehouse.WarehouseName = package.Warehouse.WarehouseName;
        }


    }
}
