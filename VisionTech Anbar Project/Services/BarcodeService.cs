using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

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
        try
        {
            Log.Information("Fetching all barcodes.");
            return await _barcodeRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all barcodes.");
            throw;
        }
    }

    public async Task<Barcode> GetBarcodeByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching barcode with ID: {Id}", id);

            var barcode = await _barcodeRepository.GetByIdAsync(id);
            if (barcode == null)
            {
                Log.Warning("Barcode with ID: {Id} not found.", id);
            }

            return barcode;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching barcode with ID: {Id}", id);
            throw;
        }
    }

    public async Task CreateBarcodeAsync(Barcode barcode)
    {
        if (barcode == null)
        {
            Log.Error("Attempted to create a null barcode.");
            throw new ArgumentNullException(nameof(barcode), "Barcode cannot be null.");
        }

        try
        {
            Log.Information("Creating a new barcode for ProductId: {ProductId}", barcode.ProductId);
            await _barcodeRepository.AddAsync(barcode);
            Log.Information("Barcode successfully created.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a barcode.");
            throw;
        }
    }

    public async Task UpdateBarcodeAsync(Barcode barcode)
    {
        if (barcode == null)
        {
            Log.Error("Attempted to update a null barcode.");
            throw new ArgumentNullException(nameof(barcode), "Barcode cannot be null.");
        }

        try
        {
            Log.Information("Updating barcode with ID: {Id}", barcode.Id);
            await _barcodeRepository.UpdateAsync(barcode);
            Log.Information("Barcode with ID: {Id} successfully updated.", barcode.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating barcode with ID: {Id}", barcode.Id);
            throw;
        }
    }

    public async Task DeleteBarcodeAsync(int id)
    {
        try
        {
            Log.Information("Deleting barcode with ID: {Id}", id);
            await _barcodeRepository.DeleteAsync(id);
            Log.Information("Barcode with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting barcode with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Barcode>> GetBarcodesByProductIdAsync(int productId)
    {
        try
        {
            Log.Information("Fetching barcodes for ProductId: {ProductId}", productId);
            return await _barcodeRepository.GetByProductIdAsync(productId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching barcodes for ProductId: {ProductId}", productId);
            throw;
        }
    }

    public async Task<bool> IsExist(int BarcodeId)
    {
      return await _barcodeRepository.IsExist(BarcodeId);  
    }

    public async Task<bool> IsExist(string Barcode)
    {
        var barcodes = await _barcodeRepository.GetAllAsync();
        return barcodes.Any(b => b.BarCode == Barcode);
    }
}
