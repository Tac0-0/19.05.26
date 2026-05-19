using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class PaymentsController
{
    public async Task<List<Payments>> GetPaymentsByOrder(int orderId){await using DonerDBContext c=new(); return await c.Payments.Where(p=>p.OrderId==orderId).ToListAsync();}
    public async Task CreatePayment(Payments payment){await using DonerDBContext c=new(); await c.Payments.AddAsync(payment); await c.SaveChangesAsync();}
    public async Task MarkAsPaid(int orderId){await using DonerDBContext c=new(); var payment=await c.Payments.FirstOrDefaultAsync(p=>p.OrderId==orderId); if(payment is null) return; payment.PaymentStatus=PaymentStatus.Paid; await c.SaveChangesAsync();}
    public async Task RefundPayment(int paymentId){await using DonerDBContext c=new(); var payment=await c.Payments.FindAsync(paymentId); if(payment is null) return; payment.PaymentStatus=PaymentStatus.Refunded; await c.SaveChangesAsync();}
    public async Task<List<Payments>> GetPaymentsByStatus(PaymentStatus status){await using DonerDBContext c=new(); return await c.Payments.Where(p=>p.PaymentStatus==status).ToListAsync();}
}
