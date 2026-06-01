using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class SuppliersControllerTests
{
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
    public void NullEntitiesAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new SuppliersController(factory.CreateContext);
        Assert.That((Func<Task>)(async () => await controller.AddSupplier(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.UpdateSupplier(null!)), Throws.TypeOf<NullReferenceException>());

        var failingController = new SuppliersController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAllSuppliers(),
            async () => await failingController.GetSupplierById(1),
            async () => await failingController.AddSupplier(new Suppliers()),
            async () => await failingController.UpdateSupplier(new Suppliers()),
            async () => await failingController.DeleteSupplier(1));
    }

}
