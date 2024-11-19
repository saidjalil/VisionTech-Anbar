using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities.Categories
{
    public class Category : BaseItem
    {
        public string Name { get; set; } // Category name

        // Self-referencing ParentId (nullable for root categories)
        public int? ParentId { get; set; }

        // Navigation property to the parent category
        public Category Parent { get; set; }

        // Navigation property to child categories
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}
