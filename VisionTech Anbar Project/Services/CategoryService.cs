using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

namespace VisionTech_Anbar_Project.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        try
        {
            Log.Information("Fetching all categories.");
            var categories = await _categoryRepository.GetAllAsync();
            Log.Information("Successfully retrieved {Count} categories.", categories?.Count() ?? 0);
            return categories;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all categories.");
            throw;
        }
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching category with ID: {Id}.", id);
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                Log.Warning("Category with ID: {Id} not found.", id);
            }
            else
            {
                Log.Information("Category with ID: {Id} successfully retrieved.", id);
            }

            return category;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching category with ID: {Id}.", id);
            throw;
        }
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        Category res;
        if (category == null)
        {
            Log.Error("Attempted to create a null category.");
            throw new ArgumentNullException(nameof(category), "Category cannot be null.");
        }

        try
        {
            Log.Information("Creating a new category: {CategoryName}.", category.Name);
            res = (await _categoryRepository.Create(category)).Entity;
            Log.Information("Category {CategoryName} successfully created.", category.Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a category.");
            throw;
        }

        return res;
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        if (category == null)
        {
            Log.Error("Attempted to update a null category.");
            throw new ArgumentNullException(nameof(category), "Category cannot be null.");
        }

        try
        {
            Log.Information("Updating category with ID: {Id}.", category.Id);
            await _categoryRepository.UpdateAsync(category);
            Log.Information("Category with ID: {Id} successfully updated.", category.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating category with ID: {Id}.", category.Id);
            throw;
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        try
        {
            Log.Information("Deleting category with ID: {Id}.", id);
            await _categoryRepository.DeleteAsync(id);
            Log.Information("Category with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting category with ID: {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
    {
        try
        {
            Log.Information("Fetching subcategories for Parent ID: {ParentId}.", parentId);
            var subCategories = await _categoryRepository.GetSubCategoriesAsync(parentId);

            if (subCategories == null || !subCategories.Any())
            {
                Log.Warning("No subcategories found for Parent ID: {ParentId}.", parentId);
            }
            else
            {
                Log.Information("{Count} subcategories found for Parent ID: {ParentId}.", subCategories.Count(), parentId);
            }

            return subCategories;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching subcategories for Parent ID: {ParentId}.", parentId);
            throw;
        }
    }
    
    public async Task<Category> FindCategoryByNameAsync(string name)
    {
        return await _categoryRepository.FindCategoryByNameAsync(name);
    }
    
    public async Task<List<Category>> GetRootCategoriesAsync()
    {
        return await _categoryRepository.GetRootCategoriesAsync();
    }
}
