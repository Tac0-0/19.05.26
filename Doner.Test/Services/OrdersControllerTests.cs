using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class OrdersControllerTests
{
    [Test]
    public async Task OrdersControllerSupportsQueriesCreationStatusAndTotals()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new OrdersController(factory.CreateContext);
        var details = new List<OrderDetails> { new() { ProductId = seed.ProductId, Quantity = 3, UnitPrice = 2, Subtotal = 6 } };
        var order = new Orders { UserId = seed.CustomerId, AddressId = seed.AddressId, OrderDate = DateTime.Now.AddMinutes(31), OrderType = OrderType.Takeaway, OrderStatus = OrderStatus.Pending };

        Assert.That(await controller.GetAllOrders(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrdersByUser(seed.CustomerId), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrdersByStatus(OrderStatus.Completed), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrderById(seed.OrderId), Is.Not.Null);
        Assert.That(controller.CalculateTotal(details), Is.EqualTo(6));
        Orders created = await controller.CreateOrder(order, details);
        Assert.That(created.TotalPrice, Is.EqualTo(6));
        Assert.That((Func<Task>)(async () => await controller.CreateOrder(
            new Orders { UserId = seed.CustomerId, AddressId = seed.AddressId, OrderDate = DateTime.Now.AddMinutes(29), OrderType = OrderType.Takeaway, OrderStatus = OrderStatus.Pending },
            [])), Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Order date and time must be at least 30 minutes from now."));
        await controller.UpdateOrderStatus(created.OrderId, OrderStatus.Accepted);
        Assert.That((await controller.GetOrderById(created.OrderId))!.OrderStatus, Is.EqualTo(OrderStatus.Accepted));
        await controller.CancelOrder(created.OrderId);
        await controller.UpdateOrderStatus(-1, OrderStatus.Cancelled);
        Assert.That((await controller.GetOrderById(created.OrderId))!.OrderStatus, Is.EqualTo(OrderStatus.Cancelled));
        Assert.That(await controller.GetOrderById(-1), Is.Null);
    }
    [Test]
    public void InvalidArgumentsAndContextCreationFailuresArePropagated()
    {
        var factory = new ControllerTestFactory();
        var controller = new OrdersController(factory.CreateContext);
        Assert.That((Action)(() => controller.CalculateTotal(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.CreateOrder(null!, [])), Throws.TypeOf<NullReferenceException>());
        Assert.That((Func<Task>)(async () => await controller.CreateOrder(new Orders(), null!)), Throws.TypeOf<ArgumentNullException>());

        var failingController = new OrdersController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await failingController.GetAllOrders(),
            async () => await failingController.GetOrdersByUser(1),
            async () => await failingController.GetOrdersByStatus(OrderStatus.Pending),
            async () => await failingController.GetOrderById(1),
            async () => await failingController.CreateOrder(new Orders { OrderDate = DateTime.Now.AddMinutes(31) }, []),
            async () => await failingController.UpdateOrderStatus(1, OrderStatus.Pending),
            async () => await failingController.CancelOrder(1));
    }

}
