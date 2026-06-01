using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class CustomerOrderForm : Form
    {
        private readonly OrderDetailsController _controller = new();
        private readonly int _orderId;

        public CustomerOrderForm(int orderId = 0)
        {
            InitializeComponent();
            _orderId = orderId;
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, () => _controller.GetDetailsByOrder(_orderId));
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add product", async () =>
            {
                ProductQuantity input = new();
                if (!UiGridHelper.EditEntity(input, "Add product")) return;
                await _controller.AddProductToOrder(_orderId, input.ProductId, input.Quantity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit quantity", async () =>
            {
                OrderDetails? selected = UiGridHelper.GetSelected<OrderDetails>(grid);
                if (selected is null) return;
                Quantity input = new() { Value = selected.Quantity };
                if (!UiGridHelper.EditEntity(input, "Edit quantity")) return;
                await _controller.UpdateQuantity(selected.OrderDetailsId, input.Value);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                OrderDetails? selected = UiGridHelper.GetSelected<OrderDetails>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("order item")) return;
                await _controller.RemoveProductFromOrder(selected.OrderDetailsId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Recalculate", async () =>
            {
                OrderDetails? selected = UiGridHelper.GetSelected<OrderDetails>(grid);
                if (selected is null) return;
                await _controller.RecalculateSubtotal(selected.OrderDetailsId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }

        private sealed class ProductQuantity { public int ProductId { get; set; } public int Quantity { get; set; } }
        private sealed class Quantity { public int Value { get; set; } }
    }
}
