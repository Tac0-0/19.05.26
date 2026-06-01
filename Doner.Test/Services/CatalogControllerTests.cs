using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class CatalogControllerTests
{
    [Test]
    public async Task CategoriesControllerSupportsCrudAndMissingDelete()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new CategoriesController(factory.CreateContext);
        var added = new Categories { Name = "Sides", Description = "Extras" };

        Assert.That(await controller.GetAllCategories(), Has.Count.EqualTo(1));
        Assert.That((await controller.GetCategoryById(seed.CategoryId))!.Name, Is.EqualTo("Wraps"));
        await controller.AddCategory(added);
        added.Name = "Updated Sides";
        await controller.UpdateCategory(added);
        Assert.That((await controller.GetCategoryById(added.CategoryId))!.Name, Is.EqualTo("Updated Sides"));
        await controller.DeleteCategory(added.CategoryId);
        await controller.DeleteCategory(-1);
        Assert.That(await controller.GetCategoryById(added.CategoryId), Is.Null);
    }

    [Test]
    public async Task SuppliersControllerSupportsCrudAndMissingDelete()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new SuppliersController(factory.CreateContext);
        var added = new Suppliers { Name = "Backup" };

        Assert.That(await controller.GetAllSuppliers(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetSupplierById(seed.SupplierId), Is.Not.Null);
        await controller.AddSupplier(added);
        added.Name = "Updated Backup";
        await controller.UpdateSupplier(added);
        Assert.That((await controller.GetSupplierById(added.SupplierId))!.Name, Is.EqualTo("Updated Backup"));
        await controller.DeleteSupplier(added.SupplierId);
        await controller.DeleteSupplier(-1);
        Assert.That(await controller.GetSupplierById(added.SupplierId), Is.Null);
    }

    [Test]
    public async Task ProductsControllerSupportsQueriesCrudAndAvailability()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new ProductsController(factory.CreateContext);
        var added = new Products { Name = "Beef Doner", CategoryId = seed.CategoryId, ProductSize = ProductSize.Large, MeatType = MeatType.Beef, Price = 15, IsAvailable = false };

        Assert.That(await controller.GetAllProducts(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetAvailableProducts(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetProductsByCategory(seed.CategoryId), Has.Count.EqualTo(1));
        Assert.That(await controller.GetProductById(seed.ProductId), Is.Not.Null);
        await controller.AddProduct(added);
        added.Name = "Updated Beef";
        await controller.UpdateProduct(added);
        await controller.SetAvailability(added.ProductId, true);
        await controller.SetAvailability(-1, true);
        Assert.That((await controller.GetProductById(added.ProductId))!.IsAvailable, Is.True);
        await controller.DeleteProduct(added.ProductId);
        await controller.DeleteProduct(-1);
        Assert.That(await controller.GetProductById(added.ProductId), Is.Null);
    }

    [Test]
    public async Task IngredientsControllerSupportsStockQueriesCrudAndAdjustments()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new IngredientsController(factory.CreateContext);
        var added = new Ingredients { Name = "Tomato", QuantityInStock = 20, Unit = IngredientUnit.Kilogram, SupplierId = seed.SupplierId };

        Assert.That(await controller.GetAllIngredients(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetLowStockIngredients(), Has.Count.EqualTo(1));
        await controller.AddIngredient(added);
        added.Name = "Updated Tomato";
        await controller.UpdateIngredient(added);
        await controller.IncreaseStock(added.IngredientId, 5);
        await controller.DecreaseStock(added.IngredientId, 2);
        await controller.IncreaseStock(-1, 1);
        await controller.DecreaseStock(-1, 1);
        await using (var context = factory.CreateContext())
            Assert.That((await context.Ingredients.FindAsync(added.IngredientId))!.QuantityInStock, Is.EqualTo(23));
        await controller.DeleteIngredient(added.IngredientId);
        await controller.DeleteIngredient(-1);
    }

    [Test]
    public async Task ProductIngredientsControllerSupportsRecipeCrudAndMissingRows()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new ProductIngredientsController(factory.CreateContext);

        Assert.That(await controller.GetRecipeByProduct(seed.ProductId), Has.Count.EqualTo(1));
        await controller.AddIngredientToProduct(seed.ProductId, seed.IngredientId, .4m);
        int addedId = (await controller.GetRecipeByProduct(seed.ProductId)).Max(x => x.ProductIngredientId);
        await controller.UpdateRequiredQuantity(addedId, .6m);
        await controller.UpdateRequiredQuantity(-1, 1);
        Assert.That((await controller.GetRecipeByProduct(seed.ProductId)).Single(x => x.ProductIngredientId == addedId).RequiredQuantity, Is.EqualTo(.6m));
        await controller.RemoveIngredientFromProduct(addedId);
        await controller.RemoveIngredientFromProduct(-1);
    }
}
