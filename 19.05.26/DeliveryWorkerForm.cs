using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class DeliveryWorkerForm : Form
    {
        private readonly AuthController _authController = new();
        private readonly DeliveriesController _deliveriesController = new();

        public DeliveryWorkerForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload()
            {
                Users? user = await _authController.GetLoggedUser();
                await UiGridHelper.BindAsync(grid, () => user is null ? Task.FromResult(new List<Deliveries>()) : _deliveriesController.GetDeliveriesByWorker(user.UserId));
            }
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Change status", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                DeliveryStatusChange change = new() { DeliveryStatus = selected.DeliveryStatus };
                if (!UiGridHelper.EditEntity(change, "Change delivery status")) return;
                await _deliveriesController.UpdateDeliveryStatus(selected.DeliveryId, change.DeliveryStatus);
                await reload();
            });
            Load += async (_, _) => await reload();
        }

        private sealed class DeliveryStatusChange { public DeliveryStatus DeliveryStatus { get; set; } }
    }
}
