using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.ViewModel
{
    internal class Package
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }

        public bool Exported { get; set; }


    }
}
