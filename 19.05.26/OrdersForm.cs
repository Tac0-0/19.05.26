using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class OrdersForm : Form
    {
        private readonly OrdersController _controller = new();
        private readonly StaffAccess? _access;

        public OrdersForm(StaffAccess? access = null)
        {
            InitializeComponent();
            _access = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, _access, StaffFeature.OrderDesk)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllOrders);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Orders order = new() { OrderDate = OrdersController.GetMinimumOrderDateTime().AddMinutes(15) };
                if (!UiGridHelper.EditEntity(order, "Add order", "OrderId", "TotalPrice")) return;
                if (order.OrderDate < OrdersController.GetMinimumOrderDateTime())
                {
                    throw new InvalidOperationException("Order date and time must be at least 30 minutes from now.");
                }
                await _controller.CreateOrder(order, new List<OrderDetails>());
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Change status", async () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is null) return;
                OrderStatusChange change = new() { OrderStatus = selected.OrderStatus };
                if (!UiGridHelper.EditEntity(change, "Change order status")) return;
                await _controller.UpdateOrderStatus(selected.OrderId, change.OrderStatus);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Cancel", async () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is null) return;
                await _controller.CancelOrder(selected.OrderId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Details", () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is not null) new CustomerOrderForm(selected.OrderId).ShowDialog(this);
                return Task.CompletedTask;
            });
            UiGridHelper.AddButton(toolbar, "Payments", () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is not null) new PaymentsForm(selected.OrderId, _access).ShowDialog(this);
                return Task.CompletedTask;
            });
            Load += async (_, _) => await reload();
        }

        private sealed class OrderStatusChange
        {
            public Doner.Data.Enum.OrderStatus OrderStatus { get; set; }
        }
    }
}
