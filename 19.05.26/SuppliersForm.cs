using Doner.Controller;

namespace _19._05._26
{
    public partial class SuppliersForm : Form
    {
        private readonly SuppliersController _controller = new();
        public SuppliersForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllSuppliers);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllSuppliers);
        }
    }
}
