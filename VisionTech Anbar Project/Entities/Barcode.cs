using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities
{
    public class Barcode : BaseItem
    {
        public int BarCode { get; set; } // Unique barcode value

        // Foreign key for Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
