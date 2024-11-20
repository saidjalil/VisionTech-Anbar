using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

namespace VisionTech_Anbar_Project.Services;

public class WarehouseService
{
    private readonly WarehouseRepository _warehouseRepository;

    public WarehouseService(WarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
    {
        try
        {
            Log.Information("Fetching all warehouses.");
            var warehouses = await _warehouseRepository.GetAllAsync();
            Log.Information("Successfully retrieved {Count} warehouses.", warehouses?.Count() ?? 0);
            return warehouses;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all warehouses.");
            throw;
        }
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching warehouse with ID: {Id}.", id);
            var warehouse = await _warehouseRepository.GetByIdAsync(id);

            if (warehouse == null)
            {
                Log.Warning("Warehouse with ID: {Id} not found.", id);
            }
            else
            {
                Log.Information("Warehouse with ID: {Id} successfully retrieved.", id);
            }

            return warehouse;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching warehouse with ID: {Id}.", id);
            throw;
        }
    }

    public async Task CreateWarehouseAsync(Warehouse warehouse)
    {
        if (warehouse == null)
        {
            Log.Error("Attempted to create a null warehouse.");
            throw new ArgumentNullException(nameof(warehouse), "Warehouse cannot be null.");
        }

        try
        {
            Log.Information("Creating a new warehouse with details: {@Warehouse}.", warehouse);
            await _warehouseRepository.AddAsync(warehouse);
            Log.Information("Warehouse successfully created.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a warehouse.");
            throw;
        }
    }

    public async Task UpdateWarehouseAsync(Warehouse warehouse)
    {
        if (warehouse == null)
        {
            Log.Error("Attempted to update a null warehouse.");
            throw new ArgumentNullException(nameof(warehouse), "Warehouse cannot be null.");
        }

        try
        {
            Log.Information("Updating warehouse with ID: {Id}.", warehouse.Id);
            await _warehouseRepository.UpdateAsync(warehouse);
            Log.Information("Warehouse with ID: {Id} successfully updated.", warehouse.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating warehouse with ID: {Id}.", warehouse.Id);
            throw;
        }
    }

    public async Task DeleteWarehouseAsync(int id)
    {
        try
        {
            Log.Information("Deleting warehouse with ID: {Id}.", id);
            var warehouse = await _warehouseRepository.GetByIdAsync(id);

            if (warehouse == null)
            {
                Log.Warning("Warehouse with ID: {Id} not found. Cannot delete.", id);
                return;
            }

            await _warehouseRepository.DeleteAsync(id);
            Log.Information("Warehouse with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting warehouse with ID: {Id}.", id);
            throw;
        }
    }
}
