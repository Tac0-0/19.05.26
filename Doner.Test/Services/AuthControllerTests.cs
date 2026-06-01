using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class AuthControllerTests
{
    [Test]
    public async Task RegisterLoginAndLogoutManageCustomerSession()
    {
        var factory = new ControllerTestFactory();
        var controller = new AuthController(factory.CreateContext());
        var customer = NewCustomer("new-user", "new@example.com");

        Assert.That(await controller.Register(customer), Is.True);
        Assert.That(customer.IsActive, Is.True);
        Assert.That(await controller.Register(NewCustomer("new-user", "different@example.com")), Is.False);
        Assert.That(await controller.Register(NewCustomer("different", "new@example.com")), Is.False);
        Assert.That(await controller.Login("new-user", "wrong"), Is.Null);
        Assert.That(await controller.Login("new-user", "password"), Is.SameAs(customer));
        Assert.That(await controller.GetLoggedUser(), Is.SameAs(customer));
        Assert.That(await controller.IsCustomer(), Is.True);
        Assert.That(await controller.IsAdmin(), Is.False);
        await controller.Logout();
        Assert.That(await controller.GetLoggedUser(), Is.Null);
    }

    [Test]
    public async Task LoginRejectsInactiveUserAndNormalizesLegacyAdminRole()
    {
        var factory = new ControllerTestFactory();
        await using (var context = factory.CreateContext())
        {
            context.Users.Add(NewCustomer("inactive", "inactive@example.com", false));
            context.Users.Add(new Admins
            {
                UserName = "legacy-admin", Password = "password", Email = "legacy@example.com", FirstName = "Legacy",
                LastName = "Admin", PhoneNumber = "1", IsActive = true, Role = (UserRole)99
            });
            await context.SaveChangesAsync();
        }

        var controller = new AuthController(factory.CreateContext());
        Assert.That(await controller.Login("inactive", "password"), Is.Null);
        Assert.That((await controller.Login("legacy-admin", "password"))!.Role, Is.EqualTo(UserRole.Admin));
        Assert.That(await controller.IsAdmin(), Is.True);
    }

    private static Customers NewCustomer(string username, string email, bool active = false) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "User",
        PhoneNumber = "1", Role = UserRole.Customer, IsActive = active
    };
}
