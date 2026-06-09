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
            SetBusy(true);
            try
            {
                var username = usernameTextBox.Text.Trim();
                var password = passwordTextBox.Text;
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username and password are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = await _authController.Login(username, password);
                if (user is null)
                {
                    MessageBox.Show("Invalid credentials or inactive account.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Form nextForm = user.Role == UserRole.Customer ? new CustomerMainForm() : new StaffMainForm(user);
                nextForm.FormClosed += (_, _) => Close();
                Hide();
                nextForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Visible)
                {
                    SetBusy(false);
                }
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            new RegisterForm().ShowDialog(this);
        }

        private void importJsonButton_Click(object sender, EventArgs e)
        {
            try
            {
                new JsonImportForm(allowDemoAccess: true).ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "JSON import failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetBusy(bool busy)
        {
            loginButton.Enabled = !busy;
            registerButton.Enabled = !busy;
            importJsonButton.Enabled = !busy;
            UseWaitCursor = busy;
        }
    }
}
