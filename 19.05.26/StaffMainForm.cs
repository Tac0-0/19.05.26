using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class StaffMainForm : Form
    {
        private readonly StaffAccess _access;

        public StaffMainForm(Users user)
        {
            InitializeComponent();
            _access = new StaffAccess(user);
            roleLabel.Text = $"Role: {_access.DisplayRole}";
            BuildMenu();
        }

        private void BuildMenu()
        {
            FlowLayoutPanel panel = new() { Dock = DockStyle.Bottom, Height = 220, Padding = new Padding(16) };
            AddButton(panel, StaffFeature.UserAdministration, "Users", () => new UsersForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.ProductCatalog, "Products", () => new ProductsForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.OrderDesk, "Orders", () => new OrdersForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.KitchenOrders, "Kitchen Orders", () => new KitchenOrdersForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.CategoryManagement, "Categories", () => new CategoriesForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.SupplierManagement, "Suppliers", () => new SuppliersForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.InventoryManagement, "Ingredients", () => new IngredientsForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.DeliveryManagement, "Deliveries", () => new DeliveriesForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.AssignedDeliveries, "My Deliveries", () => new DeliveryWorkerForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.Reports, "Reports", () => new ReportsForm(_access).ShowDialog(this));
            AddButton(panel, StaffFeature.JsonDataTransfer, "JSON Data", () => new JsonImportForm(_access).ShowDialog(this));
            if (panel.Controls.Count == 0)
            {
                panel.Controls.Add(new Label
                {
                    Text = "No dashboard actions are assigned to this employee position.",
                    AutoSize = true,
                    ForeColor = Color.Gainsboro,
                    Margin = new Padding(8, 14, 8, 8)
                });
            }
            contentPanel.Controls.Add(panel);
        }

        private void AddButton(FlowLayoutPanel panel, StaffFeature feature, string text, Action onClick)
        {
            if (_access.CanOpen(feature))
            {
                panel.Controls.Add(MakeButton(text, onClick));
            }
        }

        private static Button MakeButton(string text, Action onClick)
        {
            Button button = new() { Text = text, Width = 130, Height = 36, Margin = new Padding(8) };
            button.Click += (_, _) => onClick();
            return button;
        }
    }
}
