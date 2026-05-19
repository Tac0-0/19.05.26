using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class ProductIngredientsController
{
    public async Task<List<ProductIngredients>> GetRecipeByProduct(int productId){await using DonerDBContext c=new(); return await c.ProductIngredients.Where(pi=>pi.ProductId==productId).ToListAsync();}
    public async Task AddIngredientToProduct(int productId, int ingredientId, decimal quantity){await using DonerDBContext c=new(); await c.ProductIngredients.AddAsync(new ProductIngredients{ProductId=productId,IngredientId=ingredientId,RequiredQuantity=quantity}); await c.SaveChangesAsync();}
    public async Task RemoveIngredientFromProduct(int id){await using DonerDBContext c=new(); var pi=await c.ProductIngredients.FindAsync(id); if(pi is null) return; c.ProductIngredients.Remove(pi); await c.SaveChangesAsync();}
    public async Task UpdateRequiredQuantity(int id, decimal quantity){await using DonerDBContext c=new(); var pi=await c.ProductIngredients.FindAsync(id); if(pi is null) return; pi.RequiredQuantity=quantity; await c.SaveChangesAsync();}
}
