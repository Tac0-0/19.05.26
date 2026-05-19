using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class SuppliersController
{
    public async Task<List<Suppliers>> GetAllSuppliers(){await using DonerDBContext c=new(); return await c.Suppliers.ToListAsync();}
    public async Task<Suppliers?> GetSupplierById(int id){await using DonerDBContext c=new(); return await c.Suppliers.FindAsync(id);}
    public async Task AddSupplier(Suppliers supplier){await using DonerDBContext c=new(); await c.Suppliers.AddAsync(supplier); await c.SaveChangesAsync();}
    public async Task UpdateSupplier(Suppliers supplier){await using DonerDBContext c=new(); c.Suppliers.Update(supplier); await c.SaveChangesAsync();}
    public async Task DeleteSupplier(int id){await using DonerDBContext c=new(); var s=await c.Suppliers.FindAsync(id); if(s is null) return; c.Suppliers.Remove(s); await c.SaveChangesAsync();}
}
