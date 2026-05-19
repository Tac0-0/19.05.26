using Doner.Controller;

namespace _19._05._26
{
    public partial class IngredientsForm : Form
    {
        private readonly IngredientsController _controller = new();
        public IngredientsForm()
        {
            InitializeComponent();
            var (grid, reload) = UiGridHelper.BuildGridUi(contentPanel);
            reload.Click += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllIngredients);
            Load += async (_, _) => await UiGridHelper.BindAsync(grid, _controller.GetAllIngredients);
        }
    }
}
