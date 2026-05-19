using Doner.Data;
using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller
{
    public class AuthController
    {
        public async Task<Users?> Login(string username, string password)
        {
            DonerDBContext context = new DonerDBContext();
            return await context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
        public async Task<bool> Register(Users user)
        {
            DonerDBContext context = new DonerDBContext();
            if (await Login(user.UserName, user.Password) != null)
            {
                return false;
            }
            else
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return true;
            }
        }
        public Task<Users?> GetLoggedUser()
        {
            return Task.FromResult<Users?>(null);
        }
    }
}
