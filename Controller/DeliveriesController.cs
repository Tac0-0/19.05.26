using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class DeliveriesController
{
    public async Task<List<Deliveries>> GetAllDeliveries(){await using DonerDBContext c=new(); return await c.Deliveries.ToListAsync();}
    public async Task<List<Deliveries>> GetDeliveriesByWorker(int userId){await using DonerDBContext c=new(); return await c.Deliveries.Where(d=>d.DeliveryWorkerId==userId).ToListAsync();}
    public async Task<List<Deliveries>> GetDeliveriesByStatus(DeliveryStatus status){await using DonerDBContext c=new(); return await c.Deliveries.Where(d=>d.DeliveryStatus==status).ToListAsync();}
    public async Task AssignDeliveryWorker(int deliveryId, int userId){await using DonerDBContext c=new(); var d=await c.Deliveries.FindAsync(deliveryId); if(d is null) return; d.DeliveryWorkerId=userId; d.DeliveryStatus=DeliveryStatus.Assigned; await c.SaveChangesAsync();}
    public async Task UpdateDeliveryStatus(int deliveryId, DeliveryStatus status){await using DonerDBContext c=new(); var d=await c.Deliveries.FindAsync(deliveryId); if(d is null) return; d.DeliveryStatus=status; if(status==DeliveryStatus.Delivered)d.DeliveredAt=DateTime.UtcNow; await c.SaveChangesAsync();}
    public async Task CompleteDelivery(int deliveryId)=>await UpdateDeliveryStatus(deliveryId, DeliveryStatus.Delivered);
    public async Task CancelDelivery(int deliveryId)=>await UpdateDeliveryStatus(deliveryId, DeliveryStatus.Cancelled);
}
