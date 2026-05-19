using Doner.Controller;

namespace _19._05._26
{
    public partial class UsersForm : Form
    {
        private readonly UsersController _controller = new();
        public UsersForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllUsers);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllUsers);
        }
    }
}
