using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Doner.Test.Helpers;

internal sealed class ControllerTestFactory
{
    private static readonly InMemoryDatabaseRoot DatabaseRoot = new();
    private readonly DbContextOptions<DonerDBContext> options;

    public ControllerTestFactory()
    {
        options = new DbContextOptionsBuilder<DonerDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString(), DatabaseRoot)
            .Options;

        using DonerDBContext context = CreateContext();
        context.Database.EnsureCreated();
    }

    public DonerDBContext CreateContext() => new(options);

    public async Task<SeedData> SeedAsync()
    {
        await using DonerDBContext context = CreateContext();
        var customer = new Customers
        {
            UserName = "customer", Password = "password", Email = "customer@example.com",
            FirstName = "Casey", LastName = "Customer", PhoneNumber = "111", Role = UserRole.Customer, IsActive = true
        };
        var employee = new Employees
        {
            UserName = "courier", Password = "password", Email = "courier@example.com",
            FirstName = "Drew", LastName = "Driver", PhoneNumber = "222", Role = UserRole.Employee, IsActive = true,
            EmployeePosition = EmployeePosition.DeliveryWorker, HireDate = new DateTime(2026, 1, 1), Salary = 3000
        };
        var admin = new Admins
        {
            UserName = "admin", Password = "password", Email = "admin@example.com",
            FirstName = "Avery", LastName = "Admin", PhoneNumber = "333", Role = UserRole.Admin, IsActive = true
        };
        var address = new UserAddresses { User = customer, AddressType = AddressType.Home, AddressLine = "1 Main St", City = "Testville", IsDefault = true };
        var category = new Categories { Name = "Wraps", Description = "Main dishes" };
        var product = new Products { Name = "Chicken Doner", Category = category, ProductSize = ProductSize.Medium, MeatType = MeatType.Chicken, Price = 12.50m, IsAvailable = true };
        var supplier = new Suppliers { Name = "Fresh Foods", PhoneNumber = "444", Address = "2 Supply Rd" };
        var ingredient = new Ingredients { Name = "Chicken", QuantityInStock = 8, Unit = IngredientUnit.Kilogram, Supplier = supplier };
        var order = new Orders
        {
            User = customer, Address = address, OrderDate = new DateTime(2026, 5, 15, 12, 0, 0), OrderType = OrderType.Delivery,
            OrderStatus = OrderStatus.Completed, TotalPrice = 25, OrderDetails = new List<OrderDetails>(), Payments = new List<Payments>()
        };
        var detail = new OrderDetails { Order = order, Product = product, Quantity = 2, UnitPrice = 12.50m, Subtotal = 25 };
        var payment = new Payments { Order = order, PaymentMethod = PaymentMethod.Card, PaymentStatus = PaymentStatus.Unpaid, AmountPaid = 25, PaymentDate = order.OrderDate };
        var delivery = new Deliveries { Order = order, Address = address, DeliveryStatus = DeliveryStatus.WaitingForCourier, DeliveryFee = 3 };
        var recipe = new ProductIngredients { Product = product, Ingredient = ingredient, RequiredQuantity = .2m };
        context.AddRange(customer, employee, admin, address, category, product, supplier, ingredient, order, detail, payment, delivery, recipe);
        await context.SaveChangesAsync();
        return new SeedData(customer.UserId, employee.UserId, admin.UserId, address.UserAddressId, category.CategoryId, product.ProductId,
            supplier.SupplierId, ingredient.IngredientId, order.OrderId, detail.OrderDetailsId, payment.PaymentId, delivery.DeliveryId, recipe.ProductIngredientId);
    }
}

internal record SeedData(int CustomerId, int EmployeeId, int AdminId, int AddressId, int CategoryId, int ProductId,
    int SupplierId, int IngredientId, int OrderId, int DetailId, int PaymentId, int DeliveryId, int RecipeId);
