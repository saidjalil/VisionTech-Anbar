using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities.Categories
{
    public class Type : BaseItem
    {
        public string Name { get; set; }

        // Foreign key for SubCategory
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
