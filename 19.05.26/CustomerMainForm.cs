namespace _19._05._26
{
    public partial class CustomerMainForm : Form
    {
        public CustomerMainForm()
        {
            InitializeComponent();
            BuildMenu();
        }

        private void BuildMenu()
        {
            FlowLayoutPanel panel = new() { Dock = DockStyle.Top, Height = 140, Padding = new Padding(16) };
            panel.Controls.Add(MakeButton("Browse Menu", () => new CustomerMenuForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("My Orders", () => new MyOrdersForm().ShowDialog(this)));
            panel.Controls.Add(MakeButton("Profile", () => new ProfileForm().ShowDialog(this)));
            contentPanel.Controls.Add(panel);
        }

        private static Button MakeButton(string text, Action onClick)
        {
            Button button = new() { Text = text, Width = 150, Height = 40, Margin = new Padding(10) };
            button.Click += (_, _) => onClick();
            return button;
        }
    }
}
