using Microsoft.Extensions.Configuration;
using VisionTech_Anbar_Project.Services;
using VisionTech_Anbar_Project.ViewModel;
using Image = VisionTech_Anbar_Project.Entities.Image;
using Package = VisionTech_Anbar_Project.Entities.Package;

namespace VisionTech_Anbar_Project.Utilts;

public class ExportDataMapper
{
    public static async Task<ExportViewModel> MapToExportVM(IConfiguration _configuration ,PackageService _packageService,CategoryService _categoryService,Package package, Image image)
    {
        var products = (await _packageService.GetProductsByPackageIdAsync(package.Id)).ToList();
        
        List<Product> productsVM = new();
        foreach (var product in products)
        {
            List<Category> categories = new List<Category>();

            Category category = new();
            category.id = product.CategoryId;
            category.name = product.Category.Name;
            category.description = "";
            category.icon = "";
            category.parent_id = product.Category.ParentId;
            categories.Add(category);
            
            while (true)
            {
                if (product.Category.ParentId != null)
                {
                    var parentCategory = await _categoryService.GetCategoryByIdAsync(product.Category.ParentId.Value);
                    Category parentCategoryVM = new()
                    {
                        id = parentCategory.Id,
                        description = "",
                        name = parentCategory.Name,
                        icon = "",
                        parent_id = parentCategory.ParentId
                    };
                    categories.Add(parentCategoryVM);
                }
                else
                {
                    break;
                }
            }
            
            Product productVM = new()
            {
                id = product.Id,
                name = product.ProductName,
                photo = null,
                quantity = product.PackageProducts.Where(x => x.ProductId == product.Id).Sum(x => x.Quantity),
                barcodes = product.Barcodes.Select(b => b.BarCode).ToList(),
                categories = categories,
            };
            
            if (product.IsRegular)
            {
                productVM.is_permanent = 1;
            }
            else
            {
                productVM.is_permanent = 0;
            }
            
        }

        Warehouse warehouse = new()
        {
            id = package.WarehouseId,
            name = package.Warehouse.WarehouseName,
            description = package.Warehouse.Description,
        };
        Vendor vendor = new()
        {
            id = package.VendorId,
            name = package.Vendor.VendorName,
            description = package.Vendor.Description,
        };
        Data exportData = new()
        {
            id = package.Id,
            type = "income",
            warehouse = warehouse,
            vendor = vendor,
            departments = null,
            products = productsVM,
            recipient = package.Reciver,
            place = package.Adress,
            file = image,
            created_at = package.CreatedTime,
        };
        
        Decoder decoder = new();
        var hash = decoder.GenerateHash(_configuration, exportData);
        
        ExportViewModel export = new();
        export.data = exportData;
        export.hash = hash;
        
        return export;
    }
}