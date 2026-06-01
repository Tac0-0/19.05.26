using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class PaymentsControllerTests
{
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
}
