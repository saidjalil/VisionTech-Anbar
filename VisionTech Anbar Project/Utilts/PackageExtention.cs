using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.Utilts
{
    internal static class PackageExtention
    {
        // public static void AddProduct(this Package package, Product product)
        // {
        //     package.Products.Add(product);
        // }
        //
        // public static List<Product> GetProducts(this Package package)
        // {
        //     return package.Products;
        // }
        //
        // public static Product GetProduct(this Package package, string productId)
        // {
        //     var prod = package.Products.FirstOrDefault(x => x.Id == productId);
        //
        //     if (prod == null)
        //     {
        //         throw new Exception("Product not found");
        //     }
        //
        //     return prod;
        // }
        //
        // public static void DeleteProduct(this Package package, string productId)
        // {
        //     var prod = package.Products.FirstOrDefault(x => x.Id == productId);
        //     if (prod == null)
        //     {
        //         throw new Exception("Product not found");
        //     }
        //
        //     package.Products.Remove(prod);
        // }
        //
        // public static void UpdateProduct(this Package package, Product product)
        // {
        //     DeleteProduct(package, product.Id);
        //     product.Id = product.Id;
        //     AddProduct(package, product);
        // }
    }
}
