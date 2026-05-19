namespace _19._05._26
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            // TODO: save user with Role = Customer
            MessageBox.Show("Registration stub created (Role = Customer).", "Register");
            Close();
        }
    }
}
