using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;
using VisionTech_Anbar_Project.Entities.Categories;

namespace VisionTech_Anbar_Project.Entities
{
    public class Product : BaseItem
    {
        public string ProductName { get; set; }

        public bool IsRegular { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        // One-to-Many relationship with PackageProduct
        public ICollection<PackageProduct> PackageProducts { get; set; } = new List<PackageProduct>();

        // One-to-Many relationship with Barcode
        //public ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();
    }
}
