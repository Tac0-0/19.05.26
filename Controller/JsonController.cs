using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Doner.Controller;

public class JsonController
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public async Task ImportAll(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        var data = JsonSerializer.Deserialize<AllDataImport>(json) ?? new AllDataImport();

        await using DonerDBContext context = new();

        await context.Users.AddRangeAsync(data.Users);
        await context.Customers.AddRangeAsync(data.Customers);
        await context.Employees.AddRangeAsync(data.Employees);
        await context.Admins.AddRangeAsync(data.Admins);
        await context.UserAddresses.AddRangeAsync(data.UserAddresses);
        await context.Categories.AddRangeAsync(data.Categories);
        await context.Products.AddRangeAsync(data.Products);
        await context.Orders.AddRangeAsync(data.Orders);
        await context.OrderDetails.AddRangeAsync(data.OrderDetails);
        await context.Payments.AddRangeAsync(data.Payments);
        await context.Deliveries.AddRangeAsync(data.Deliveries);
        await context.Suppliers.AddRangeAsync(data.Suppliers);
        await context.Ingredients.AddRangeAsync(data.Ingredients);
        await context.ProductIngredients.AddRangeAsync(data.ProductIngredients);

        await context.SaveChangesAsync();
    }

    public async Task ExportAll(string path)
    {
        await using DonerDBContext context = new();

        var data = new AllDataImport
        {
            Users = await context.Users.AsNoTracking().ToListAsync(),
            Customers = await context.Customers.AsNoTracking().ToListAsync(),
            Employees = await context.Employees.AsNoTracking().ToListAsync(),
            Admins = await context.Admins.AsNoTracking().ToListAsync(),
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
