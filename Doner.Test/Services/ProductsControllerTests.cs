using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class ProductsControllerTests
{
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
    public void NullEntitiesAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new ProductsController(factory.CreateContext);
        Assert.That((Func<Task>)(async () => await controller.AddProduct(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.UpdateProduct(null!)), Throws.TypeOf<NullReferenceException>());

        var failingController = new ProductsController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAllProducts(),
            async () => await failingController.GetAvailableProducts(),
            async () => await failingController.GetProductsByCategory(1),
            async () => await failingController.GetProductById(1),
            async () => await failingController.AddProduct(new Products()),
            async () => await failingController.UpdateProduct(new Products()),
            async () => await failingController.DeleteProduct(1),
            async () => await failingController.SetAvailability(1, true));
    }

}
