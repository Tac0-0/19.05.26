using Doner.Controller;

namespace _19._05._26
{
    public partial class OrdersForm : Form
    {
        private readonly OrdersController _controller = new();
        public OrdersForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllOrders);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllOrders);
        }
    }
}
