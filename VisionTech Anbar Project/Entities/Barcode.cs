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
        public int BarCode { get; set; }
        public int ProductId { get; set; }
        public List<Product> Products { get; set; }
    }
}
