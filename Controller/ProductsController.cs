using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class ProductsController : DbController
{
    public ProductsController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<Products>> GetAllProducts()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Products.ToListAsync();
    }

    public async Task<List<Products>> GetAvailableProducts()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Products.Where(p => p.IsAvailable).ToListAsync();
    }

    public async Task<List<Products>> GetProductsByCategory(int categoryId)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task<Products?> GetProductById(int id)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Products.FindAsync(id);
    }

    public async Task AddProduct(Products product)
    {
        await using DonerDBContext context = CreateContext();
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateProduct(Products product)
    {
        await using DonerDBContext context = CreateContext();
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        await using DonerDBContext context = CreateContext();
        Products? product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return;
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task SetAvailability(int productId, bool isAvailable)
    {
        await using DonerDBContext context = CreateContext();
        Products? product = await context.Products.FindAsync(productId);
        if (product is null)
        {
            return;
        }

        product.IsAvailable = isAvailable;
        await context.SaveChangesAsync();
    }
}
