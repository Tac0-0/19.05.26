namespace _19._05._26
{
    public partial class ReportsForm : Form
    {
        public ReportsForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.Reports);
        }
    }
}
