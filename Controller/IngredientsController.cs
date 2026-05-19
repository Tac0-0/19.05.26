using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class IngredientsController
{
    public async Task<List<Ingredients>> GetAllIngredients(){await using DonerDBContext c=new(); return await c.Ingredients.ToListAsync();}
    public async Task<List<Ingredients>> GetLowStockIngredients(decimal threshold = 10){await using DonerDBContext c=new(); return await c.Ingredients.Where(i=>i.QuantityInStock<=threshold).ToListAsync();}
    public async Task AddIngredient(Ingredients ingredient){await using DonerDBContext c=new(); await c.Ingredients.AddAsync(ingredient); await c.SaveChangesAsync();}
    public async Task UpdateIngredient(Ingredients ingredient){await using DonerDBContext c=new(); c.Ingredients.Update(ingredient); await c.SaveChangesAsync();}
    public async Task DeleteIngredient(int id){await using DonerDBContext c=new(); var i=await c.Ingredients.FindAsync(id); if(i is null) return; c.Ingredients.Remove(i); await c.SaveChangesAsync();}
    public async Task IncreaseStock(int id, decimal amount){await using DonerDBContext c=new(); var i=await c.Ingredients.FindAsync(id); if(i is null) return; i.QuantityInStock+=amount; await c.SaveChangesAsync();}
    public async Task DecreaseStock(int id, decimal amount){await using DonerDBContext c=new(); var i=await c.Ingredients.FindAsync(id); if(i is null) return; i.QuantityInStock-=amount; await c.SaveChangesAsync();}
}
