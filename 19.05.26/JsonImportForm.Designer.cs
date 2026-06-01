namespace _19._05._26
{
    partial class JsonImportForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label titleLabel;
        private Label descriptionLabel;
        private Label fileLabel;
        private TextBox pathTextBox;
        private Button browseButton;
        private Button importButton;
        private Button exportButton;
        private Label statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootPanel = new Panel();
            headerPanel = new Panel();
            contentPanel = new Panel();
            titleLabel = new Label();
            descriptionLabel = new Label();
            fileLabel = new Label();
            pathTextBox = new TextBox();
            browseButton = new Button();
            importButton = new Button();
            exportButton = new Button();
            statusLabel = new Label();
            rootPanel.SuspendLayout();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            //
            // rootPanel
            //
            rootPanel.BackColor = Color.FromArgb(58, 58, 58);
            rootPanel.Controls.Add(contentPanel);
            rootPanel.Controls.Add(headerPanel);
            rootPanel.Dock = DockStyle.Fill;
            rootPanel.Padding = new Padding(16);
            //
            // headerPanel
            //
            headerPanel.BackColor = Color.FromArgb(45, 45, 45);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 56;
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.ForeColor = Color.WhiteSmoke;
            titleLabel.Location = new Point(16, 17);
            titleLabel.Text = "Import or export JSON data";
            //
            // contentPanel
            //
            contentPanel.BackColor = Color.FromArgb(74, 74, 74);
            contentPanel.Controls.Add(descriptionLabel);
            contentPanel.Controls.Add(fileLabel);
            contentPanel.Controls.Add(pathTextBox);
            contentPanel.Controls.Add(browseButton);
            contentPanel.Controls.Add(importButton);
            contentPanel.Controls.Add(exportButton);
            contentPanel.Controls.Add(statusLabel);
            contentPanel.Dock = DockStyle.Fill;
            //
            // descriptionLabel
            //
            descriptionLabel.ForeColor = Color.Gainsboro;
            descriptionLabel.Location = new Point(20, 22);
            descriptionLabel.Size = new Size(574, 48);
            descriptionLabel.Text = "Choose a JSON file to import a complete data set, or export the current database to a JSON backup file.";
            //
            // fileLabel
            //
            fileLabel.AutoSize = true;
            fileLabel.ForeColor = Color.Gainsboro;
            fileLabel.Location = new Point(20, 84);
            fileLabel.Text = "JSON file";
            //
            // pathTextBox
            //
            pathTextBox.BackColor = Color.WhiteSmoke;
            pathTextBox.BorderStyle = BorderStyle.FixedSingle;
            pathTextBox.Location = new Point(20, 108);
            pathTextBox.Size = new Size(470, 27);
            pathTextBox.TabIndex = 0;
            //
            // browseButton
            //
            browseButton.BackColor = Color.FromArgb(98, 98, 98);
            browseButton.FlatStyle = FlatStyle.Flat;
            browseButton.ForeColor = Color.WhiteSmoke;
            browseButton.Location = new Point(500, 106);
            browseButton.Size = new Size(94, 31);
            browseButton.TabIndex = 1;
            browseButton.Text = "Browse...";
            browseButton.UseVisualStyleBackColor = false;
            browseButton.Click += browseButton_Click;
            //
            // importButton
            //
            importButton.BackColor = Color.FromArgb(98, 98, 98);
            importButton.FlatStyle = FlatStyle.Flat;
            importButton.ForeColor = Color.WhiteSmoke;
            importButton.Location = new Point(20, 164);
            importButton.Size = new Size(180, 36);
            importButton.TabIndex = 2;
            importButton.Text = "Import selected file";
            importButton.UseVisualStyleBackColor = false;
            importButton.Click += importButton_Click;
            //
            // exportButton
            //
            exportButton.BackColor = Color.FromArgb(98, 98, 98);
            exportButton.FlatStyle = FlatStyle.Flat;
            exportButton.ForeColor = Color.WhiteSmoke;
            exportButton.Location = new Point(212, 164);
            exportButton.Size = new Size(180, 36);
            exportButton.TabIndex = 3;
            exportButton.Text = "Export backup...";
            exportButton.UseVisualStyleBackColor = false;
            exportButton.Click += exportButton_Click;
            //
            // statusLabel
            //
            statusLabel.ForeColor = Color.Gainsboro;
            statusLabel.Location = new Point(20, 230);
            statusLabel.Size = new Size(574, 52);
            statusLabel.Text = "Ready.";
            //
            // JsonImportForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 34, 34);
            ClientSize = new Size(646, 340);
            Controls.Add(rootPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "JsonImportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "JSON data transfer";
            rootPanel.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
