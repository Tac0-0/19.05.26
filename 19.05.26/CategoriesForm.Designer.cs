namespace _19._05._26
{
    partial class CategoriesForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel rootPanel; private Panel headerPanel; private Panel contentPanel; private Label titleLabel;
        protected override void Dispose(bool disposing){ if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent(){
            rootPanel=new Panel();headerPanel=new Panel();contentPanel=new Panel();titleLabel=new Label();
            rootPanel.SuspendLayout();headerPanel.SuspendLayout();SuspendLayout();
            rootPanel.BackColor=Color.FromArgb(58,58,58);rootPanel.Dock=DockStyle.Fill;rootPanel.Padding=new Padding(16);
            headerPanel.BackColor=Color.FromArgb(45,45,45);headerPanel.Dock=DockStyle.Top;headerPanel.Size=new Size(0,56);
            titleLabel.AutoSize=true;titleLabel.ForeColor=Color.WhiteSmoke;titleLabel.Font=new Font("Segoe UI Semibold",12F,FontStyle.Bold,GraphicsUnit.Point);titleLabel.Location=new Point(16,17);titleLabel.Text="CategoriesForm";
            contentPanel.BackColor=Color.FromArgb(74,74,74);contentPanel.Dock=DockStyle.Fill;
            headerPanel.Controls.Add(titleLabel);rootPanel.Controls.Add(contentPanel);rootPanel.Controls.Add(headerPanel);Controls.Add(rootPanel);
            BackColor=Color.FromArgb(34,34,34);ClientSize=new Size(900,560);Name="CategoriesForm";StartPosition=FormStartPosition.CenterParent;Text="CategoriesForm";
            rootPanel.ResumeLayout(false);headerPanel.ResumeLayout(false);headerPanel.PerformLayout();ResumeLayout(false);
        }
    }
}
