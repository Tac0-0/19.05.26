using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class UsersControllerTests
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
    public async Task QueriesNormalizeLegacyRoles()
    {
        var factory = new ControllerTestFactory();
        var customer = NewCustomer("legacy", "legacy@example.com");
        customer.Role = (UserRole)99;
        await using (var context = factory.CreateContext())
        {
            context.Users.Add(customer);
            await context.SaveChangesAsync();
        }

        var controller = new UsersController(factory.CreateContext);

        Assert.That(await controller.GetUsersByRole(UserRole.Customer), Has.Count.EqualTo(1));
        Assert.That((await controller.GetUserById(customer.UserId))!.Role, Is.EqualTo(UserRole.Customer));
    }

    private static Customers NewCustomer(string username, string email) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "User",
        PhoneNumber = "1", Role = UserRole.Customer, IsActive = true
    };
}
