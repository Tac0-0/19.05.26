using Doner.Controller;

namespace _19._05._26
{
    public partial class CategoriesForm : Form
    {
        private readonly CategoriesController _controller = new();
        public CategoriesForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllCategories);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllCategories);
        }
    }
}
