using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller
{
    public class AuthController
    {
        private static Users? _loggedUser;

        public async Task<Users?> Login(string username, string password)
        {
            await using DonerDBContext context = new DonerDBContext();
            _loggedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password && u.IsActive);
            return _loggedUser;
        }

        public async Task<bool> Register(Users user)
        {
            await using DonerDBContext context = new DonerDBContext();
            bool exists = await context.Users.AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email);
            if (exists)
            {
                return false;
            }

            user.Role = UserRole.Customer;
            user.IsActive = true;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }

        public Task<Users?> GetLoggedUser() => Task.FromResult(_loggedUser);

        public Task Logout()
        {
            _loggedUser = null;
            return Task.CompletedTask;
        }

        public Task<bool> IsAdmin() => Task.FromResult(_loggedUser?.Role == UserRole.Admin);

        public Task<bool> IsCustomer() => Task.FromResult(_loggedUser?.Role == UserRole.Customer);
    }
}
