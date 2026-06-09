using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;

namespace _19._05._26
{
    public enum StaffFeature
    {
        UserAdministration,
        ProductCatalog,
        OrderDesk,
        KitchenOrders,
        CategoryManagement,
        SupplierManagement,
        InventoryManagement,
        DeliveryManagement,
        AssignedDeliveries,
        Reports,
        JsonDataTransfer
    }

    public sealed class StaffAccess
    {
        public StaffAccess(Users user)
        {
            User = user;
        }

        public Users User { get; }
        public bool IsAdmin => User.Role == UserRole.Admin;
        public EmployeePosition? Position => User is Employees employee ? employee.EmployeePosition : null;
        public bool IsManager => Position == EmployeePosition.Manager;
        public bool IsCashier => Position == EmployeePosition.Cashier;
        public bool IsCook => Position == EmployeePosition.Cook;
        public bool IsDeliveryWorker => Position == EmployeePosition.DeliveryWorker;

        public string DisplayRole => IsAdmin ? UserRole.Admin.ToString() : Position?.ToString() ?? User.Role.ToString();

        public static StaffAccess? FromCurrentSession()
        {
            Users? user = AuthController.CurrentUser;
            return user is null || user.Role == UserRole.Customer ? null : new StaffAccess(user);
        }

        public bool CanOpen(StaffFeature feature)
        {
            return feature switch
            {
                StaffFeature.UserAdministration => IsAdmin,
                StaffFeature.JsonDataTransfer => IsAdmin,
                StaffFeature.Reports => IsAdmin || IsManager,
                StaffFeature.ProductCatalog => IsAdmin || IsManager,
                StaffFeature.CategoryManagement => IsAdmin || IsManager,
                StaffFeature.SupplierManagement => IsAdmin || IsManager,
                StaffFeature.InventoryManagement => IsAdmin || IsManager,
                StaffFeature.DeliveryManagement => IsAdmin || IsManager,
                StaffFeature.OrderDesk => IsAdmin || IsManager || IsCashier,
                StaffFeature.KitchenOrders => IsAdmin || IsManager || IsCook,
                StaffFeature.AssignedDeliveries => IsAdmin || IsManager || IsDeliveryWorker,
                _ => false
            };
        }

        public static bool DenyUnless(Form form, StaffAccess? access, StaffFeature feature)
        {
            if (access?.CanOpen(feature) == true)
            {
                return false;
            }

            form.Shown += (_, _) =>
            {
                MessageBox.Show(
                    $"Your account is not allowed to use {GetFeatureName(feature)}.",
                    "Access denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                form.Close();
            };
            return true;
        }

        public static string GetFeatureName(StaffFeature feature)
        {
            return feature switch
            {
                StaffFeature.UserAdministration => "user administration",
                StaffFeature.ProductCatalog => "product catalog management",
                StaffFeature.OrderDesk => "order desk",
                StaffFeature.KitchenOrders => "kitchen orders",
                StaffFeature.CategoryManagement => "category management",
                StaffFeature.SupplierManagement => "supplier management",
                StaffFeature.InventoryManagement => "inventory management",
                StaffFeature.DeliveryManagement => "delivery management",
                StaffFeature.AssignedDeliveries => "assigned deliveries",
                StaffFeature.Reports => "reports",
                StaffFeature.JsonDataTransfer => "JSON data transfer",
                _ => "this feature"
            };
        }
    }
}
