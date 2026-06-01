using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class CategoriesControllerTests
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
    public void NullEntitiesAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new CategoriesController(factory.CreateContext);
        Assert.That((Func<Task>)(async () => await controller.AddCategory(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.UpdateCategory(null!)), Throws.TypeOf<NullReferenceException>());

        var failingController = new CategoriesController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAllCategories(),
            async () => await failingController.GetCategoryById(1),
            async () => await failingController.AddCategory(new Categories()),
            async () => await failingController.UpdateCategory(new Categories()),
            async () => await failingController.DeleteCategory(1));
    }

}
