using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class OrderDetailsController
{
    public async Task<List<OrderDetails>> GetDetailsByOrder(int orderId){await using DonerDBContext c=new(); return await c.OrderDetails.Where(x=>x.OrderId==orderId).ToListAsync();}
    public async Task AddProductToOrder(int orderId, int productId, int quantity){await using DonerDBContext c=new(); var p=await c.Products.FindAsync(productId); if(p is null) return; var d=new OrderDetails{OrderId=orderId,ProductId=productId,Quantity=quantity,UnitPrice=p.Price,Subtotal=p.Price*quantity}; await c.OrderDetails.AddAsync(d); await c.SaveChangesAsync();}
    public async Task RemoveProductFromOrder(int orderDetailId){await using DonerDBContext c=new(); var d=await c.OrderDetails.FindAsync(orderDetailId); if(d is null) return; c.OrderDetails.Remove(d); await c.SaveChangesAsync();}
    public async Task UpdateQuantity(int orderDetailId, int quantity){await using DonerDBContext c=new(); var d=await c.OrderDetails.FindAsync(orderDetailId); if(d is null) return; d.Quantity=quantity; d.Subtotal=d.UnitPrice*quantity; await c.SaveChangesAsync();}
    public async Task RecalculateSubtotal(int orderDetailId){await using DonerDBContext c=new(); var d=await c.OrderDetails.FindAsync(orderDetailId); if(d is null) return; d.Subtotal=d.UnitPrice*d.Quantity; await c.SaveChangesAsync();}
}
