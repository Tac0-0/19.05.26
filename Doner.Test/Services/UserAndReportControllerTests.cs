using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class UserAndReportControllerTests
{
    [Test]
    public async Task UsersControllerSupportsQueriesCrudRoleAndActiveChanges()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new UsersController(factory.CreateContext);
        var added = NewCustomer("added", "added@example.com");

        Assert.That(await controller.GetAllUsers(), Has.Count.EqualTo(3));
        Assert.That(await controller.GetUsersByRole(UserRole.Customer), Has.Count.EqualTo(1));
        Assert.That(await controller.GetUserById(seed.CustomerId), Is.Not.Null);
        await controller.AddUser(added);
        added.FirstName = "Updated";
        await controller.UpdateUser(added);
        await controller.ChangeRole(added.UserId, UserRole.Admin);
        await controller.ChangeRole(-1, UserRole.Admin);
        await controller.SetActive(added.UserId, false);
        await controller.SetActive(-1, false);
        Users updated = (await controller.GetUserById(added.UserId))!;
        Assert.That(updated.FirstName, Is.EqualTo("Updated"));
        Assert.That(updated.Role, Is.EqualTo(UserRole.Admin));
        Assert.That(updated.IsActive, Is.False);
        await controller.DeleteUser(added.UserId);
        await controller.DeleteUser(-1);
        Assert.That(await controller.GetUserById(added.UserId), Is.Null);
    }

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
    }

    [Test]
    public async Task ReportsControllerBuildsIncomeSalesStatusStockAndHistoryReports()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new ReportsController(factory.CreateContext);

        Assert.That(await controller.GetDailyIncome(new DateTime(2026, 5, 15)), Is.EqualTo(25));
        Assert.That(await controller.GetDailyIncome(new DateTime(2020, 1, 1)), Is.Zero);
        Assert.That(await controller.GetMonthlyIncome(2026, 5), Is.EqualTo(25));
        Assert.That(await controller.GetMonthlyIncome(2020, 1), Is.Zero);
        Assert.That((await controller.GetBestSellingProducts()).Single(), Is.EqualTo(("Chicken Doner", 2)));
        Assert.That((await controller.GetOrdersCountByStatus()).Single(), Is.EqualTo((OrderStatus.Completed, 1)));
        Assert.That(await controller.GetLowStockReport(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetCustomerOrderHistory(seed.CustomerId), Has.Count.EqualTo(1));
    }

    private static Customers NewCustomer(string username, string email) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "User",
        PhoneNumber = "1", Role = UserRole.Customer, IsActive = true
    };
}
