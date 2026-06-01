using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class DeliveriesControllerTests
{
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
    [Test]
    public void ContextCreationFailuresArePropagated()
    {
        var controller = new DeliveriesController(ControllerExceptionAssertions.ThrowContextCreation);
        ControllerExceptionAssertions.AssertContextCreationFailure(
            async () => await controller.GetAllDeliveries(),
            async () => await controller.GetDeliveriesByWorker(1),
            async () => await controller.GetDeliveriesByStatus(DeliveryStatus.Assigned),
            async () => await controller.AssignDeliveryWorker(1, 1),
            async () => await controller.UpdateDeliveryStatus(1, DeliveryStatus.Delivered),
            async () => await controller.CompleteDelivery(1),
            async () => await controller.CancelDelivery(1));
    }

}
