using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class OrderDetailsController
{
    public async Task<List<OrderDetails>> GetDetailsByOrder(int orderId)
    {
        await using DonerDBContext context = new();
        return await context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
    }

    public async Task AddProductToOrder(int orderId, int productId, int quantity)
    {
        await using DonerDBContext context = new();
        Products? product = await context.Products.FindAsync(productId);
        if (product is null)
        {
            return;
        }

        OrderDetails detail = new OrderDetails
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = product.Price,
            Subtotal = product.Price * quantity
        };

        await context.OrderDetails.AddAsync(detail);
        await context.SaveChangesAsync();
    }

    public async Task RemoveProductFromOrder(int orderDetailId)
    {
        await using DonerDBContext context = new();
        OrderDetails? detail = await context.OrderDetails.FindAsync(orderDetailId);
        if (detail is null)
        {
            return;
        }

        context.OrderDetails.Remove(detail);
        await context.SaveChangesAsync();
    }

    public async Task UpdateQuantity(int orderDetailId, int quantity)
    {
        await using DonerDBContext context = new();
        OrderDetails? detail = await context.OrderDetails.FindAsync(orderDetailId);
        if (detail is null)
        {
            return;
        }

        detail.Quantity = quantity;
        detail.Subtotal = detail.UnitPrice * quantity;
        await context.SaveChangesAsync();
    }

    public async Task RecalculateSubtotal(int orderDetailId)
    {
        await using DonerDBContext context = new();
        OrderDetails? detail = await context.OrderDetails.FindAsync(orderDetailId);
        if (detail is null)
        {
            return;
        }

        detail.Subtotal = detail.UnitPrice * detail.Quantity;
        await context.SaveChangesAsync();
    }
}
