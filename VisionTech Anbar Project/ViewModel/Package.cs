using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.ViewModel
{
    public class Package
    {
        public string PackageId { get; set; }

        public DateTime CreatedDate { get; set; }
        public List<Product> Products { get; set; }

        public bool Exported { get; set; }

        public Package()
        {
            PackageId = string.Empty;
            CreatedDate = DateTime.MinValue;
            Exported = false;
            Products = new List<Product>();
        }

        public Package(string packageId, DateTime createdDate, bool exported, List<Product> products)
        {
            PackageId = packageId;
            CreatedDate = createdDate;
            Exported = exported;
            Products = products;
        }

        public void Copy(Package package)
        {
            PackageId = package.PackageId;
            CreatedDate = package.CreatedDate;
            Exported = package.Exported;
            Products = package.Products;
        }


    }
}
