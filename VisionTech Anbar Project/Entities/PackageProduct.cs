using System.Xml.Linq;
using VisionTech_Anbar_Project.Entities.Base;
using VisionTech_Anbar_Project.Entities.Categories;

namespace VisionTech_Anbar_Project.Entities;

public class PackageProduct : BaseItem
{
    public int PackageId { get; set; }
    public Package Package { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public PackageProduct()
    {
       // PackageId = 0; 
       // ProductId = 0;
       //Quantity = 0;

        //Package = null;
       // Product = null;
    }

    public PackageProduct(int productId, string productName, int quantity,  int categoryId, List<Barcode> barcodes, bool isRegular, Brand brand)
    {
        
        this.Package = null;
        this.Product = new Product { ProductName = productName, CategoryId = categoryId, Barcodes = barcodes, Id = productId, IsRegular = isRegular, Brand = brand};
        //Description = description;
        Quantity = quantity;
        ProductId = productId;

        
    }

    public void Copy(PackageProduct packageProduct)
    {
        Product.ProductName = packageProduct.Product.ProductName;
        Product.Category = packageProduct.Product.Category;
        Product.CategoryId = packageProduct.Product.CategoryId;
        //Description = product.Description;
        Quantity = packageProduct.Quantity;
    }
}