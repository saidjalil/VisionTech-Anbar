using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.Entities
{
    public class Product
    {
        public string ProductName { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }

        public int PackageId { get; set; }
        public List<Package> Packages { get; set; }
    }
}
