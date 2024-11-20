using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productRepository.FindAsyncById(id);
    }

    public async Task CreateProductAsync(Product product)
    {
        await _productRepository.Create(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.Update(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var item = await _productRepository.FindAsyncById(id);
        await _productRepository.Remove(item);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _productRepository.GetByCategoryIdAsync(categoryId);
    }
}