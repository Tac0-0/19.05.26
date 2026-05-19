namespace _19._05._26
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel; private Panel headerPanel; private Panel contentPanel; private Label titleLabel;
        private TextBox usernameTextBox; private TextBox passwordTextBox; private Button loginButton; private Button registerButton; private Button importJsonButton;
        protected override void Dispose(bool disposing){ if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing);} 
        private void InitializeComponent(){ rootPanel=new Panel();headerPanel=new Panel();contentPanel=new Panel();titleLabel=new Label();usernameTextBox=new TextBox();passwordTextBox=new TextBox();loginButton=new Button();registerButton=new Button();importJsonButton=new Button();
            rootPanel.SuspendLayout();headerPanel.SuspendLayout();contentPanel.SuspendLayout();SuspendLayout();
            rootPanel.BackColor=Color.FromArgb(58,58,58); rootPanel.Dock=DockStyle.Fill; rootPanel.Padding=new Padding(16);
            headerPanel.BackColor=Color.FromArgb(45,45,45); headerPanel.Dock=DockStyle.Top; headerPanel.Height=56;
            titleLabel.Text="Login"; titleLabel.ForeColor=Color.WhiteSmoke; titleLabel.Location=new Point(16,17); titleLabel.AutoSize=true; titleLabel.Font=new Font("Segoe UI Semibold",12F,FontStyle.Bold);
            contentPanel.BackColor=Color.FromArgb(74,74,74); contentPanel.Dock=DockStyle.Fill; contentPanel.Padding=new Padding(16);
            usernameTextBox.Location=new Point(16,24); usernameTextBox.Size=new Size(320,23); usernameTextBox.PlaceholderText="Username"; usernameTextBox.BackColor=Color.FromArgb(245,245,245); usernameTextBox.BorderStyle=BorderStyle.FixedSingle;
            passwordTextBox.Location=new Point(16,58); passwordTextBox.Size=new Size(320,23); passwordTextBox.PlaceholderText="Password"; passwordTextBox.PasswordChar='*'; passwordTextBox.BackColor=Color.FromArgb(245,245,245); passwordTextBox.BorderStyle=BorderStyle.FixedSingle;
            loginButton.Location=new Point(16,100); loginButton.Size=new Size(320,34); loginButton.Text="Login"; loginButton.BackColor=Color.FromArgb(230,230,230); loginButton.FlatStyle=FlatStyle.Flat; loginButton.Click += loginButton_Click;
            registerButton.Location=new Point(16,142); registerButton.Size=new Size(320,34); registerButton.Text="Register"; registerButton.BackColor=Color.FromArgb(230,230,230); registerButton.FlatStyle=FlatStyle.Flat; registerButton.Click += registerButton_Click;
            importJsonButton.Location=new Point(16,184); importJsonButton.Size=new Size(320,34); importJsonButton.Text="Import JSON"; importJsonButton.BackColor=Color.FromArgb(230,230,230); importJsonButton.FlatStyle=FlatStyle.Flat; importJsonButton.Click += importJsonButton_Click;
            contentPanel.Controls.Add(usernameTextBox);contentPanel.Controls.Add(passwordTextBox);contentPanel.Controls.Add(loginButton);contentPanel.Controls.Add(registerButton);contentPanel.Controls.Add(importJsonButton);
            headerPanel.Controls.Add(titleLabel); rootPanel.Controls.Add(contentPanel); rootPanel.Controls.Add(headerPanel); Controls.Add(rootPanel);
            BackColor=Color.FromArgb(34,34,34); ClientSize=new Size(380,320); Name="LoginForm"; Text="Login"; StartPosition=FormStartPosition.CenterScreen;
            rootPanel.ResumeLayout(false);headerPanel.ResumeLayout(false);headerPanel.PerformLayout();contentPanel.ResumeLayout(false);contentPanel.PerformLayout();ResumeLayout(false);} 
    }
}
