using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class PackageService
{
    private readonly PackageRepository _packageRepository;

    public PackageService(PackageRepository packageRepository)
    {
        _packageRepository = packageRepository;
    }

    public async Task<IEnumerable<Package>> GetAllPackagesAsync()
    {
        return await _packageRepository.GetAll(x => x.PackageProducts);
    }

    public async Task<Package> GetPackageByIdAsync(int id)
    {
        return await _packageRepository.FindAsyncById(id);
    }

    public async Task CreatePackageAsync(Package package)
    {
        await _packageRepository.Create(package);
    }

    public async Task UpdatePackageAsync(Package package)
    {
        await _packageRepository.Update(package);
    }

    public async Task DeletePackageAsync(int id)
    {
        var item = await _packageRepository.FindAsyncById(id);
        
        await _packageRepository.Remove(item);
    }

    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity)
    {
        await _packageRepository.AddProductToPackageAsync(packageId, productId, quantity);
    }

    public async Task<IEnumerable<Product>> GetProductsByPackageIdAsync(int packageId)
    {
        return await _packageRepository.GetProductsByPackageIdAsync(packageId);
    }
}