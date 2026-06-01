using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Doner.Test.Helpers;

namespace Doner.Test.Services;

public class OrderDetailsControllerTests
{
    [Test]
    public async Task OrderDetailsControllerSupportsQueriesCrudAndMissingRows()
    {
        var factory = new ControllerTestFactory();
        SeedData seed = await factory.SeedAsync();
        var controller = new OrderDetailsController(factory.CreateContext);

        Assert.That(await controller.GetDetailsByOrder(seed.OrderId), Has.Count.EqualTo(1));
        await controller.AddProductToOrder(seed.OrderId, seed.ProductId, 3);
        await controller.AddProductToOrder(seed.OrderId, -1, 3);
        int addedId = (await controller.GetDetailsByOrder(seed.OrderId)).Max(x => x.OrderDetailsId);
        await controller.UpdateQuantity(addedId, 4);
        await controller.UpdateQuantity(-1, 4);
        await controller.RecalculateSubtotal(addedId);
        await controller.RecalculateSubtotal(-1);
        Assert.That((await controller.GetDetailsByOrder(seed.OrderId)).Single(x => x.OrderDetailsId == addedId).Subtotal, Is.EqualTo(50));
        await controller.RemoveProductFromOrder(addedId);
        await controller.RemoveProductFromOrder(-1);
        Assert.That(await controller.GetDetailsByOrder(seed.OrderId), Has.Count.EqualTo(1));
    }
}
