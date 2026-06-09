using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class AuthControllerTests
{
    [Test]
    public void DefaultConstructorCreatesController()
    {
        Assert.That((Action)(() => _ = new AuthController()), Throws.Nothing);
    }

    [Test]
    public async Task RegisterLoginAndLogoutManageCustomerSession()
    {
        var factory = new ControllerTestFactory();
        var controller = new AuthController(factory.CreateContext());
        var customer = NewCustomer("new-user", "new@example.com");
        var employee = NewEmployee("cashier", "cashier@example.com");
        var admin = NewAdmin("admin", "admin@example.com");

        Assert.That(await controller.Register(customer), Is.True);
        Assert.That(await controller.Register(employee), Is.True);
        Assert.That(await controller.Register(admin), Is.True);
        Assert.That(customer.IsActive, Is.True);
        Assert.That(employee.IsActive, Is.True);
        Assert.That(admin.IsActive, Is.True);
        Assert.That(await controller.Register(NewCustomer("new-user", "different@example.com")), Is.False);
        Assert.That(await controller.Register(NewCustomer("different", "new@example.com")), Is.False);
        Assert.That(await controller.Register(NewEmployee("cashier", "different-employee@example.com")), Is.False);
        Assert.That(await controller.Register(NewAdmin("admin", "different-admin@example.com")), Is.False);
        Assert.That(await controller.Login("new-user", "wrong"), Is.Null);
        Assert.That(await controller.Login("new-user", "password"), Is.SameAs(customer));
        Assert.That(await controller.GetLoggedUser(), Is.SameAs(customer));
        Assert.That(await controller.IsCustomer(), Is.True);
        Assert.That(await controller.IsAdmin(), Is.False);
        await controller.Logout();
        Assert.That(await controller.GetLoggedUser(), Is.Null);
        Assert.That(await controller.IsCustomer(), Is.False);
        Assert.That(await controller.IsAdmin(), Is.False);
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

    [Test]
    public async Task DatabaseExceptionsArePropagated()
    {
        var context = new ControllerTestFactory().CreateContext();
        await context.DisposeAsync();
        var controller = new AuthController(context);

        Assert.That((Func<Task>)(async () => await new AuthController(new ControllerTestFactory().CreateContext()).Register(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.Login("user", "password")), Throws.TypeOf<ObjectDisposedException>());
        Assert.That((Func<Task>)(async () => await controller.Register(NewCustomer("user", "user@example.com"))), Throws.TypeOf<ObjectDisposedException>());
    }

    private static Customers NewCustomer(string username, string email, bool active = false) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "User",
        PhoneNumber = "1", Role = UserRole.Customer, IsActive = active
    };

    private static Employees NewEmployee(string username, string email) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "Employee",
        PhoneNumber = "1", Role = UserRole.Employee, EmployeePosition = EmployeePosition.Cashier, IsActive = true,
        HireDate = DateTime.Now
    };

    private static Admins NewAdmin(string username, string email) => new()
    {
        UserName = username, Password = "password", Email = email, FirstName = "Test", LastName = "Admin",
        PhoneNumber = "1", Role = UserRole.Admin, IsActive = true
    };
}
