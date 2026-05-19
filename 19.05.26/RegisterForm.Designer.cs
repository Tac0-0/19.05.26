namespace _19._05._26
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel;
        private Panel headerPanel;
        private Label titleLabel;
        private Panel contentPanel;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private TextBox emailTextBox;
        private TextBox firstNameTextBox;
        private TextBox lastNameTextBox;
        private TextBox phoneTextBox;
        private ComboBox roleComboBox;
        private Label roleLabel;
        private Button createAccountButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootPanel = new Panel();
            contentPanel = new Panel();
            roleLabel = new Label();
            roleComboBox = new ComboBox();
            createAccountButton = new Button();
            phoneTextBox = new TextBox();
            lastNameTextBox = new TextBox();
            firstNameTextBox = new TextBox();
            emailTextBox = new TextBox();
            passwordTextBox = new TextBox();
            usernameTextBox = new TextBox();
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
            rootPanel.Size = new Size(420, 470);
            rootPanel.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(74, 74, 74);
            contentPanel.Controls.Add(roleLabel);
            contentPanel.Controls.Add(roleComboBox);
            contentPanel.Controls.Add(createAccountButton);
            contentPanel.Controls.Add(phoneTextBox);
            contentPanel.Controls.Add(lastNameTextBox);
            contentPanel.Controls.Add(firstNameTextBox);
            contentPanel.Controls.Add(emailTextBox);
            contentPanel.Controls.Add(passwordTextBox);
            contentPanel.Controls.Add(usernameTextBox);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(16, 72);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(16);
            contentPanel.Size = new Size(388, 382);
            contentPanel.TabIndex = 1;
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.ForeColor = Color.Gainsboro;
            roleLabel.Location = new Point(16, 226);
            roleLabel.Name = "roleLabel";
            roleLabel.Size = new Size(30, 15);
            roleLabel.TabIndex = 8;
            roleLabel.Text = "Role";
            // 
            // roleComboBox
            // 
            roleComboBox.BackColor = Color.WhiteSmoke;
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleComboBox.FlatStyle = FlatStyle.Flat;
            roleComboBox.ForeColor = Color.Black;
            roleComboBox.FormattingEnabled = true;
            roleComboBox.Items.AddRange(new object[] { "Customer", "Cashier", "Cook", "DeliveryWorker", "Manager", "Admin" });
            roleComboBox.Location = new Point(16, 244);
            roleComboBox.Name = "roleComboBox";
            roleComboBox.Size = new Size(356, 23);
            roleComboBox.TabIndex = 7;
            // 
            // createAccountButton
            // 
            createAccountButton.BackColor = Color.FromArgb(230, 230, 230);
            createAccountButton.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            createAccountButton.FlatStyle = FlatStyle.Flat;
            createAccountButton.ForeColor = Color.Black;
            createAccountButton.Location = new Point(16, 301);
            createAccountButton.Name = "createAccountButton";
            createAccountButton.Size = new Size(356, 40);
            createAccountButton.TabIndex = 8;
            createAccountButton.Text = "Create account";
            createAccountButton.UseVisualStyleBackColor = false;
            createAccountButton.Click += createAccountButton_Click;
            // 
            // phoneTextBox
            // 
            phoneTextBox.BackColor = Color.FromArgb(245, 245, 245);
            phoneTextBox.BorderStyle = BorderStyle.FixedSingle;
            phoneTextBox.Location = new Point(16, 189);
            phoneTextBox.Name = "phoneTextBox";
            phoneTextBox.PlaceholderText = "Phone number";
            phoneTextBox.Size = new Size(356, 23);
            phoneTextBox.TabIndex = 5;
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.BackColor = Color.FromArgb(245, 245, 245);
            lastNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            lastNameTextBox.Location = new Point(16, 155);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.PlaceholderText = "Last name";
            lastNameTextBox.Size = new Size(356, 23);
            lastNameTextBox.TabIndex = 4;
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.BackColor = Color.FromArgb(245, 245, 245);
            firstNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            firstNameTextBox.Location = new Point(16, 121);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.PlaceholderText = "First name";
            firstNameTextBox.Size = new Size(356, 23);
            firstNameTextBox.TabIndex = 3;
            // 
            // emailTextBox
            // 
            emailTextBox.BackColor = Color.FromArgb(245, 245, 245);
            emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            emailTextBox.Location = new Point(16, 87);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.PlaceholderText = "Email";
            emailTextBox.Size = new Size(356, 23);
            emailTextBox.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = Color.FromArgb(245, 245, 245);
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Location = new Point(16, 53);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.Size = new Size(356, 23);
            passwordTextBox.TabIndex = 1;
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = Color.FromArgb(245, 245, 245);
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Location = new Point(16, 19);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.PlaceholderText = "Username";
            usernameTextBox.Size = new Size(356, 23);
            usernameTextBox.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(45, 45, 45);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(16, 16);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(388, 56);
            headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.ForeColor = Color.WhiteSmoke;
            titleLabel.Location = new Point(16, 17);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(139, 21);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Create an account";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 34, 34);
            ClientSize = new Size(420, 470);
            Controls.Add(rootPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Register";
            rootPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
