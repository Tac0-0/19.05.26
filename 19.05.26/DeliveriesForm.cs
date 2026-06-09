using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class DeliveriesForm : Form
    {
        private readonly DeliveriesController _controller = new();

        public DeliveriesForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.DeliveryManagement)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllDeliveries);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Assign worker", async () =>
            {
                Deliveries? selected = UiGridHelper.GetSelected<Deliveries>(grid);
                if (selected is null) return;
                Employees? worker = await PromptForDeliveryWorker();
                if (worker is null) return;
                await _controller.AssignDeliveryWorker(selected.DeliveryId, worker.UserId);
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

        private async Task<Employees?> PromptForDeliveryWorker()
        {
            List<Employees> workers = await _controller.GetActiveDeliveryWorkers();
            if (workers.Count == 0)
            {
                MessageBox.Show("No active delivery workers are available.", "Assign delivery worker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            List<WorkerOption> options = workers.Select(worker => new WorkerOption(worker)).ToList();
            using Form dialog = new()
            {
                Text = "Assign delivery worker",
                Width = 440,
                Height = 150,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            TableLayoutPanel layout = new() { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

            ComboBox workerCombo = new()
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = options,
                DisplayMember = nameof(WorkerOption.DisplayName)
            };
            layout.Controls.Add(new Label { Text = "Delivery worker", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            layout.Controls.Add(workerCombo, 1, 0);

            FlowLayoutPanel buttons = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            Button assign = new() { Text = "Assign", DialogResult = DialogResult.OK, AutoSize = true };
            Button cancel = new() { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };
            buttons.Controls.Add(assign);
            buttons.Controls.Add(cancel);
            layout.Controls.Add(buttons, 0, 1);
            layout.SetColumnSpan(buttons, 2);
            dialog.Controls.Add(layout);
            dialog.AcceptButton = assign;
            dialog.CancelButton = cancel;
            UiTheme.Apply(dialog);

            return dialog.ShowDialog(this) == DialogResult.OK && workerCombo.SelectedItem is WorkerOption selected
                ? selected.Worker
                : null;
        }

        private sealed class WorkerOption
        {
            public WorkerOption(Employees worker)
            {
                Worker = worker;
                string displayName = $"{worker.FirstName} {worker.LastName}".Trim();
                DisplayName = string.IsNullOrWhiteSpace(displayName)
                    ? $"{worker.UserName} (#{worker.UserId})"
                    : $"{displayName} ({worker.UserName}, #{worker.UserId})";
            }

            public Employees Worker { get; }
            public string DisplayName { get; }
        }

        private sealed class DeliveryStatusChange { public DeliveryStatus DeliveryStatus { get; set; } }
    }
}