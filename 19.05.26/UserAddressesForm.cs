using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class UserAddressesForm : Form
    {
        private readonly UserAddressesController _controller = new();
        private readonly int _userId;

        public UserAddressesForm(int userId = 0)
        {
            InitializeComponent();
            _userId = userId;
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, () => _controller.GetAddressesByUser(_userId));
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                UserAddresses address = new() { UserId = _userId };
                if (!UiGridHelper.EditEntity(address, "Add address", "UserAddressId", "UserId")) return;
                await _controller.AddAddress(address);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                UserAddresses? selected = UiGridHelper.GetSelected<UserAddresses>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit address", "UserAddressId", "UserId")) return;
                await _controller.UpdateAddress(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                UserAddresses? selected = UiGridHelper.GetSelected<UserAddresses>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("address")) return;
                await _controller.DeleteAddress(selected.UserAddressId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Set default", async () =>
            {
                UserAddresses? selected = UiGridHelper.GetSelected<UserAddresses>(grid);
                if (selected is null) return;
                await _controller.SetDefaultAddress(selected.UserAddressId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }
    }
}
