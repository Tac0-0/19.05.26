namespace _19._05._26
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView usersGrid;

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
            usersGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)usersGrid).BeginInit();
            SuspendLayout();
            // 
            // usersGrid
            // 
            usersGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usersGrid.Dock = DockStyle.Fill;
            usersGrid.Location = new Point(0, 0);
            usersGrid.Name = "usersGrid";
            usersGrid.Size = new Size(720, 420);
            usersGrid.TabIndex = 0;
            // 
            // UsersForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 420);
            Controls.Add(usersGrid);
            Name = "UsersForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Users";
            ((System.ComponentModel.ISupportInitialize)usersGrid).EndInit();
            ResumeLayout(false);
        }
    }
}
