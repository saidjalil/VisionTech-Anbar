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


        public bool IsExported { get; set; }
        public string Type { get; set; }
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
            this.PackageProducts = new List<PackageProduct>();
            this.Reciver = string.Empty;
            this.Adress = string.Empty;

            Warehouse = null;
            Vendor = null;
            CreatedTime = DateTime.MinValue;
            // qebul eden 
    
        }
            
        public Package(DateTime createdTime, Vendor vendor, Warehouse warehouse, List<PackageProduct> packageProducts, string reciever, string adress)
        {
            this.Warehouse = new Warehouse();
            this.Vendor = new Vendor();
            this.PackageProducts = new List<PackageProduct>();
            // PackageName = packageName;
            CreatedTime = createdTime;
            Vendor = vendor;
            Warehouse = warehouse;
            PackageProducts = packageProducts;
            Reciver = reciever;
            Adress = adress;
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
