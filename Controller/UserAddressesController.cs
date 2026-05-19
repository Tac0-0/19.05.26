using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class UserAddressesController
{
    public async Task<List<UserAddresses>> GetAddressesByUser(int userId)
    {
        await using DonerDBContext context = new();
        return await context.UserAddresses.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<UserAddresses?> GetDefaultAddress(int userId)
    {
        await using DonerDBContext context = new();
        return await context.UserAddresses.FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
    }

    public async Task AddAddress(UserAddresses address)
    {
        await using DonerDBContext context = new();
        await context.UserAddresses.AddAsync(address);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAddress(UserAddresses address)
    {
        await using DonerDBContext context = new();
        context.UserAddresses.Update(address);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAddress(int id)
    {
        await using DonerDBContext context = new();
        UserAddresses? address = await context.UserAddresses.FindAsync(id);
        if (address is null) return;
        context.UserAddresses.Remove(address);
        await context.SaveChangesAsync();
    }

    public async Task SetDefaultAddress(int addressId)
    {
        await using DonerDBContext context = new();
        UserAddresses? target = await context.UserAddresses.FindAsync(addressId);
        if (target is null) return;
        List<UserAddresses> userAddresses = await context.UserAddresses.Where(a => a.UserId == target.UserId).ToListAsync();
        foreach (UserAddresses address in userAddresses) address.IsDefault = address.UserAddressId == addressId;
        await context.SaveChangesAsync();
    }
}
