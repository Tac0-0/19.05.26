using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class KitchenOrdersForm : Form
    {
        private readonly OrdersController _controller = new();

        public KitchenOrdersForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.KitchenOrders)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllOrders);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Change status", async () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is null) return;
                OrderStatusChange change = new() { OrderStatus = selected.OrderStatus };
                if (!UiGridHelper.EditEntity(change, "Change order status")) return;
                await _controller.UpdateOrderStatus(selected.OrderId, change.OrderStatus);
                await reload();
            });
            Load += async (_, _) => await reload();
        }

        private sealed class OrderStatusChange { public OrderStatus OrderStatus { get; set; } }
    }
}
