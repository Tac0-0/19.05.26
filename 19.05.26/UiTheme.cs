using System.Drawing.Drawing2D;

namespace _19._05._26;

/// <summary>
/// Applies the shared polished visual language to every form, including forms
/// created at runtime by the grid editor.
/// </summary>
public static class UiTheme
{
    private static readonly Color Ink = Color.FromArgb(22, 31, 46);
    private static readonly Color Navy = Color.FromArgb(26, 39, 61);
    private static readonly Color Slate = Color.FromArgb(43, 59, 84);
    private static readonly Color Surface = Color.FromArgb(238, 244, 252);
    private static readonly Color Field = Color.FromArgb(251, 253, 255);
    private static readonly Color Accent = Color.FromArgb(31, 132, 255);
    private static readonly Color AccentDeep = Color.FromArgb(13, 87, 196);
    private static readonly HashSet<Control> StyledControls = new();

    public static void Enable()
    {
        Application.Idle += (_, _) =>
        {
            foreach (Form form in Application.OpenForms)
            {
                Apply(form);
            }
        };
    }

    public static void Apply(Control root)
    {
        Style(root);
        foreach (Control child in root.Controls)
        {
            Apply(child);
        }
    }

    private static void Style(Control control)
    {
        if (!StyledControls.Add(control)) return;
        control.ControlAdded += (_, args) => Apply(args.Control);

        switch (control)
        {
            case Form form:
                form.BackColor = Ink;
                form.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
                break;
            case DataGridView grid:
                StyleGrid(grid);
                break;
            case Button button:
                StyleButton(button);
                break;
            case TextBox textBox:
                textBox.BackColor = Field;
                textBox.ForeColor = Ink;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                break;
            case ComboBox comboBox:
                comboBox.BackColor = Field;
                comboBox.ForeColor = Ink;
                comboBox.FlatStyle = FlatStyle.Flat;
                break;
            case DateTimePicker picker:
                picker.CalendarForeColor = Ink;
                picker.CalendarMonthBackground = Field;
                break;
            case Label label:
                label.ForeColor = IsOnLightSurface(label) ? Ink : Color.FromArgb(233, 241, 255);
                break;
            case FlowLayoutPanel flowPanel:
                StylePanel(flowPanel, flowPanel.Parent is TableLayoutPanel ? Surface : Slate);
                break;
            case TableLayoutPanel tablePanel:
                StylePanel(tablePanel, Surface);
                break;
            case Panel panel:
                StylePanel(panel, PanelColor(panel));
                break;
        }
    }

    private static void StyleButton(Button button)
    {
        button.BackColor = Accent;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
        button.Padding = new Padding(8, 0, 8, 0);
        button.Paint += PaintGlossyButton;
        button.Resize += (_, _) => Round(button, 10);
        Round(button, 10);
    }

    private static void PaintGlossyButton(object? sender, PaintEventArgs args)
    {
        if (sender is not Button button || button.Width < 1 || button.Height < 1) return;

        Color top = button.Enabled ? Color.FromArgb(93, 184, 255) : Color.FromArgb(142, 154, 171);
        Color bottom = button.Enabled ? AccentDeep : Color.FromArgb(87, 99, 116);
        using LinearGradientBrush fill = new(button.ClientRectangle, top, bottom, LinearGradientMode.Vertical);
        args.Graphics.FillRectangle(fill, button.ClientRectangle);

        Rectangle shineArea = new(0, 0, button.Width, Math.Max(1, button.Height / 2));
        using LinearGradientBrush shine = new(shineArea, Color.FromArgb(105, Color.White), Color.FromArgb(12, Color.White), LinearGradientMode.Vertical);
        args.Graphics.FillRectangle(shine, shineArea);

        using Pen edge = new(Color.FromArgb(120, Color.White));
        args.Graphics.DrawLine(edge, 5, 1, Math.Max(5, button.Width - 6), 1);
        TextRenderer.DrawText(args.Graphics, button.Text, button.Font, button.ClientRectangle, button.ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void StylePanel(Panel panel, Color color)
    {
        panel.BackColor = color;
        panel.Paint += (_, args) => PaintPanelGloss(panel, args, color);
    }

    private static void PaintPanelGloss(Panel panel, PaintEventArgs args, Color color)
    {
        if (panel.Width < 1 || panel.Height < 1) return;
        Color top = Blend(color, Color.White, 0.14F);
        Color bottom = Blend(color, Color.Black, 0.10F);
        using LinearGradientBrush fill = new(panel.ClientRectangle, top, bottom, LinearGradientMode.Vertical);
        args.Graphics.FillRectangle(fill, panel.ClientRectangle);
        using Pen highlight = new(Color.FromArgb(48, Color.White));
        args.Graphics.DrawLine(highlight, 0, 0, Math.Max(0, panel.Width - 1), 0);
    }

    private static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.GridColor = Color.FromArgb(213, 222, 235);
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Navy,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
            SelectionBackColor = Navy,
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };
        grid.ColumnHeadersHeight = 38;
        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Field,
            ForeColor = Ink,
            SelectionBackColor = Color.FromArgb(190, 220, 255),
            SelectionForeColor = Ink,
            Padding = new Padding(4, 2, 4, 2)
        };
        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(235, 243, 252)
        };
        grid.RowTemplate.Height = 31;
    }

    private static Color PanelColor(Panel panel)
    {
        string name = panel.Name.ToLowerInvariant();
        if (name.Contains("header")) return Navy;
        if (name.Contains("root")) return Ink;
        if (name.Contains("content")) return Slate;
        return panel.Parent is Form ? Surface : Slate;
    }

    private static bool IsOnLightSurface(Label label) =>
        label.Parent is TableLayoutPanel || label.Parent?.BackColor.GetBrightness() > 0.68F;

    private static Color Blend(Color first, Color second, float amount) => Color.FromArgb(
        first.A,
        (int)(first.R + (second.R - first.R) * amount),
        (int)(first.G + (second.G - first.G) * amount),
        (int)(first.B + (second.B - first.B) * amount));

    private static void Round(Control control, int radius)
    {
        if (control.Width < 1 || control.Height < 1) return;
        using GraphicsPath path = new();
        int diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height));
        Rectangle arc = new(0, 0, diameter, diameter);
        path.AddArc(arc, 180, 90);
        arc.X = control.Width - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = control.Height - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = 0;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        control.Region?.Dispose();
        control.Region = new Region(path);
    }
}
