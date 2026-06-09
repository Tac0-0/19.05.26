namespace _19._05._26
{
    partial class DeliveriesForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel; private Panel headerPanel; private Panel contentPanel; private Label titleLabel;
        protected override void Dispose(bool disposing){ if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            rootPanel = new Panel();
            contentPanel = new Panel();
            headerPanel = new Panel();
            titleLabel = new Label();
            rootPanel.SuspendLayout();
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
            rootPanel.Size = new Size(900, 560);
            rootPanel.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(74, 74, 74);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(16, 72);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(868, 472);
            contentPanel.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(45, 45, 45);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(16, 16);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(868, 56);
            headerPanel.TabIndex = 1;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            titleLabel.ForeColor = Color.WhiteSmoke;
            titleLabel.Location = new Point(16, 17);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(147, 28);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "DeliveriesForm";
            // 
            // DeliveriesForm
            // 
            BackColor = Color.FromArgb(34, 34, 34);
            ClientSize = new Size(900, 560);
            Controls.Add(rootPanel);
            Name = "DeliveriesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DeliveriesForm";
            rootPanel.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
