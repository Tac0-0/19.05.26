using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class PaymentsController : DbController
{
    public PaymentsController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<Payments>> GetPaymentsByOrder(int orderId)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Payments.Where(p => p.OrderId == orderId).ToListAsync();
    }

    public async Task CreatePayment(Payments payment)
    {
        await using DonerDBContext context = CreateContext();
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }

    public async Task MarkAsPaid(int orderId)
    {
        await using DonerDBContext context = CreateContext();
        Payments? payment = await context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment is null)
        {
            return;
        }

        payment.PaymentStatus = PaymentStatus.Paid;
        await context.SaveChangesAsync();
    }

    public async Task RefundPayment(int paymentId)
    {
        await using DonerDBContext context = CreateContext();
        Payments? payment = await context.Payments.FindAsync(paymentId);
        if (payment is null)
        {
            return;
        }

        payment.PaymentStatus = PaymentStatus.Refunded;
        await context.SaveChangesAsync();
    }

    public async Task<List<Payments>> GetPaymentsByStatus(PaymentStatus status)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Payments.Where(p => p.PaymentStatus == status).ToListAsync();
    }
}
