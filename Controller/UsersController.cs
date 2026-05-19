using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller;

public class UsersController
{
    public async Task<List<Users>> GetAllUsers()
    {
        await using DonerDBContext context = new();
        return await context.Users.ToListAsync();
    }

    public async Task<List<Users>> GetUsersByRole(UserRole role)
    {
        await using DonerDBContext context = new();
        return await context.Users.Where(u => u.Role == role).ToListAsync();
    }

    public async Task<Users?> GetUserById(int id)
    {
        await using DonerDBContext context = new();
        return await context.Users.FindAsync(id);
    }

    public async Task AddUser(Users user)
    {
        await using DonerDBContext context = new();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUser(Users user)
    {
        await using DonerDBContext context = new();
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        await using DonerDBContext context = new();
        Users? user = await context.Users.FindAsync(id);
        if (user is null) return;
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task ChangeRole(int userId, UserRole role)
    {
        await using DonerDBContext context = new();
        Users? user = await context.Users.FindAsync(userId);
        if (user is null) return;
        user.Role = role;
        await context.SaveChangesAsync();
    }

    public async Task SetActive(int userId, bool isActive)
    {
        await using DonerDBContext context = new();
        Users? user = await context.Users.FindAsync(userId);
        if (user is null) return;
        user.IsActive = isActive;
        await context.SaveChangesAsync();
    }
}
