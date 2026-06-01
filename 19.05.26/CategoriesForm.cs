using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class CategoriesForm : Form
    {
        private readonly CategoriesController _controller = new();

        public CategoriesForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllCategories);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Categories? entity = await UiGridHelper.PromptForEntity<Categories>("Add category", "CategoryId");
                if (entity is null) return;
                await _controller.AddCategory(entity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                Categories? selected = UiGridHelper.GetSelected<Categories>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit category", "CategoryId")) return;
                await _controller.UpdateCategory(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                Categories? selected = UiGridHelper.GetSelected<Categories>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("category")) return;
                await _controller.DeleteCategory(selected.CategoryId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }
    }
}
