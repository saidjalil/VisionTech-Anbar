using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities.Categories
{
    public class SubCategory : BaseItem
    {
        public string Name { get; set; }

        // One-to-Many relationship
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Type> Types { get; set; } = new List<Type>();
    }
}
