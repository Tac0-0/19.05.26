namespace _19._05._26
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            // TODO: replace with real authentication/role lookup
            var username = usernameTextBox.Text.Trim();
            var role = username.Equals("admin", StringComparison.OrdinalIgnoreCase)
                ? "Admin"
                : "Customer";

            Form nextForm = role == "Customer"
                ? new CustomerMainForm()
                : new StaffMainForm(role);

            nextForm.FormClosed += (_, _) => Close();
            Hide();
            nextForm.Show();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            new RegisterForm().ShowDialog(this);
        }

        private void importJsonButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("JSON import will be added in a later step.", "Not implemented");
        }
    }
}
