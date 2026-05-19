using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class OrdersController
{
    public async Task<List<Orders>> GetAllOrders(){await using DonerDBContext c=new(); return await c.Orders.Include(o=>o.OrderDetails).ToListAsync();}
    public async Task<List<Orders>> GetOrdersByUser(int userId){await using DonerDBContext c=new(); return await c.Orders.Where(o=>o.UserId==userId).ToListAsync();}
    public async Task<List<Orders>> GetOrdersByStatus(OrderStatus status){await using DonerDBContext c=new(); return await c.Orders.Where(o=>o.OrderStatus==status).ToListAsync();}
    public async Task<Orders?> GetOrderById(int id){await using DonerDBContext c=new(); return await c.Orders.Include(o=>o.OrderDetails).FirstOrDefaultAsync(o=>o.OrderId==id);}
    public async Task<Orders> CreateOrder(Orders order, List<OrderDetails> orderDetails){await using DonerDBContext c=new(); order.TotalPrice=CalculateTotal(orderDetails); await c.Orders.AddAsync(order); await c.SaveChangesAsync(); foreach(var d in orderDetails){d.OrderId=order.OrderId; d.Subtotal=d.UnitPrice*d.Quantity;} await c.OrderDetails.AddRangeAsync(orderDetails); await c.SaveChangesAsync(); return order;}
    public async Task UpdateOrderStatus(int orderId, OrderStatus status){await using DonerDBContext c=new(); var o=await c.Orders.FindAsync(orderId); if(o is null) return; o.OrderStatus=status; await c.SaveChangesAsync();}
    public async Task CancelOrder(int orderId)=>await UpdateOrderStatus(orderId, OrderStatus.Cancelled);
    public decimal CalculateTotal(List<OrderDetails> orderDetails)=>orderDetails.Sum(d=>d.UnitPrice*d.Quantity);
}
