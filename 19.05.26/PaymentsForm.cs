using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class PaymentsForm : Form
    {
        private readonly PaymentsController _controller = new();
        private readonly int _orderId;

        public PaymentsForm(int orderId = 0, StaffAccess? access = null)
        {
            InitializeComponent();
            _orderId = orderId;
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.OrderDesk)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, () => _controller.GetPaymentsByOrder(_orderId));
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Payments payment = new() { OrderId = _orderId, PaymentDate = DateTime.Now, PaymentStatus = PaymentStatus.Unpaid };
                if (!UiGridHelper.EditEntity(payment, "Add payment", "PaymentId", "OrderId")) return;
                await _controller.CreatePayment(payment);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Mark paid", async () =>
            {
                await _controller.MarkAsPaid(_orderId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Refund", async () =>
            {
                Payments? selected = UiGridHelper.GetSelected<Payments>(grid);
                if (selected is null) return;
                await _controller.RefundPayment(selected.PaymentId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }
    }
}
