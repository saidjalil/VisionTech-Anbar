using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class BarcodeService
{
    private readonly BarcodeRepository _barcodeRepository;

    public BarcodeService(BarcodeRepository barcodeRepository)
    {
        _barcodeRepository = barcodeRepository;
    }

    public async Task<IEnumerable<Barcode>> GetAllBarcodesAsync()
    {
        return await _barcodeRepository.GetAllAsync();
    }

    public async Task<Barcode> GetBarcodeByIdAsync(int id)
    {
        return await _barcodeRepository.GetByIdAsync(id);
    }

    public async Task CreateBarcodeAsync(Barcode barcode)
    {
        await _barcodeRepository.AddAsync(barcode);
    }

    public async Task UpdateBarcodeAsync(Barcode barcode)
    {
        await _barcodeRepository.UpdateAsync(barcode);
    }

    public async Task DeleteBarcodeAsync(int id)
    {
        await _barcodeRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Barcode>> GetBarcodesByProductIdAsync(int productId)
    {
        return await _barcodeRepository.GetByProductIdAsync(productId);
    }}