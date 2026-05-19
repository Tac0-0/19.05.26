namespace _19._05._26;

public static class UiGridHelper
{
    public static (DataGridView Grid, Button Reload) BuildGridUi(Panel hostPanel, string buttonText = "Reload")
    {
        Button reloadButton = new()
        {
            Text = buttonText,
            Dock = DockStyle.Top,
            Height = 36,
            BackColor = Color.FromArgb(98, 98, 98),
            ForeColor = Color.WhiteSmoke,
            FlatStyle = FlatStyle.Flat
        };

        DataGridView grid = new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.FromArgb(60, 60, 60)
        };

        hostPanel.Controls.Add(grid);
        hostPanel.Controls.Add(reloadButton);
        return (grid, reloadButton);
    }

    public static async Task BindAsync<T>(DataGridView grid, Func<Task<List<T>>> loader)
    {
        try
        {
            grid.DataSource = await loader();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
