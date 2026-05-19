using Doner.Controller;

namespace _19._05._26
{
    public partial class ProductsForm : Form
    {
        private readonly ProductsController _controller = new();
        public ProductsForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllProducts);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllProducts);
        }
    }
}
