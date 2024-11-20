using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class VendorService
{
    private readonly VendorRepository _vendorRepository;

    public VendorService(VendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
    {
        return await _vendorRepository.GetAllAsync();
    }

    public async Task<Vendor> GetVendorByIdAsync(int id)
    {
        return await _vendorRepository.GetByIdAsync(id);
    }

    public async Task CreateVendorAsync(Vendor vendor)
    {
        await _vendorRepository.AddAsync(vendor);
    }

    public async Task UpdateVendorAsync(Vendor vendor)
    {
        await _vendorRepository.UpdateAsync(vendor);
    }

    public async Task DeleteVendorAsync(int id)
    {
        await _vendorRepository.DeleteAsync(id);
    }
}