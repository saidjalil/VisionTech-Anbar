using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.ViewModel
{
    public class Product
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public Product()
        {
            Name = string.Empty;
            Description = string.Empty;
            Quantity = string.Empty;
        }

        public Product(string id, string name, string description, string quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
        }

        public void Copy(Product product)
        {
            Name = product.Name;
            Description = product.Description;
            Quantity = product.Quantity;
        }

    }


}
