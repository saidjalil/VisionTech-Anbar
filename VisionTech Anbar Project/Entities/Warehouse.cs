using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities
{
    public class Warehouse : BaseItem
    {
        public string WarehouseName { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
