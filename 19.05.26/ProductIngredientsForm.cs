using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class ProductIngredientsForm : Form
    {
        private readonly ProductIngredientsController _controller = new();
        private readonly int _productId;

        public ProductIngredientsForm(int productId = 0)
        {
            InitializeComponent();
            _productId = productId;
            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, () => _controller.GetRecipeByProduct(_productId));
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                RecipeIngredient input = new();
                if (!UiGridHelper.EditEntity(input, "Add recipe ingredient")) return;
                await _controller.AddIngredientToProduct(_productId, input.IngredientId, input.RequiredQuantity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit quantity", async () =>
            {
                ProductIngredients? selected = UiGridHelper.GetSelected<ProductIngredients>(grid);
                if (selected is null) return;
                RecipeQuantity input = new() { RequiredQuantity = selected.RequiredQuantity };
                if (!UiGridHelper.EditEntity(input, "Edit required quantity")) return;
                await _controller.UpdateRequiredQuantity(selected.ProductIngredientId, input.RequiredQuantity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                ProductIngredients? selected = UiGridHelper.GetSelected<ProductIngredients>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("recipe ingredient")) return;
                await _controller.RemoveIngredientFromProduct(selected.ProductIngredientId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }

        private sealed class RecipeIngredient { public int IngredientId { get; set; } public decimal RequiredQuantity { get; set; } }
        private sealed class RecipeQuantity { public decimal RequiredQuantity { get; set; } }
    }
}
