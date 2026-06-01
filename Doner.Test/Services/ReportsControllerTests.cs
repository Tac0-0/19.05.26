using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class ReportsControllerTests
{
    [Test]
    public async Task ReportsControllerBuildsIncomeSalesStatusStockAndHistoryReports()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new ReportsController(factory.CreateContext);

        Assert.That(await controller.GetDailyIncome(new DateTime(2026, 5, 15)), Is.EqualTo(25));
        Assert.That(await controller.GetDailyIncome(new DateTime(2020, 1, 1)), Is.Zero);
        Assert.That(await controller.GetMonthlyIncome(2026, 5), Is.EqualTo(25));
        Assert.That(await controller.GetMonthlyIncome(2020, 1), Is.Zero);
        Assert.That((await controller.GetBestSellingProducts()).Single(), Is.EqualTo(("Chicken Doner", 2)));
        Assert.That((await controller.GetOrdersCountByStatus()).Single(), Is.EqualTo((OrderStatus.Completed, 1)));
        Assert.That(await controller.GetLowStockReport(), Has.Count.EqualTo(1));
        Assert.That(await controller.GetLowStockReport(5), Is.Empty);
        Assert.That(await controller.GetCustomerOrderHistory(seed.CustomerId), Has.Count.EqualTo(1));
        Assert.That(await controller.GetCustomerOrderHistory(-1), Is.Empty);
    }
}
