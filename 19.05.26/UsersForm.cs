using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class UsersForm : Form
    {
        private readonly UsersController _controller = new();

        public UsersForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllUsers);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                NewUserRole input = new();
                if (!UiGridHelper.EditEntity(input, "Choose user role")) return;
                Users user = input.Role switch
                {
                    UserRole.Admin => new Admins(),
                    UserRole.Employee => new Employees { HireDate = DateTime.Now },
                    _ => new Customers()
                };
                user.Role = input.Role;
                user.IsActive = true;
                if (!UiGridHelper.EditEntity(user, "Add user", "UserId", "Role")) return;
                await _controller.AddUser(user);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                Users? selected = UiGridHelper.GetSelected<Users>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit user", "UserId")) return;
                await _controller.UpdateUser(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                Users? selected = UiGridHelper.GetSelected<Users>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("user")) return;
                await _controller.DeleteUser(selected.UserId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Toggle active", async () =>
            {
                Users? selected = UiGridHelper.GetSelected<Users>(grid);
                if (selected is null) return;
                await _controller.SetActive(selected.UserId, !selected.IsActive);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Addresses", () =>
            {
                Users? selected = UiGridHelper.GetSelected<Users>(grid);
                if (selected is not null) new UserAddressesForm(selected.UserId).ShowDialog(this);
                return Task.CompletedTask;
            });
            Load += async (_, _) => await reload();
        }

        private sealed class NewUserRole { public UserRole Role { get; set; } }
    }
}
