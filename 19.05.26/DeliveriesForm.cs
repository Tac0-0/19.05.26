using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class DeliveriesForm : Form
    {
        private readonly DeliveriesController _controller = new();

        public DeliveriesForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllDeliveries);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Assign worker", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                WorkerAssignment assignment = new();
                if (!UiGridHelper.EditEntity(assignment, "Assign delivery worker")) return;
                await _controller.AssignDeliveryWorker(selected.DeliveryId, assignment.UserId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Change status", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                DeliveryStatusChange change = new() { DeliveryStatus = selected.DeliveryStatus };
                if (!UiGridHelper.EditEntity(change, "Change delivery status")) return;
                await _controller.UpdateDeliveryStatus(selected.DeliveryId, change.DeliveryStatus);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Complete", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                await _controller.CompleteDelivery(selected.DeliveryId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Cancel", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                await _controller.CancelDelivery(selected.DeliveryId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }

        private sealed class WorkerAssignment { public int UserId { get; set; } }
        private sealed class DeliveryStatusChange { public DeliveryStatus DeliveryStatus { get; set; } }
    }
}
