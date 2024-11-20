using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

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
        return await _warehouseRepository.GetAllAsync();
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int id)
    {
        return await _warehouseRepository.GetByIdAsync(id);
    }

    public async Task CreateWarehouseAsync(Warehouse warehouse)
    {
        await _warehouseRepository.AddAsync(warehouse);
    }

    public async Task UpdateWarehouseAsync(Warehouse warehouse)
    {
        await _warehouseRepository.UpdateAsync(warehouse);
    }

    public async Task DeleteWarehouseAsync(int id)
    {
        await _warehouseRepository.DeleteAsync(id);
    }
}