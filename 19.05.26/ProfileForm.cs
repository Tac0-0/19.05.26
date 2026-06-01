using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class ProfileForm : Form
    {
        private readonly AuthController _authController = new();
        private readonly UsersController _usersController = new();

        public ProfileForm()
        {
            InitializeComponent();
            FlowLayoutPanel toolbar = new() { Dock = DockStyle.Top, Height = 44, Padding = new Padding(4) };
            UiGridHelper.AddButton(toolbar, "Edit profile", async () =>
            {
                Users? user = await _authController.GetLoggedUser();
                if (user is null || !UiGridHelper.EditEntity(user, "Edit profile", "UserId", "Role", "IsActive")) return;
                await _usersController.UpdateUser(user);
            });
            UiGridHelper.AddButton(toolbar, "Addresses", async () =>
            {
                Users? user = await _authController.GetLoggedUser();
                if (user is not null) new UserAddressesForm(user.UserId).ShowDialog(this);
            });
            contentPanel.Controls.Add(toolbar);
        }
    }
}
