using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class MyOrdersForm : Form
    {
        private readonly AuthController _authController = new();
        private readonly OrdersController _ordersController = new();

        public MyOrdersForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload()
            {
                Users? user = await _authController.GetLoggedUser();
                await UiGridHelper.BindAsync(grid, () => user is null ? Task.FromResult(new List<Orders>()) : _ordersController.GetOrdersByUser(user.UserId));
            }
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Details", () =>
            {
                Orders? selected = UiGridHelper.GetSelected<Orders>(grid);
                if (selected is not null) new CustomerOrderForm(selected.OrderId).ShowDialog(this);
                return Task.CompletedTask;
            });
            Load += async (_, _) => await reload();
        }
    }
}
