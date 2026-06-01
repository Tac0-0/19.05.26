using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class IngredientsControllerTests
{
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
        Assert.That(await controller.GetAllIngredients(), Has.Count.EqualTo(1));
    }
    [Test]
    public void NullEntitiesAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new IngredientsController(factory.CreateContext);
        Assert.That((Func<Task>)(async () => await controller.AddIngredient(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.UpdateIngredient(null!)), Throws.TypeOf<NullReferenceException>());

        var failingController = new IngredientsController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAllIngredients(),
            async () => await failingController.GetLowStockIngredients(),
            async () => await failingController.AddIngredient(new Ingredients()),
            async () => await failingController.UpdateIngredient(new Ingredients()),
            async () => await failingController.DeleteIngredient(1),
            async () => await failingController.IncreaseStock(1, 1),
            async () => await failingController.DecreaseStock(1, 1));
    }

}
