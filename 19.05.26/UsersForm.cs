using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public partial class UsersForm : Form
    {
        private readonly UsersController _controller = new();
        private readonly StaffAccess? _access;

        public UsersForm(StaffAccess? access = null)
        {
            InitializeComponent();
            _access = access ?? StaffAccess.FromCurrentSession();
            if (StaffAccess.DenyUnless(this, _access, StaffFeature.UserAdministration)) return;

            var (grid, toolbar) = UiGridHelper.BuildGridUi(contentPanel);
            grid.CellFormatting += UsersGrid_CellFormatting;
            async Task reload()
            {
                await UiGridHelper.BindAsync(grid, _controller.GetAllUsers);
                grid.Columns[nameof(Users.Role)].HeaderText = "Role / Position";
            }
            UiGridHelper.AddButton(toolbar, "Reload", reload);
            UiGridHelper.AddButton(toolbar, "Add", async () =>
            {
                NewUserType input = new();
                if (!UiGridHelper.EditEntity(input, "Choose user type")) return;
                Users user = CreateUser(input.UserType);
                user.IsActive = true;
                if (!UiGridHelper.EditEntity(user, "Add user", "UserId", "Role", "EmployeePosition")) return;
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

        private static void UsersGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs args)
        {
            if (sender is not DataGridView grid ||
                args.RowIndex < 0 ||
                args.ColumnIndex < 0 ||
                grid.Columns[args.ColumnIndex].DataPropertyName != nameof(Users.Role) ||
                grid.Rows[args.RowIndex].DataBoundItem is not Employees employee)
            {
                return;
            }

            args.Value = Enum.IsDefined(typeof(EmployeePosition), employee.EmployeePosition)
                ? employee.EmployeePosition.ToString()
                : $"Unknown position ({Convert.ToInt64(employee.EmployeePosition)})";
            args.FormattingApplied = true;
        }

        private static Users CreateUser(NewUserTypeSelection userType)
        {
            return userType switch
            {
                NewUserTypeSelection.Admin => new Admins { Role = UserRole.Admin },
                NewUserTypeSelection.Cashier => CreateEmployee(EmployeePosition.Cashier),
                NewUserTypeSelection.Cook => CreateEmployee(EmployeePosition.Cook),
                NewUserTypeSelection.Cleaner => CreateEmployee(EmployeePosition.Cleaner),
                NewUserTypeSelection.DeliveryWorker => CreateEmployee(EmployeePosition.DeliveryWorker),
                NewUserTypeSelection.Manager => CreateEmployee(EmployeePosition.Manager),
                _ => new Customers { Role = UserRole.Customer }
            };
        }

        private static Employees CreateEmployee(EmployeePosition position)
        {
            return new Employees
            {
                Role = UserRole.Employee,
                EmployeePosition = position,
                HireDate = DateTime.Now
            };
        }

        private sealed class NewUserType
        {
            public NewUserTypeSelection UserType { get; set; }
        }

        private enum NewUserTypeSelection
        {
            Customer,
            Cashier,
            Cook,
            Cleaner,
            DeliveryWorker,
            Manager,
            Admin
        }
    }
}
