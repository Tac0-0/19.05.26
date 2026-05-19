using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class RegisterForm : Form
    {
        private readonly AuthController _authController = new();

        public RegisterForm()
        {
            InitializeComponent();
            roleComboBox.SelectedIndex = 0;
        }

        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            var firstName = firstNameTextBox.Text.Trim();
            var lastName = lastNameTextBox.Text.Trim();
            var username = usernameTextBox.Text.Trim();
            var email = emailTextBox.Text.Trim();
            var password = passwordTextBox.Text;

            if (new[] { firstName, lastName, username, email, password }.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains('@'))
            {
                MessageBox.Show("Please enter a valid email.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ok = await _authController.Register(new Users
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                Email = email,
                Password = password
            });

            MessageBox.Show(ok ? "Registration successful." : "Username or email already exists.", "Register");
            if (ok) Close();
        }
    }
}
