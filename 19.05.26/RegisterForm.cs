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
            roleComboBox.Items.Clear();
            roleComboBox.Items.Add("Customer");
            roleComboBox.SelectedIndex = 0;
            roleComboBox.Enabled = false;
            roleComboBox.Visible = false;
            roleLabel.AutoSize = false;
            roleLabel.Text = "Public registration creates customer accounts only.";
            roleLabel.Location = new Point(16, 226);
            roleLabel.Size = new Size(356, 36);
            createAccountButton.Location = new Point(16, 284);
        }

        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            var firstName = firstNameTextBox.Text.Trim();
            var lastName = lastNameTextBox.Text.Trim();
            var username = usernameTextBox.Text.Trim();
            var email = emailTextBox.Text.Trim();
            var password = passwordTextBox.Text;
            var phoneNumber = phoneTextBox.Text.Trim();

            if (new[] { firstName, lastName, username, email, password, phoneNumber }.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains('@'))
            {
                MessageBox.Show("Please enter a valid email.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Users user = CreateUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            user.PhoneNumber = phoneNumber;

            bool ok = await _authController.Register(user);
            MessageBox.Show(ok ? "Registration successful." : "Username or email already exists.", "Register");
            if (ok) Close();
        }

        private static Users CreateUser()
        {
            return new Customers
            {
                Role = UserRole.Customer
            };
        }
    }
}
