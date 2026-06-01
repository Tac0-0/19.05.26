using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class UserAddressesControllerTests
{
    [Test]
    public async Task UserAddressesControllerSupportsQueriesCrudAndDefaultSelection()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new UserAddressesController(factory.CreateContext);
        var added = new UserAddresses { UserId = seed.CustomerId, AddressType = AddressType.Work, AddressLine = "2 Office Rd", City = "Testville" };

        Assert.That(await controller.GetAddressesByUser(seed.CustomerId), Has.Count.EqualTo(1));
        Assert.That((await controller.GetDefaultAddress(seed.CustomerId))!.UserAddressId, Is.EqualTo(seed.AddressId));
        await controller.AddAddress(added);
        added.City = "Updated City";
        await controller.UpdateAddress(added);
        await controller.SetDefaultAddress(added.UserAddressId);
        await controller.SetDefaultAddress(-1);
        Assert.That((await controller.GetDefaultAddress(seed.CustomerId))!.UserAddressId, Is.EqualTo(added.UserAddressId));
        await controller.DeleteAddress(added.UserAddressId);
        await controller.DeleteAddress(-1);
        Assert.That(await controller.GetAddressesByUser(seed.CustomerId), Has.Count.EqualTo(1));
        Assert.That(await controller.GetDefaultAddress(seed.CustomerId), Is.Null);
    }
    [Test]
    public void NullEntitiesAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new UserAddressesController(factory.CreateContext);
        Assert.That((Func<Task>)(async () => await controller.AddAddress(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.UpdateAddress(null!)), Throws.TypeOf<NullReferenceException>());

        var failingController = new UserAddressesController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAddressesByUser(1),
            async () => await failingController.GetDefaultAddress(1),
            async () => await failingController.AddAddress(new UserAddresses()),
            async () => await failingController.UpdateAddress(new UserAddresses()),
            async () => await failingController.DeleteAddress(1),
            async () => await failingController.SetDefaultAddress(1));
    }

}
