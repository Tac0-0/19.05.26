using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class ReportsController
{
    public async Task<decimal> GetDailyIncome(DateTime date)
    {
        await using DonerDBContext context = new();
        return await context.Orders
            .Where(o => o.OrderDate.Date == date.Date && o.OrderStatus == OrderStatus.Completed)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;
    }

    public async Task<decimal> GetMonthlyIncome(int year, int month)
    {
        await using DonerDBContext context = new();
        return await context.Orders
            .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month && o.OrderStatus == OrderStatus.Completed)
            .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;
    }

    public async Task<List<(string ProductName, int Quantity)>> GetBestSellingProducts()
    {
        await using DonerDBContext context = new();
        var data = await context.OrderDetails
            .Include(od => od.Product)
            .GroupBy(od => od.Product.Name)
            .Select(g => new { ProductName = g.Key, Quantity = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.Quantity)
            .ToListAsync();

        return data.Select(x => (x.ProductName, x.Quantity)).ToList();
    }

    public async Task<List<(OrderStatus Status, int Count)>> GetOrdersCountByStatus()
    {
        await using DonerDBContext context = new();
        var data = await context.Orders
            .GroupBy(o => o.OrderStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        return data.Select(x => (x.Status, x.Count)).ToList();
    }

    public async Task<List<Ingredients>> GetLowStockReport(decimal threshold = 10)
    {
        await using DonerDBContext context = new();
        return await context.Ingredients.Where(i => i.QuantityInStock <= threshold).ToListAsync();
    }

    public async Task<List<Orders>> GetCustomerOrderHistory(int userId)
    {
        await using DonerDBContext context = new();
        return await context.Orders.Where(o => o.UserId == userId).Include(o => o.OrderDetails).ToListAsync();
    }
}
