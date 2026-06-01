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
        var order = new Orders { UserId = seed.CustomerId, AddressId = seed.AddressId, OrderDate = DateTime.UtcNow, OrderType = OrderType.Takeaway, OrderStatus = OrderStatus.Pending };

        Assert.That(await controller.GetAllOrders(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrdersByUser(seed.CustomerId), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrdersByStatus(OrderStatus.Completed), Has.Count.EqualTo(1));
        Assert.That(await controller.GetOrderById(seed.OrderId), Is.Not.Null);
        Assert.That(controller.CalculateTotal(details), Is.EqualTo(6));
        Orders created = await controller.CreateOrder(order, details);
        Assert.That(created.TotalPrice, Is.EqualTo(6));
        await controller.UpdateOrderStatus(created.OrderId, OrderStatus.Accepted);
        Assert.That((await controller.GetOrderById(created.OrderId))!.OrderStatus, Is.EqualTo(OrderStatus.Accepted));
        await controller.CancelOrder(created.OrderId);
        await controller.UpdateOrderStatus(-1, OrderStatus.Cancelled);
        Assert.That((await controller.GetOrderById(created.OrderId))!.OrderStatus, Is.EqualTo(OrderStatus.Cancelled));
        Assert.That(await controller.GetOrderById(-1), Is.Null);
    }
}
