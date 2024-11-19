using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities.Categories
{
    internal class Category : BaseItem
    {
        public string CategoryName { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
