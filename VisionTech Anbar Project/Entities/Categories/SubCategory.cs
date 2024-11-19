using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities.Categories
{
    internal class SubCategory : BaseItem
    {
        public string Name { get; set; }
        public List<Type> SubCategories { get; set; }
        

    }
}
