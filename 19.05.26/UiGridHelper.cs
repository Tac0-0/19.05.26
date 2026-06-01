using System.Reflection;

namespace _19._05._26;

public static class UiGridHelper
{
    public static (DataGridView Grid, FlowLayoutPanel Toolbar) BuildGridUi(Panel hostPanel)
    {
        FlowLayoutPanel toolbar = new()
        {
            Dock = DockStyle.Top,
            Height = 44,
            Padding = new Padding(4),
            WrapContents = false,
            AutoScroll = true,
            BackColor = Color.FromArgb(74, 74, 74)
        };

        DataGridView grid = new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AutoGenerateColumns = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.FromArgb(60, 60, 60),
            BorderStyle = BorderStyle.None,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible = false
        };

        grid.CellFormatting += (_, args) =>
        {
            if (args.Value is Enum value)
            {
                args.Value = FormatEnum(value);
                args.FormattingApplied = true;
            }
        };
        grid.DataError += (_, args) =>
        {
            args.ThrowException = false;
        };

        hostPanel.Controls.Add(grid);
        hostPanel.Controls.Add(toolbar);
        return (grid, toolbar);
    }

    public static Button AddButton(FlowLayoutPanel toolbar, string text, Func<Task> onClick)
    {
        Button button = new()
        {
            Text = text,
            AutoSize = true,
            Height = 32,
            BackColor = Color.FromArgb(98, 98, 98),
            ForeColor = Color.WhiteSmoke,
            FlatStyle = FlatStyle.Flat
        };
        button.Click += async (_, _) =>
        {
            try
            {
                button.Enabled = false;
                await onClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
            }
        };
        toolbar.Controls.Add(button);
        return button;
    }

    public static async Task BindAsync<T>(DataGridView grid, Func<Task<List<T>>> loader)
    {
        try
        {
            List<T> rows = await loader();
            ConfigureColumns<T>(grid);
            grid.DataSource = null;
            grid.DataSource = rows;
            grid.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static T? GetSelected<T>(DataGridView grid) where T : class
    {
        T? selected = grid.CurrentRow?.DataBoundItem as T;
        if (selected is null)
        {
            MessageBox.Show("Select a row first.", "Selection required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        return selected;
    }

    public static bool EditEntity(object entity, string title, params string[] excludedProperties)
    {
        HashSet<string> excluded = new(excludedProperties, StringComparer.OrdinalIgnoreCase);
        List<PropertyInfo> properties = entity.GetType().GetProperties()
            .Where(property => property.CanRead && property.CanWrite && !excluded.Contains(property.Name) && !IsGeneratedIdentifier(entity, property) && IsEditable(property.PropertyType))
            .ToList();

        using Form dialog = new()
        {
            Text = title,
            Width = 430,
            Height = Math.Min(720, 110 + properties.Count * 42),
            StartPosition = FormStartPosition.CenterParent,
            AutoScroll = true
        };
        TableLayoutPanel layout = new() { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10), AutoScroll = true };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        Dictionary<PropertyInfo, Control> editors = new();

        foreach (PropertyInfo property in properties)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.Controls.Add(new Label { Text = SplitName(property.Name), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft });
            Control editor = CreateEditor(property, property.GetValue(entity));
            editors[property] = editor;
            layout.Controls.Add(editor);
        }

        FlowLayoutPanel buttons = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        Button save = new() { Text = "Save", DialogResult = DialogResult.OK, AutoSize = true };
        Button cancel = new() { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };
        buttons.Controls.Add(save);
        buttons.Controls.Add(cancel);
        layout.Controls.Add(buttons, 0, properties.Count);
        layout.SetColumnSpan(buttons, 2);
        dialog.Controls.Add(layout);
        dialog.AcceptButton = save;
        dialog.CancelButton = cancel;
        UiTheme.Apply(dialog);

        while (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                foreach ((PropertyInfo property, Control editor) in editors)
                {
                    property.SetValue(entity, ReadValue(property, editor));
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        return false;
    }

    public static async Task<T?> PromptForEntity<T>(string title, params string[] excludedProperties) where T : class, new()
    {
        T entity = new();
        return await Task.FromResult(EditEntity(entity, title, excludedProperties) ? entity : null);
    }

    public static bool ConfirmDelete(string itemName) => MessageBox.Show($"Delete the selected {itemName}?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

    private static void ConfigureColumns<T>(DataGridView grid)
    {
        if (grid.Columns.Count > 0) return;
        foreach (PropertyInfo property in typeof(T).GetProperties().Where(property => IsDisplayable(property.PropertyType) && property.Name != "Password"))
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = property.Name,
                DataPropertyName = property.Name,
                HeaderText = SplitName(property.Name),
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }
    }

    private static bool IsDisplayable(Type type)
    {
        Type actual = Nullable.GetUnderlyingType(type) ?? type;
        return actual.IsEnum || actual.IsPrimitive || actual == typeof(string) || actual == typeof(decimal) || actual == typeof(DateTime);
    }

    private static bool IsEditable(Type type) => IsDisplayable(type);

    private static bool IsGeneratedIdentifier(object entity, PropertyInfo property)
    {
        Type concreteType = entity.GetType();
        if (concreteType.Namespace != "Doner.Data.Entities") return false;
        Type? entityType = concreteType;
        while (entityType is not null && entityType != typeof(object))
        {
            if (property.Name.Equals($"{entityType.Name}Id", StringComparison.OrdinalIgnoreCase) ||
                property.Name.Equals($"{Singularize(entityType.Name)}Id", StringComparison.OrdinalIgnoreCase)) return true;
            entityType = entityType.BaseType;
        }
        return false;
    }

    private static string Singularize(string name)
    {
        if (name.EndsWith("ies", StringComparison.OrdinalIgnoreCase)) return $"{name[..^3]}y";
        if (name.EndsWith("ses", StringComparison.OrdinalIgnoreCase)) return name[..^2];
        if (name.EndsWith('s')) return name[..^1];
        return name;
    }

    private static Control CreateEditor(PropertyInfo property, object? value)
    {
        Type actual = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        if (actual.IsEnum)
        {
            ComboBox combo = new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = Enum.GetValues(actual) };
            object? selectedValue = value is not null && Enum.IsDefined(actual, value) ? value : Enum.GetValues(actual).GetValue(0);
            combo.SelectedItem = selectedValue;
            return combo;
        }
        if (actual == typeof(bool)) return new CheckBox { Dock = DockStyle.Fill, Checked = value as bool? ?? false };
        if (actual == typeof(DateTime))
        {
            DateTimePicker picker = new() { Dock = DockStyle.Fill, ShowCheckBox = Nullable.GetUnderlyingType(property.PropertyType) is not null };
            picker.Value = value as DateTime? ?? DateTime.Now;
            picker.Checked = value is not null;
            return picker;
        }
        return new TextBox { Dock = DockStyle.Fill, Text = value?.ToString() ?? "", UseSystemPasswordChar = property.Name == "Password" };
    }

    private static object? ReadValue(PropertyInfo property, Control editor)
    {
        Type? nullableType = Nullable.GetUnderlyingType(property.PropertyType);
        Type actual = nullableType ?? property.PropertyType;
        if (editor is ComboBox combo) return combo.SelectedItem;
        if (editor is CheckBox checkBox) return checkBox.Checked;
        if (editor is DateTimePicker picker) return nullableType is not null && !picker.Checked ? null : picker.Value;
        string text = ((TextBox)editor).Text.Trim();
        if (nullableType is not null && text.Length == 0) return null;
        if (actual == typeof(string)) return text;
        if (text.Length == 0) throw new InvalidOperationException($"{SplitName(property.Name)} is required.");
        try { return Convert.ChangeType(text, actual); }
        catch { throw new InvalidOperationException($"Enter a valid value for {SplitName(property.Name)}."); }
    }

    private static string FormatEnum(Enum value) => Enum.IsDefined(value.GetType(), value) ? value.ToString() : $"Unknown ({Convert.ToInt64(value)})";

    private static string SplitName(string name) => string.Concat(name.Select((character, index) => index > 0 && char.IsUpper(character) ? $" {character}" : character.ToString()));
}
