using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class SuppliersController : DbController
{
    public SuppliersController(Func<DonerDBContext>? createContext = null) : base(createContext)
    {
    }
    public async Task<List<Suppliers>> GetAllSuppliers()
    {
        await using DonerDBContext context = CreateContext();
        return await context.Suppliers.ToListAsync();
    }

    public async Task<Suppliers?> GetSupplierById(int id)
    {
        await using DonerDBContext context = CreateContext();
        return await context.Suppliers.FindAsync(id);
    }

    public async Task AddSupplier(Suppliers supplier)
    {
        await using DonerDBContext context = CreateContext();
        await context.Suppliers.AddAsync(supplier);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSupplier(Suppliers supplier)
    {
        await using DonerDBContext context = CreateContext();
        context.Suppliers.Update(supplier);
        await context.SaveChangesAsync();
    }

    public async Task DeleteSupplier(int id)
    {
        await using DonerDBContext context = CreateContext();
        Suppliers? supplier = await context.Suppliers.FindAsync(id);
        if (supplier is null)
        {
            return;
        }

        context.Suppliers.Remove(supplier);
        await context.SaveChangesAsync();
    }
}
