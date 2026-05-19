using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class CategoriesController
{
    public async Task<List<Categories>> GetAllCategories()
    {
        await using DonerDBContext context = new();
        return await context.Categories.ToListAsync();
    }

    public async Task<Categories?> GetCategoryById(int id)
    {
        await using DonerDBContext context = new();
        return await context.Categories.FindAsync(id);
    }

    public async Task AddCategory(Categories category)
    {
        await using DonerDBContext context = new();
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCategory(Categories category)
    {
        await using DonerDBContext context = new();
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCategory(int id)
    {
        await using DonerDBContext context = new();
        Categories? category = await context.Categories.FindAsync(id);
        if (category is null)
        {
            return;
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }
}
