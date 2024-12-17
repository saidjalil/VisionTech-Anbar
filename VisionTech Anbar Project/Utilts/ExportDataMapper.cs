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
            List<Category> categories = new();

            // Add the product's direct category first
            Category category = new()
            {
                id = product.CategoryId,
                name = product.Category.Name,
                description = "",
                icon = "",
                parent_id = product.Category.ParentId
            };
            categories.Add(category);

            // Traverse the parent categories
            int? currentParentId = product.Category.ParentId; // Start with the parent of the product's category

            while (currentParentId != null)
            {
                var parentCategory = await _categoryService.GetCategoryByIdAsync(currentParentId.Value);
                if (parentCategory == null) break; // If the parent does not exist, exit the loop

                Category parentCategoryVM = new()
                {
                    id = parentCategory.Id,
                    name = parentCategory.Name,
                    description = "",
                    icon = "",
                    parent_id = parentCategory.ParentId
                };
                categories.Add(parentCategoryVM);

                currentParentId = parentCategory.ParentId; // Update the current parent ID to the next level up
            }

            Product productVM = new()
            {
                id = product.Id,
                name = product.ProductName,
                categories = categories,
                photo = null, // Assuming this should be a placeholder for the image
                quantity = product.PackageProducts.Where(x => x.ProductId == product.Id).Sum(x => x.Quantity),
                barcodes = product.Barcodes.Select(b => b.BarCode).ToList(),
                is_permanent = product.IsRegular ? 1 : 0
            };

            productsVM.Add(productVM);
        }

        Warehouse warehouse = new()
        {
            id = package.WarehouseId,
            name = package.Warehouse.WarehouseName,
            description = null,
        };
        Vendor vendor = new()
        {
            id = package.VendorId,
            name = package.Vendor.VendorName,
            description = null,
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
            file = image.Base64,
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