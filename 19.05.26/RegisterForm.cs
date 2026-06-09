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
            roleComboBox.Items.AddRange(new object[] { "Customer", "Cashier", "Cook", "DeliveryWorker", "Manager", "Admin" });
            roleComboBox.SelectedIndex = 0;
            roleComboBox.Enabled = true;
            roleComboBox.Visible = true;
            roleLabel.AutoSize = true;
            roleLabel.Text = "Role";
            roleLabel.Location = new Point(16, 226);
            createAccountButton.Location = new Point(16, 301);
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

            Users user = CreateUser(roleComboBox.SelectedIndex);
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

        private static Users CreateUser(int selectedRole)
        {
            return selectedRole switch
            {
                0 => new Customers { Role = UserRole.Customer },
                1 => CreateEmployee(EmployeePosition.Cashier),
                2 => CreateEmployee(EmployeePosition.Cook),
                3 => CreateEmployee(EmployeePosition.DeliveryWorker),
                4 => CreateEmployee(EmployeePosition.Manager),
                5 => new Admins { Role = UserRole.Admin },
                _ => throw new InvalidOperationException("Select a valid role.")
            };
        }

        private static Employees CreateEmployee(EmployeePosition position)
        {
            return new Employees
            {
                Role = UserRole.Employee,
                EmployeePosition = position,
                HireDate = DateTime.Now
            };
        }
    }
}
