using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class ProductsController
{
    public async Task<List<Products>> GetAllProducts() { await using DonerDBContext c=new(); return await c.Products.ToListAsync(); }
    public async Task<List<Products>> GetAvailableProducts() { await using DonerDBContext c=new(); return await c.Products.Where(p=>p.IsAvailable).ToListAsync(); }
    public async Task<List<Products>> GetProductsByCategory(int categoryId) { await using DonerDBContext c=new(); return await c.Products.Where(p=>p.CategoryId==categoryId).ToListAsync(); }
    public async Task<Products?> GetProductById(int id) { await using DonerDBContext c=new(); return await c.Products.FindAsync(id); }
    public async Task AddProduct(Products product) { await using DonerDBContext c=new(); await c.Products.AddAsync(product); await c.SaveChangesAsync(); }
    public async Task UpdateProduct(Products product) { await using DonerDBContext c=new(); c.Products.Update(product); await c.SaveChangesAsync(); }
    public async Task DeleteProduct(int id) { await using DonerDBContext c=new(); var e=await c.Products.FindAsync(id); if(e is null) return; c.Products.Remove(e); await c.SaveChangesAsync(); }
    public async Task SetAvailability(int productId, bool isAvailable) { await using DonerDBContext c=new(); var e=await c.Products.FindAsync(productId); if(e is null) return; e.IsAvailable=isAvailable; await c.SaveChangesAsync(); }
}
