namespace _19._05._26
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel; private Panel headerPanel; private Panel contentPanel; private Label titleLabel;
        private TextBox usernameTextBox; private TextBox passwordTextBox; private Button loginButton; private Button registerButton; private Button importJsonButton;
        protected override void Dispose(bool disposing){ if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing);}
        private void InitializeComponent()
        {
            rootPanel = new Panel();
            contentPanel = new Panel();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            registerButton = new Button();
            importJsonButton = new Button();
            headerPanel = new Panel();
            titleLabel = new Label();
            rootPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // rootPanel
            // 
            rootPanel.BackColor = Color.FromArgb(58, 58, 58);
            rootPanel.Controls.Add(contentPanel);
            rootPanel.Controls.Add(headerPanel);
            rootPanel.Dock = DockStyle.Fill;
            rootPanel.Location = new Point(0, 0);
            rootPanel.Name = "rootPanel";
            rootPanel.Padding = new Padding(16);
            rootPanel.Size = new Size(380, 320);
            rootPanel.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(74, 74, 74);
            contentPanel.Controls.Add(usernameTextBox);
            contentPanel.Controls.Add(passwordTextBox);
            contentPanel.Controls.Add(loginButton);
            contentPanel.Controls.Add(registerButton);
            contentPanel.Controls.Add(importJsonButton);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(16, 72);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(16);
            contentPanel.Size = new Size(348, 232);
            contentPanel.TabIndex = 0;
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = Color.FromArgb(245, 245, 245);
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Location = new Point(16, 24);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.PlaceholderText = "Username/Email";
            usernameTextBox.Size = new Size(320, 27);
            usernameTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = Color.FromArgb(245, 245, 245);
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Location = new Point(16, 58);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.Size = new Size(320, 27);
            passwordTextBox.TabIndex = 1;
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.FromArgb(230, 230, 230);
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.Location = new Point(16, 100);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(320, 34);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += loginButton_Click;
            // 
            // registerButton
            // 
            registerButton.BackColor = Color.FromArgb(230, 230, 230);
            registerButton.FlatStyle = FlatStyle.Flat;
            registerButton.Location = new Point(16, 142);
            registerButton.Name = "registerButton";
            registerButton.Size = new Size(320, 34);
            registerButton.TabIndex = 3;
            registerButton.Text = "Register";
            registerButton.UseVisualStyleBackColor = false;
            registerButton.Click += registerButton_Click;
            // 
            // importJsonButton
            // 
            importJsonButton.BackColor = Color.FromArgb(230, 230, 230);
            importJsonButton.FlatStyle = FlatStyle.Flat;
            importJsonButton.Location = new Point(16, 184);
            importJsonButton.Name = "importJsonButton";
            importJsonButton.Size = new Size(320, 34);
            importJsonButton.TabIndex = 4;
            importJsonButton.Text = "Import JSON";
            importJsonButton.UseVisualStyleBackColor = false;
            importJsonButton.Click += importJsonButton_Click;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(45, 45, 45);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(16, 16);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(348, 56);
            headerPanel.TabIndex = 1;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            titleLabel.ForeColor = Color.WhiteSmoke;
            titleLabel.Location = new Point(16, 17);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(63, 28);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Login";
            // 
            // LoginForm
            // 
            BackColor = Color.FromArgb(34, 34, 34);
            ClientSize = new Size(380, 320);
            Controls.Add(rootPanel);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            rootPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
