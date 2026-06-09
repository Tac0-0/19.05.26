using Doner.Controller;

namespace _19._05._26
{
    public partial class JsonImportForm : Form
    {
        private readonly JsonController _controller = new();

        public JsonImportForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.JsonDataTransfer)) return;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Choose JSON data file",
                CheckFileExists = true
            };
            if (dialog.ShowDialog(this) == DialogResult.OK) pathTextBox.Text = dialog.FileName;
        }

        private async void importButton_Click(object sender, EventArgs e)
        {
            string path = pathTextBox.Text.Trim();
            if (!File.Exists(path))
            {
                MessageBox.Show("Choose an existing JSON file first.", "JSON import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Replace the current database with the selected JSON backup?", "Confirm JSON import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            await RunTransferAsync("Importing JSON data...", "JSON data imported successfully.", () => _controller.ImportAll(path));
        }

        private async void exportButton_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Export JSON backup",
                FileName = $"doner-backup-{DateTime.Now:yyyy-MM-dd}.json",
                DefaultExt = "json",
                AddExtension = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            pathTextBox.Text = dialog.FileName;
            await RunTransferAsync("Exporting JSON data...", "JSON backup exported successfully.", () => _controller.ExportAll(dialog.FileName));
        }

        private async Task RunTransferAsync(string progressMessage, string successMessage, Func<Task> transfer)
        {
            SetBusy(true, progressMessage);
            try
            {
                await transfer();
                statusLabel.Text = successMessage;
                MessageBox.Show(successMessage, "JSON data transfer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                statusLabel.Text = "The JSON data transfer failed.";
                MessageBox.Show(ex.GetBaseException().Message, "JSON data transfer failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false, statusLabel.Text);
            }
        }

        private void SetBusy(bool busy, string status)
        {
            browseButton.Enabled = !busy;
            importButton.Enabled = !busy;
            exportButton.Enabled = !busy;
            pathTextBox.Enabled = !busy;
            UseWaitCursor = busy;
            statusLabel.Text = status;
        }
    }
}
