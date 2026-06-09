using Doner.Controller;
using Doner.Data.Entities;

namespace _19._05._26
{
    public partial class SuppliersForm : Form
    {
        private readonly SuppliersController _controller = new();

        public SuppliersForm(StaffAccess? access = null)
        {
            InitializeComponent();
            StaffAccess? resolvedAccess = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, resolvedAccess, StaffFeature.SupplierManagement)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            async Task reload() => await UiGridHelper.BindAsync(grid, _controller.GetAllSuppliers);
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                Suppliers? entity = await UiGridHelper.PromptForEntity<Suppliers>("Add supplier", "SupplierId");
                if (entity is null) return;
                await _controller.AddSupplier(entity);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Edit", async () =>
            {
                Suppliers? selected = UiGridHelper.GetSelected<Suppliers>(grid);
                if (selected is null || !UiGridHelper.EditEntity(selected, "Edit supplier", "SupplierId")) return;
                await _controller.UpdateSupplier(selected);
                await reload();
            });
            UiGridHelper.AddButton(toolbar, "Delete", async () =>
            {
                Suppliers? selected = UiGridHelper.GetSelected<Suppliers>(grid);
                if (selected is null || !UiGridHelper.ConfirmDelete("supplier")) return;
                await _controller.DeleteSupplier(selected.SupplierId);
                await reload();
            });
            Load += async (_, _) => await reload();
        }
    }
}
