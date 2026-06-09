using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class DeliveriesController : DbController
{
    public DeliveriesController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<Deliveries>> GetAllDeliveries()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Deliveries.ToListAsync();
    }

    public async Task<List<Deliveries>> GetDeliveriesByWorker(int userId)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Deliveries.Where(d => d.DeliveryWorkerId == userId).ToListAsync();
    }

    public async Task<List<Deliveries>> GetDeliveriesByStatus(DeliveryStatus status)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Deliveries.Where(d => d.DeliveryStatus == status).ToListAsync();
    }

    public async Task<List<Employees>> GetActiveDeliveryWorkers()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Users
            .OfType<Employees>()
            .Where(u => u.IsActive && u.EmployeePosition == EmployeePosition.DeliveryWorker)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ThenBy(u => u.UserName)
            .ToListAsync();
    }

    public async Task AssignDeliveryWorker(int deliveryId, int userId)
    {
        await using DonerDBContext context = CreateContext();
        Deliveries? delivery = await context.Deliveries.FindAsync(deliveryId);
       
        if (delivery is null)
        {
            return;
        }

        bool isDeliveryWorker = await context.Users
            .OfType<Employees>()
            .AnyAsync(u => u.UserId == userId && u.IsActive && u.EmployeePosition == EmployeePosition.DeliveryWorker);
        if (!isDeliveryWorker)
        {
            throw new InvalidOperationException("Select an active delivery worker.");
        }

        delivery.DeliveryWorkerId = userId;
        delivery.DeliveryStatus = DeliveryStatus.Assigned;
        await context.SaveChangesAsync();
    }
}

    public async Task UpdateDeliveryStatus(int deliveryId, DeliveryStatus status)
    {
        await using DonerDBContext context = CreateContext();
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
