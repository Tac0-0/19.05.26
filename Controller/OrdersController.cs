using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class OrdersController
{
    public async Task<List<Orders>> GetAllOrders()
    {
        await using DonerDBContext context = new();
        return await context.Orders.Include(o => o.OrderDetails).ToListAsync();
    }

    public async Task<List<Orders>> GetOrdersByUser(int userId)
    {
        await using DonerDBContext context = new();
        return await context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<List<Orders>> GetOrdersByStatus(OrderStatus status)
    {
        await using DonerDBContext context = new();
        return await context.Orders.Where(o => o.OrderStatus == status).ToListAsync();
    }

    public async Task<Orders?> GetOrderById(int id)
    {
        await using DonerDBContext context = new();
        return await context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<Orders> CreateOrder(Orders order, List<OrderDetails> orderDetails)
    {
        await using DonerDBContext context = new();
        order.TotalPrice = CalculateTotal(orderDetails);

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        foreach (OrderDetails detail in orderDetails)
        {
            detail.OrderId = order.OrderId;
            detail.Subtotal = detail.UnitPrice * detail.Quantity;
        }

        await context.OrderDetails.AddRangeAsync(orderDetails);
        await context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrderStatus(int orderId, OrderStatus status)
    {
        await using DonerDBContext context = new();
        Orders? order = await context.Orders.FindAsync(orderId);
        if (order is null)
        {
            return;
        }

        order.OrderStatus = status;
        await context.SaveChangesAsync();
    }

    public async Task CancelOrder(int orderId)
    {
        await UpdateOrderStatus(orderId, OrderStatus.Cancelled);
    }

    public decimal CalculateTotal(List<OrderDetails> orderDetails)
    {
        return orderDetails.Sum(d => d.UnitPrice * d.Quantity);
    }
}
