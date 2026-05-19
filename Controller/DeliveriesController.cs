using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class DeliveriesController
{
    public async Task<List<Deliveries>> GetAllDeliveries()
    {
        await using DonerDBContext context = new();
        return await context.Deliveries.ToListAsync();
    }

    public async Task<List<Deliveries>> GetDeliveriesByWorker(int userId)
    {
        await using DonerDBContext context = new();
        return await context.Deliveries.Where(d => d.DeliveryWorkerId == userId).ToListAsync();
    }

    public async Task<List<Deliveries>> GetDeliveriesByStatus(DeliveryStatus status)
    {
        await using DonerDBContext context = new();
        return await context.Deliveries.Where(d => d.DeliveryStatus == status).ToListAsync();
    }

    public async Task AssignDeliveryWorker(int deliveryId, int userId)
    {
        await using DonerDBContext context = new();
        Deliveries? delivery = await context.Deliveries.FindAsync(deliveryId);
        if (delivery is null)
        {
            return;
        }

        delivery.DeliveryWorkerId = userId;
        delivery.DeliveryStatus = DeliveryStatus.Assigned;
        await context.SaveChangesAsync();
    }

    public async Task UpdateDeliveryStatus(int deliveryId, DeliveryStatus status)
    {
        await using DonerDBContext context = new();
        Deliveries? delivery = await context.Deliveries.FindAsync(deliveryId);
        if (delivery is null)
        {
            return;
        }

        delivery.DeliveryStatus = status;
        if (status == DeliveryStatus.Delivered)
        {
            delivery.DeliveredAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }

    public async Task CompleteDelivery(int deliveryId)
    {
        await UpdateDeliveryStatus(deliveryId, DeliveryStatus.Delivered);
    }

    public async Task CancelDelivery(int deliveryId)
    {
        await UpdateDeliveryStatus(deliveryId, DeliveryStatus.Cancelled);
    }
}
