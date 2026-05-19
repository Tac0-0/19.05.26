using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class CategoriesController
{
    public async Task<List<Categories>> GetAllCategories() { await using DonerDBContext c=new(); return await c.Categories.ToListAsync(); }
    public async Task<Categories?> GetCategoryById(int id) { await using DonerDBContext c=new(); return await c.Categories.FindAsync(id); }
    public async Task AddCategory(Categories category) { await using DonerDBContext c=new(); await c.Categories.AddAsync(category); await c.SaveChangesAsync(); }
    public async Task UpdateCategory(Categories category) { await using DonerDBContext c=new(); c.Categories.Update(category); await c.SaveChangesAsync(); }
    public async Task DeleteCategory(int id) { await using DonerDBContext c=new(); var e=await c.Categories.FindAsync(id); if(e is null) return; c.Categories.Remove(e); await c.SaveChangesAsync(); }
}
