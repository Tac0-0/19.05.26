using Doner.Data;
using Doner.Data.Entities;
using System.Text.Json;

namespace Doner.Controller;

public class JsonImportController
{
    public async Task ImportCategories(string path)
    {
        await using DonerDBContext context = new();
        var items = JsonSerializer.Deserialize<List<Categories>>(await File.ReadAllTextAsync(path)) ?? [];
        await context.Categories.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public async Task ImportProducts(string path)
    {
        await using DonerDBContext context = new();
        var items = JsonSerializer.Deserialize<List<Products>>(await File.ReadAllTextAsync(path)) ?? [];
        await context.Products.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public async Task ImportSuppliers(string path)
    {
        await using DonerDBContext context = new();
        var items = JsonSerializer.Deserialize<List<Suppliers>>(await File.ReadAllTextAsync(path)) ?? [];
        await context.Suppliers.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public async Task ImportIngredients(string path)
    {
        await using DonerDBContext context = new();
        var items = JsonSerializer.Deserialize<List<Ingredients>>(await File.ReadAllTextAsync(path)) ?? [];
        await context.Ingredients.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public async Task ImportAll(string path)
    {
        await ImportCategories(Path.Combine(path, "categories.json"));
        await ImportProducts(Path.Combine(path, "products.json"));
        await ImportSuppliers(Path.Combine(path, "suppliers.json"));
        await ImportIngredients(Path.Combine(path, "ingredients.json"));
    }
}
