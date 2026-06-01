using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class ProductsForm : Form
    {
        private readonly ProductsController _controller = new();

        public ProductsForm()
        {
            InitializeComponent();
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllProducts);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Products? entity = await UiGridHelper.PromptForEntity<Products>("Add product", "ProductId");
                if (entity is null) return;
                await _controller.AddProduct(entity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                Products? selected = UiGridHelper.GetSelected<Products>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit product", "ProductId")) return;
                await _controller.UpdateProduct(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                Products? selected = UiGridHelper.GetSelected<Products>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("product")) return;
                await _controller.DeleteProduct(selected.ProductId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Toggle availability", async () =>
            {
                Products? selected = UiGridHelper.GetSelected<Products>(grid);
                if (selected is null) return;
                await _controller.SetAvailability(selected.ProductId, !selected.IsAvailable);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Recipe", () =>
            {
                Products? selected = UiGridHelper.GetSelected<Products>(grid);
                if (selected is not null) new ProductIngredientsForm(selected.ProductId).ShowDialog(this);
                return Task.CompletedTask;
            });
            Load += async (_, _) => await reload();
        }
    }
}
