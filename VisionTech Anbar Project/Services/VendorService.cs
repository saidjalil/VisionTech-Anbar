using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

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
        try
        {
            Log.Information("Fetching all vendors.");
            var vendors = await _vendorRepository.GetAllAsync();
            Log.Information("Successfully retrieved {Count} vendors.", vendors?.Count() ?? 0);
            return vendors;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all vendors.");
            throw;
        }
    }

    public async Task<Vendor> GetVendorByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching vendor with ID: {Id}.", id);
            var vendor = await _vendorRepository.GetByIdAsync(id);

            if (vendor == null)
            {
                Log.Warning("Vendor with ID: {Id} not found.", id);
            }
            else
            {
                Log.Information("Vendor with ID: {Id} successfully retrieved.", id);
            }

            return vendor;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching vendor with ID: {Id}.", id);
            throw;
        }
    }

    public async Task CreateVendorAsync(Vendor vendor)
    {
        if (vendor == null)
        {
            Log.Error("Attempted to create a null vendor.");
            throw new ArgumentNullException(nameof(vendor), "Vendor cannot be null.");
        }

        try
        {
            Log.Information("Creating a new vendor with details: {@Vendor}.", vendor);
            await _vendorRepository.AddAsync(vendor);
            Log.Information("Vendor successfully created.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a vendor.");
            throw;
        }
    }

    public async Task UpdateVendorAsync(Vendor vendor)
    {
        if (vendor == null)
        {
            Log.Error("Attempted to update a null vendor.");
            throw new ArgumentNullException(nameof(vendor), "Vendor cannot be null.");
        }

        try
        {
            Log.Information("Updating vendor with ID: {Id}.", vendor.Id);
            await _vendorRepository.UpdateAsync(vendor);
            Log.Information("Vendor with ID: {Id} successfully updated.", vendor.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating vendor with ID: {Id}.", vendor.Id);
            throw;
        }
    }

    public async Task DeleteVendorAsync(int id)
    {
        try
        {
            Log.Information("Deleting vendor with ID: {Id}.", id);
            var vendor = await _vendorRepository.GetByIdAsync(id);

            if (vendor == null)
            {
                Log.Warning("Vendor with ID: {Id} not found. Cannot delete.", id);
                return;
            }

            await _vendorRepository.DeleteAsync(id);
            Log.Information("Vendor with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting vendor with ID: {Id}.", id);
            throw;
        }
    }
}
