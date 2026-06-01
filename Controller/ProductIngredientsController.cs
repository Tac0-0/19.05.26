using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class ProductIngredientsController : DbController
{
    public ProductIngredientsController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<ProductIngredients>> GetRecipeByProduct(int productId)
    {
        await using DonerDBContext context = CreateContext();
        return await context.ProductIngredients.Where(pi => pi.ProductId == productId).ToListAsync();
    }

    public async Task AddIngredientToProduct(int productId, int ingredientId, decimal quantity)
    {
        await using DonerDBContext context = CreateContext();
        ProductIngredients productIngredient = new ProductIngredients
        {
            ProductId = productId,
            IngredientId = ingredientId,
            RequiredQuantity = quantity
        };

        await context.ProductIngredients.AddAsync(productIngredient);
        await context.SaveChangesAsync();
    }

    public async Task RemoveIngredientFromProduct(int id)
    {
        await using DonerDBContext context = CreateContext();
        ProductIngredients? productIngredient = await context.ProductIngredients.FindAsync(id);
        if (productIngredient is null)
        {
            return;
        }

        context.ProductIngredients.Remove(productIngredient);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRequiredQuantity(int id, decimal quantity)
    {
        await using DonerDBContext context = CreateContext();
        ProductIngredients? productIngredient = await context.ProductIngredients.FindAsync(id);
        if (productIngredient is null)
        {
            return;
        }

        productIngredient.RequiredQuantity = quantity;
        await context.SaveChangesAsync();
    }
}
