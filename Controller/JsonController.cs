using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Doner.Data.Enum;

namespace Doner.Controller;

public class JsonController : DbController
{
    public JsonController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new UsersJsonConverter() }
    };

    public async Task ImportAll(string path)
    {
        string json = await File.ReadAllTextAsync(path);
        AllDataImport data = JsonSerializer.Deserialize<AllDataImport>(json, JsonOptions) ?? new AllDataImport();
        List<Users> users = data.Users.Count > 0
            ? data.Users
            : [.. data.Customers, .. data.Employees, .. data.Admins];
        RemoveNavigationReferences(data, users);

        await using DonerDBContext context = CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await ClearDatabaseAsync(context);
            await AddWithIdentityInsertAsync(context, "Suppliers", data.Suppliers);
            await AddWithIdentityInsertAsync(context, "Users", users);
            await AddWithIdentityInsertAsync(context, "UserAddresses", data.UserAddresses);
            await AddWithIdentityInsertAsync(context, "Categories", data.Categories);
            await AddWithIdentityInsertAsync(context, "Products", data.Products);
            await AddWithIdentityInsertAsync(context, "Orders", data.Orders);
            await AddWithIdentityInsertAsync(context, "OrderDetails", data.OrderDetails);
            await AddWithIdentityInsertAsync(context, "Payments", data.Payments);
            await AddWithIdentityInsertAsync(context, "Deliveries", data.Deliveries);
            await AddWithIdentityInsertAsync(context, "Ingredients", data.Ingredients);
            await AddWithIdentityInsertAsync(context, "ProductIngredients", data.ProductIngredients);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task ClearDatabaseAsync(DonerDBContext context)
    {
        const string deleteSql = """
            DELETE FROM [ProductIngredients];
            DELETE FROM [Deliveries];
            DELETE FROM [Payments];
            DELETE FROM [OrderDetails];
            DELETE FROM [Ingredients];
            DELETE FROM [Orders];
            DELETE FROM [Products];
            DELETE FROM [UserAddresses];
            DELETE FROM [Categories];
            DELETE FROM [Suppliers];
            DELETE FROM [Users];
            """;
        await context.Database.ExecuteSqlRawAsync(deleteSql);
    }

    private static async Task AddWithIdentityInsertAsync<TEntity>(DonerDBContext context, string tableName, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        List<TEntity> rows = entities.ToList();
        if (rows.Count == 0) return;

        await context.Set<TEntity>().AddRangeAsync(rows);
        await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [{tableName}] ON;");
        try
        {
            await context.SaveChangesAsync();
        }
        finally
        {
            await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [{tableName}] OFF;");
            context.ChangeTracker.Clear();
        }
    }

    private static void RemoveNavigationReferences(AllDataImport data, IEnumerable<Users> users)
    {
        foreach (Users user in users)
        {
            user.UserAddresses = null!;
            user.Orders = null!;
        }
        foreach (UserAddresses address in data.UserAddresses) address.User = null!;
        foreach (Categories category in data.Categories) category.Products = null!;
        foreach (Products product in data.Products) product.Category = null!;
        foreach (Orders order in data.Orders)
        {
            order.User = null!;
            order.Address = null!;
            order.OrderDetails = null!;
            order.Payments = null!;
        }
        foreach (OrderDetails detail in data.OrderDetails)
        {
            detail.Order = null!;
            detail.Product = null!;
        }
        foreach (Payments payment in data.Payments) payment.Order = null!;
        foreach (Deliveries delivery in data.Deliveries)
        {
            delivery.Order = null!;
            delivery.DeliveryWorker = null;
            delivery.Address = null!;
        }
        foreach (Ingredients ingredient in data.Ingredients) ingredient.Supplier = null!;
        foreach (ProductIngredients ingredient in data.ProductIngredients)
        {
            ingredient.Product = null!;
            ingredient.Ingredient = null!;
        }
    }

    public async Task ExportAll(string path)
    {
        await using DonerDBContext context = CreateContext();

        var data = new AllDataImport
        {
            Users = await context.Users.AsNoTracking().ToListAsync(),
            UserAddresses = await context.UserAddresses.AsNoTracking().ToListAsync(),
            Categories = await context.Categories.AsNoTracking().ToListAsync(),
            Products = await context.Products.AsNoTracking().ToListAsync(),
            Orders = await context.Orders.AsNoTracking().ToListAsync(),
            OrderDetails = await context.OrderDetails.AsNoTracking().ToListAsync(),
            Payments = await context.Payments.AsNoTracking().ToListAsync(),
            Deliveries = await context.Deliveries.AsNoTracking().ToListAsync(),
            Suppliers = await context.Suppliers.AsNoTracking().ToListAsync(),
            Ingredients = await context.Ingredients.AsNoTracking().ToListAsync(),
            ProductIngredients = await context.ProductIngredients.AsNoTracking().ToListAsync()
        };

        var json = JsonSerializer.Serialize(data, JsonOptions);
        await File.WriteAllTextAsync(path, json);
    }

    private sealed class UsersJsonConverter : JsonConverter<Users>
    {
        public override Users Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement root = document.RootElement;
            UserRole role = ReadEnum<UserRole>(root, nameof(Users.Role));
            string? discriminator = ReadOptionalString(root, "Discriminator");

            Users user = discriminator?.ToLowerInvariant() switch
            {
                "admins" => new Admins(),
                "employees" => new Employees(),
                "customers" => new Customers(),
                _ => role switch
                {
                    UserRole.Admin => new Admins(),
                    UserRole.Employee => new Employees(),
                    _ => new Customers()
                }
            };

            user.UserId = ReadOptionalInt32(root, nameof(Users.UserId));
            user.UserName = ReadOptionalString(root, nameof(Users.UserName)) ?? string.Empty;
            user.Password = ReadOptionalString(root, nameof(Users.Password)) ?? string.Empty;
            user.Email = ReadOptionalString(root, nameof(Users.Email)) ?? string.Empty;
            user.FirstName = ReadOptionalString(root, nameof(Users.FirstName)) ?? string.Empty;
            user.LastName = ReadOptionalString(root, nameof(Users.LastName)) ?? string.Empty;
            user.PhoneNumber = ReadOptionalString(root, nameof(Users.PhoneNumber)) ?? string.Empty;
            user.Role = role;
            user.IsActive = ReadOptionalBoolean(root, nameof(Users.IsActive));

            if (user is Employees employee)
            {
                employee.EmployeePosition = ReadEnum<EmployeePosition>(root, nameof(Employees.EmployeePosition));
                employee.Salary = ReadOptionalDecimal(root, nameof(Employees.Salary));
                employee.HireDate = ReadOptionalDateTime(root, nameof(Employees.HireDate));
            }
            return user;
        }

        public override void Write(Utf8JsonWriter writer, Users value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Discriminator", value.GetType().Name);
            writer.WriteNumber(nameof(Users.UserId), value.UserId);
            writer.WriteString(nameof(Users.UserName), value.UserName);
            writer.WriteString(nameof(Users.Password), value.Password);
            writer.WriteString(nameof(Users.Email), value.Email);
            writer.WriteString(nameof(Users.FirstName), value.FirstName);
            writer.WriteString(nameof(Users.LastName), value.LastName);
            writer.WriteString(nameof(Users.PhoneNumber), value.PhoneNumber);
            writer.WriteNumber(nameof(Users.Role), (int)value.Role);
            writer.WriteBoolean(nameof(Users.IsActive), value.IsActive);
            if (value is Employees employee)
            {
                writer.WriteNumber(nameof(Employees.EmployeePosition), (int)employee.EmployeePosition);
                writer.WriteNumber(nameof(Employees.Salary), employee.Salary);
                writer.WriteString(nameof(Employees.HireDate), employee.HireDate);
            }
            writer.WriteEndObject();
        }

        private static TEnum ReadEnum<TEnum>(JsonElement root, string propertyName) where TEnum : struct, Enum
        {
            if (!TryGetProperty(root, propertyName, out JsonElement property)) return default;
            if (property.ValueKind == JsonValueKind.Number && property.TryGetInt32(out int numericValue)) return (TEnum)Enum.ToObject(typeof(TEnum), numericValue);
            if (property.ValueKind == JsonValueKind.String && Enum.TryParse(property.GetString(), true, out TEnum value)) return value;
            throw new JsonException($"{propertyName} must be a valid {typeof(TEnum).Name} value.");
        }

        private static string? ReadOptionalString(JsonElement root, string propertyName) =>
            TryGetProperty(root, propertyName, out JsonElement property) && property.ValueKind != JsonValueKind.Null ? property.GetString() : null;

        private static int ReadOptionalInt32(JsonElement root, string propertyName) =>
            TryGetProperty(root, propertyName, out JsonElement property) && property.TryGetInt32(out int value) ? value : default;

        private static decimal ReadOptionalDecimal(JsonElement root, string propertyName) =>
            TryGetProperty(root, propertyName, out JsonElement property) && property.TryGetDecimal(out decimal value) ? value : default;

        private static bool ReadOptionalBoolean(JsonElement root, string propertyName) =>
            TryGetProperty(root, propertyName, out JsonElement property) && (property.ValueKind is JsonValueKind.True or JsonValueKind.False) && property.GetBoolean();

        private static DateTime ReadOptionalDateTime(JsonElement root, string propertyName) =>
            TryGetProperty(root, propertyName, out JsonElement property) && property.TryGetDateTime(out DateTime value) ? value : default;

        private static bool TryGetProperty(JsonElement root, string propertyName, out JsonElement property)
        {
            foreach (JsonProperty candidate in root.EnumerateObject())
            {
                if (candidate.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    property = candidate.Value;
                    return true;
                }
            }
            property = default;
            return false;
        }
    }

    public class AllDataImport
    {
        public List<Users> Users { get; set; } = new List<Users>();
        public List<Customers> Customers { get; set; } = new List<Customers>();
        public List<Employees> Employees { get; set; } = new List<Employees>();
        public List<Admins> Admins { get; set; } = new List<Admins>();
        public List<UserAddresses> UserAddresses { get; set; } = new List<UserAddresses>();
        public List<Categories> Categories { get; set; } = new List<Categories>();
        public List<Products> Products { get; set; } = new List<Products>();
        public List<Orders> Orders { get; set; } = new List<Orders>();
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public List<Payments> Payments { get; set; } = new List<Payments>();
        public List<Deliveries> Deliveries { get; set; } = new List<Deliveries>();
        public List<Suppliers> Suppliers { get; set; } = new List<Suppliers>();
        public List<Ingredients> Ingredients { get; set; } = new List<Ingredients>();
        public List<ProductIngredients> ProductIngredients { get; set; } = new List<ProductIngredients>();
    }
}
