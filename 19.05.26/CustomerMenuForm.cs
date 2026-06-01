using Doner.Controller;

namespace _19._05._26
{
    public partial class CustomerMenuForm : Form
    {
        private readonly ProductsController _controller = new();

        public CustomerMenuForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAvailableProducts);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            Load += async (_, _) => await reload();
        }
    }
}
