using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

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
            var phoneNumber = phoneTextBox.Text.Trim();
            var role = (UserRole)roleComboBox.SelectedIndex;

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

            bool ok;
            if (role == UserRole.Customer)
            {
                ok = await _authController.Register(new Customers
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = username,
                    Email = email,
                    Password = password,
                    PhoneNumber = phoneNumber,
                    Role = role
                });
            }
            else if(role == UserRole.Employee)
            {
                ok = await _authController.Register(new Employees
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = username,
                    Email = email,
                    Password = password,
                    PhoneNumber = phoneNumber,
                    Role = role
                });
            }
            else
            {
                ok = await _authController.Register(new Admins
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = username,
                    Email = email,
                    Password = password,
                    PhoneNumber = phoneNumber,
                    Role = role
                });
            }

            MessageBox.Show(ok ? "Registration successful." : "Username or email already exists.", "Register");
            if (ok) Close();
        }
    }
}
