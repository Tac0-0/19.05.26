using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class ProductIngredientsControllerTests
{
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
        Assert.That(await controller.GetRecipeByProduct(seed.ProductId), Has.Count.EqualTo(1));
    }
    [Test]
    public void ContextCreationFailuresArePropagated()
    {
        var controller = new ProductIngredientsController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await controller.GetRecipeByProduct(1),
            async () => await controller.AddIngredientToProduct(1, 1, 1),
            async () => await controller.RemoveIngredientFromProduct(1),
            async () => await controller.UpdateRequiredQuantity(1, 1));
    }

}
