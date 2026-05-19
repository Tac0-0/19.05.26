using Doner.Controller;

namespace _19._05._26
{
    public partial class DeliveriesForm : Form
    {
        private readonly DeliveriesController _controller = new();
        public DeliveriesForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllDeliveries);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllDeliveries);
        }
    }
}
