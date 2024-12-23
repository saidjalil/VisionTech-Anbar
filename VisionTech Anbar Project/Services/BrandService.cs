using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class BrandService
{
    private readonly BrandRepository _brandRepository;

    public BrandService(BrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    // Create a new brand
    public async Task<Brand> CreateBrandAsync(Brand brand)
    {
        if (string.IsNullOrWhiteSpace(brand.BrandName))
        {
            throw new ArgumentException("Brand name cannot be empty.");
        }

        var newBrand = await _brandRepository.Create(brand);
        return newBrand.Entity;
    }

    // Get all brands
    public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
    {
        return await _brandRepository.GetAll(x => x.Products);
    }

    // Get brand by ID
    public async Task<Brand> GetBrandByIdAsync(int id)
    {
        var brand = await _brandRepository.FindAsyncById(id);
        if (brand == null)
        {
            throw new KeyNotFoundException("Brand not found.");
        }
        return brand;
    }

    // Get brand with products by ID
    public async Task<Brand> GetBrandWithProductsAsync(int id)
    {
        var brand = await _brandRepository.GetBrandWithProductsAsync(id);
        if (brand == null)
        {
            throw new KeyNotFoundException("Brand not found.");
        }
        return brand;
    }

    // Update an existing brand
    public async Task UpdateBrandAsync(Brand brand)
    {
        if (string.IsNullOrWhiteSpace(brand.BrandName))
        {
            throw new ArgumentException("Brand name cannot be empty.");
        }

        await _brandRepository.Update(brand);
    }

    // Delete a brand by ID
    public async Task DeleteBrandAsync(int id)
    {
        var brand = await _brandRepository.FindAsyncById(id);
        if (brand == null)
        {
            throw new KeyNotFoundException("Brand not found.");
        }
        await _brandRepository.Remove(brand);
    }

    public async Task<Brand> GetBrandByName(string brandName)
    {
        return (await _brandRepository.GetAll()).FirstOrDefault(x => x.BrandName == brandName);
    }
} 