namespace _19._05._26
{
    public partial class StaffMainForm : Form
    {
        public StaffMainForm(string role)
        {
            InitializeComponent();
            roleLabel.Text = $"Role: {role}";
        }

        private void usersButton_Click(object sender, EventArgs e)
        {
            new UsersForm().ShowDialog(this);
        }
    }
}
