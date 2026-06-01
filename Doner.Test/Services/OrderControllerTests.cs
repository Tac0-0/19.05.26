using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class OrderControllerTests
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
    }

    [Test]
    public async Task OrderDetailsControllerSupportsQueriesCrudAndMissingRows()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new OrderDetailsController(factory.CreateContext);

        Assert.That(await controller.GetDetailsByOrder(seed.OrderId), Has.Count.EqualTo(1));
        await controller.AddProductToOrder(seed.OrderId, seed.ProductId, 3);
        await controller.AddProductToOrder(seed.OrderId, -1, 3);
        int addedId = (await controller.GetDetailsByOrder(seed.OrderId)).Max(x => x.OrderDetailsId);
        await controller.UpdateQuantity(addedId, 4);
        await controller.UpdateQuantity(-1, 4);
        await controller.RecalculateSubtotal(addedId);
        await controller.RecalculateSubtotal(-1);
        Assert.That((await controller.GetDetailsByOrder(seed.OrderId)).Single(x => x.OrderDetailsId == addedId).Subtotal, Is.EqualTo(50));
        await controller.RemoveProductFromOrder(addedId);
        await controller.RemoveProductFromOrder(-1);
    }

    [Test]
    public async Task PaymentsControllerSupportsQueriesCreationAndStatusChanges()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new PaymentsController(factory.CreateContext);
        var added = new Payments { OrderId = seed.OrderId, PaymentMethod = PaymentMethod.Cash, PaymentStatus = PaymentStatus.Unpaid, AmountPaid = 5, PaymentDate = DateTime.UtcNow };

        Assert.That(await controller.GetPaymentsByOrder(seed.OrderId), Has.Count.EqualTo(1));
        await controller.CreatePayment(added);
        await controller.MarkAsPaid(seed.OrderId);
        await controller.MarkAsPaid(-1);
        Assert.That(await controller.GetPaymentsByStatus(PaymentStatus.Paid), Has.Count.EqualTo(1));
        await controller.RefundPayment(added.PaymentId);
        await controller.RefundPayment(-1);
        Assert.That(await controller.GetPaymentsByStatus(PaymentStatus.Refunded), Has.Count.EqualTo(1));
    }

    [Test]
    public async Task DeliveriesControllerSupportsQueriesAssignmentCompletionAndCancellation()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new DeliveriesController(factory.CreateContext);

        Assert.That(await controller.GetAllDeliveries(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetDeliveriesByStatus(DeliveryStatus.WaitingForCourier), Has.Count.EqualTo(1));
        await controller.AssignDeliveryWorker(seed.DeliveryId, seed.EmployeeId);
        await controller.AssignDeliveryWorker(-1, seed.EmployeeId);
        Assert.That(await controller.GetDeliveriesByWorker(seed.EmployeeId), Has.Count.EqualTo(1));
        await controller.CompleteDelivery(seed.DeliveryId);
        await using (var context = factory.CreateContext())
            Assert.That((await context.Deliveries.FindAsync(seed.DeliveryId))!.DeliveredAt, Is.Not.Null);
        await controller.CancelDelivery(seed.DeliveryId);
        await controller.UpdateDeliveryStatus(-1, DeliveryStatus.Failed);
        Assert.That(await controller.GetDeliveriesByStatus(DeliveryStatus.Cancelled), Has.Count.EqualTo(1));
    }
}
