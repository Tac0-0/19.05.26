namespace _19._05._26
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            roleComboBox.SelectedIndex = 0;
        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            var selectedRole = roleComboBox.SelectedItem?.ToString() ?? "Customer";
            MessageBox.Show($"Registration stub created (Role = {selectedRole}).", "Register");
            Close();
        }
    }
}
