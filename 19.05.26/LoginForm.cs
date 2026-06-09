using Doner.Controller;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class LoginForm : Form
    {
        private readonly AuthController _authController = new();

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            var username = usernameTextBox.Text.Trim();
            var password = passwordTextBox.Text;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username/Email and password are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = await _authController.Login(username, password);
            if (user is null)
            {
                MessageBox.Show("Invalid credentials or inactive account.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form nextForm = user.Role == UserRole.Customer ? new CustomerMainForm() : new StaffMainForm(user.Role.ToString());
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
            new JsonImportForm().ShowDialog(this);
        }
    }
}
