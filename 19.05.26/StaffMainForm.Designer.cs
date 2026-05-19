namespace _19._05._26
{
    partial class StaffMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label roleLabel;
        private Button usersButton;

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
            roleLabel = new Label();
            usersButton = new Button();
            SuspendLayout();
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(25, 24);
            roleLabel.Name = "roleLabel";
            roleLabel.Size = new Size(52, 15);
            roleLabel.TabIndex = 0;
            roleLabel.Text = "Role: ---";
            // 
            // usersButton
            // 
            usersButton.Location = new Point(25, 56);
            usersButton.Name = "usersButton";
            usersButton.Size = new Size(180, 30);
            usersButton.TabIndex = 1;
            usersButton.Text = "Open Users";
            usersButton.UseVisualStyleBackColor = true;
            usersButton.Click += usersButton_Click;
            // 
            // StaffMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 360);
            Controls.Add(usersButton);
            Controls.Add(roleLabel);
            Name = "StaffMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Staff Dashboard";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
