namespace _19._05._26
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private TextBox emailTextBox;
        private TextBox firstNameTextBox;
        private TextBox lastNameTextBox;
        private TextBox phoneTextBox;
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
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            emailTextBox = new TextBox();
            firstNameTextBox = new TextBox();
            lastNameTextBox = new TextBox();
            phoneTextBox = new TextBox();
            createAccountButton = new Button();
            SuspendLayout();
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(24, 24);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.PlaceholderText = "Username";
            usernameTextBox.Size = new Size(272, 23);
            usernameTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(24, 53);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.Size = new Size(272, 23);
            passwordTextBox.TabIndex = 1;
            // 
            // emailTextBox
            // 
            emailTextBox.Location = new Point(24, 82);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.PlaceholderText = "Email";
            emailTextBox.Size = new Size(272, 23);
            emailTextBox.TabIndex = 2;
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.Location = new Point(24, 111);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.PlaceholderText = "First name";
            firstNameTextBox.Size = new Size(272, 23);
            firstNameTextBox.TabIndex = 3;
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.Location = new Point(24, 140);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.PlaceholderText = "Last name";
            lastNameTextBox.Size = new Size(272, 23);
            lastNameTextBox.TabIndex = 4;
            // 
            // phoneTextBox
            // 
            phoneTextBox.Location = new Point(24, 169);
            phoneTextBox.Name = "phoneTextBox";
            phoneTextBox.PlaceholderText = "Phone number";
            phoneTextBox.Size = new Size(272, 23);
            phoneTextBox.TabIndex = 5;
            // 
            // createAccountButton
            // 
            createAccountButton.Location = new Point(24, 208);
            createAccountButton.Name = "createAccountButton";
            createAccountButton.Size = new Size(272, 30);
            createAccountButton.TabIndex = 6;
            createAccountButton.Text = "Create account";
            createAccountButton.UseVisualStyleBackColor = true;
            createAccountButton.Click += createAccountButton_Click;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 261);
            Controls.Add(createAccountButton);
            Controls.Add(phoneTextBox);
            Controls.Add(lastNameTextBox);
            Controls.Add(firstNameTextBox);
            Controls.Add(emailTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Register";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
