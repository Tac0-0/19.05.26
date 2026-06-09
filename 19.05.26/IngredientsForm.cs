using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class IngredientsForm : Form
    {
        private readonly IngredientsController _controller = new();

        public IngredientsForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.InventoryManagement)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllIngredients);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Ingredients? entity = await UiGridHelper.PromptForEntity<Ingredients>("Add ingredient", "IngredientId");
                if (entity is null) return;
                await _controller.AddIngredient(entity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                Ingredients? selected = UiGridHelper.GetSelected<Ingredients>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit ingredient", "IngredientId")) return;
                await _controller.UpdateIngredient(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                Ingredients? selected = UiGridHelper.GetSelected<Ingredients>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("ingredient")) return;
                await _controller.DeleteIngredient(selected.IngredientId);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Increase stock", async () => await ChangeStock(grid, true));
            UiGridHelper.AddButton(toolbar, "Decrease stock", async () => await ChangeStock(grid, false));
            Load += async (_, _) => await reload();
        }

        private async Task ChangeStock(DataGridView grid, bool increase)
        {
            Ingredients? selected = UiGridHelper.GetSelected<Ingredients>(grid);
            if (selected is null) return;
            StockChange value = new();
            if (!UiGridHelper.EditEntity(value, increase ? "Increase stock" : "Decrease stock")) return;
            if (value.Amount <= 0) throw new InvalidOperationException("Amount must be greater than zero.");
            if (increase) await _controller.IncreaseStock(selected.IngredientId, value.Amount);
            else await _controller.DecreaseStock(selected.IngredientId, value.Amount);
            await UiGridHelper.BindAsync(grid, _controller.GetAllIngredients);
        }

        private sealed class StockChange
        {
            public decimal Amount { get; set; }
        }
    }
}
