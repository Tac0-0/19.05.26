namespace _19._05._26
{
    public partial class StaffMainForm : Form
    {
        public StaffMainForm(string role)
        {
            InitializeComponent();
            roleLabel.Text = $"Role: {role}";
            BuildMenu();
        }

        private void BuildMenu()
        {
            FlowLayoutPanel panel = new() { Dock = DockStyle.Bottom, Height = 220, Padding = new Padding(16) };
            panel.Controls.Add(MakeButton("Users", () => new UsersForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Products", () => new ProductsForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Orders", () => new OrdersForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Categories", () => new CategoriesForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Suppliers", () => new SuppliersForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Ingredients", () => new IngredientsForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Deliveries", () => new DeliveriesForm().ShowDialog(this)));
            contentPanel.Controls.Add(panel);
        }

        private static Button MakeButton(string text, Action onClick)
        {
            Button button = new() { Text = text, Width = 130, Height = 36, Margin = new Padding(8) };
            button.Click += (_, _) => onClick();
            return button;
        }
    }
}
