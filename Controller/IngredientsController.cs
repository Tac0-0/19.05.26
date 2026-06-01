using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class IngredientsController : DbController
{
    public IngredientsController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<Ingredients>> GetAllIngredients()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Ingredients.ToListAsync();
    }

    public async Task<List<Ingredients>> GetLowStockIngredients(decimal threshold = 10)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Ingredients.Where(i => i.QuantityInStock <= threshold).ToListAsync();
    }

    public async Task AddIngredient(Ingredients ingredient)
    {
        await using DonerDBContext context = CreateContext();
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();
    }

    public async Task UpdateIngredient(Ingredients ingredient)
    {
        await using DonerDBContext context = CreateContext();
        context.Ingredients.Update(ingredient);
        await context.SaveChangesAsync();
    }

    public async Task DeleteIngredient(int id)
    {
        await using DonerDBContext context = CreateContext();
        Ingredients? ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient is null)
        {
            return;
        }

        context.Ingredients.Remove(ingredient);
        await context.SaveChangesAsync();
    }

    public async Task IncreaseStock(int id, decimal amount)
    {
        await using DonerDBContext context = CreateContext();
        Ingredients? ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient is null)
        {
            return;
        }

        ingredient.QuantityInStock += amount;
        await context.SaveChangesAsync();
    }

    public async Task DecreaseStock(int id, decimal amount)
    {
        await using DonerDBContext context = CreateContext();
        Ingredients? ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient is null)
        {
            return;
        }

        ingredient.QuantityInStock -= amount;
        await context.SaveChangesAsync();
    }
}
