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
}
