using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller
{
    public class AuthController
    {
        public AuthController()
        {
            context = new DonerDBContext();
        }
        public AuthController(DonerDBContext dbContext)
        {
            context = dbContext;
        }


        private static Users? LoggedUser;
        private DonerDBContext context;

        public async Task<Users?> Login(string username, string password)
        {
            LoggedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password && u.IsActive);
            return LoggedUser;
        }

        public async Task<bool> Register(Users user)
        {
            bool exists = await context.Users.AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email);
            if (exists)
            {
                return false;
            }

            user.IsActive = true;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }

        public Task<Users?> GetLoggedUser()
        {
            return Task.FromResult(LoggedUser);
        }

        public Task Logout()
        {
            LoggedUser = null;
            return Task.CompletedTask;
        }

        public Task<bool> IsAdmin()
        {
            return Task.FromResult(LoggedUser?.Role == UserRole.Admin);
        }

        public Task<bool> IsCustomer()
        {
            return Task.FromResult(LoggedUser?.Role == UserRole.Customer);
        }
    }
}
