using VisionTech_Anbar_Project.Repositories;

namespace VisionTech_Anbar_Project.Services;

public class ImageService
{
    private readonly ImageRepository _imageRepository;

    public ImageService(ImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<IEnumerable<Entities.Image>> GetAllImagesAsync()
    {
        return await _imageRepository.GetAll();
    }

    public async Task<Entities.Image> GetImageByIdAsync(int id)
    {
        return await _imageRepository.FindAsyncById(id);
    }

    public async Task CreateImageAsync(Entities.Image image)
    {
        await _imageRepository.Create(image);
    }

    public async Task UpdateImageAsync(Entities.Image image)
    {
        await _imageRepository.Update(image);
    }

    public async Task DeleteImageAsync(int id)
    {
        var img = await _imageRepository.FindAsyncById(id);
        await _imageRepository.Remove(img);
    }

    public async Task<IEnumerable<Entities.Image>> GetImagesByPackageIdAsync(int packageId)
    {
        return await _imageRepository.GetImagesByPackageIdAsync(packageId);
    }
}