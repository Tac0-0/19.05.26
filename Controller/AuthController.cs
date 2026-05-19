using Doner.Data;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace Doner.Controller
{
    public class AuthController
    {
        public async Task<Users> Login(string username, string password)
        {
            DonerDBContext context = new DonerDBContext();
            Customers? customer = await context.Customers.FirstOrDefaultAsync(c => c.UserName == username && c.Password == password);
            Employees? employee = await context.Employees.FirstOrDefaultAsync(e => e.UserName == username && e.Password == password);
            Admin? admin = await context.Admins.FirstOrDefaultAsync(a => a.UserName == username && a.Password == password);
            if (customer != null) return customer;
            if (employee != null) return employee;
            if (admin != null) return admin;
            return null;
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
                if (user.Role == UserRole.Customer)
                {
                    Customers customer = new Customers
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Password = user.Password,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };
                    await context.Customers.AddAsync(customer);
                }
                else if (user.Role == UserRole.Employee)
                {
                    Employees employee = new Employees
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Password = user.Password,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };
                    await context.Employees.AddAsync(employee);
                }
                else
                {
                    Admin admin = new Admin
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Password = user.Password,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };
                    await context.Admins.AddAsync(admin);
                }
                await context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<Users> GetLoggedUser()
        {
            return await 
        }
    }
}
