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
            SetBusy(true);
            try
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

                Users user = CreateUser(roleComboBox.SelectedItem?.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Visible)
                {
                    SetBusy(false);
                }
            }
        }

        private void SetBusy(bool busy)
        {
            createAccountButton.Enabled = !busy;
            roleComboBox.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private static Users CreateUser(string? selectedRole)
        {
            return selectedRole switch
            {
                "Customer" => new Customers { Role = UserRole.Customer },
                "Cashier" => CreateEmployee(EmployeePosition.Cashier),
                "Cook" => CreateEmployee(EmployeePosition.Cook),
                "DeliveryWorker" => CreateEmployee(EmployeePosition.DeliveryWorker),
                "Manager" => CreateEmployee(EmployeePosition.Manager),
                "Admin" => new Admins { Role = UserRole.Admin },
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
