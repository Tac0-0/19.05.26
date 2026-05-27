using Doner.Controller;
using Doner.Data.Entities;
using Doner.Data.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doner.Test.Services
{
    public class AuthServiceTest
    {
        [Test]
        public async Task Login_User_1()
        {
            var context = Helpers.TestDbFactory.CreateContext();

            context.Users.Add(new Customers
            {
                UserName = "JohnDoe67",
                FirstName = "John",
                LastName = "Doe",
                Email = "JohnDoe@gmail.com",
                Role = UserRole.Customer,
                Password = "password",
                PhoneNumber = "1234567890",
                IsActive = true
            });

            await context.SaveChangesAsync();

            AuthController service = new AuthController(context);
            Users? user = await service.Login("JohnDoe67", "password");

            Assert.That(user != null);
        }

        [Test]
        public async Task Login_User_2()
        {
            var context = Helpers.TestDbFactory.CreateContext();

            context.Users.Add(new Employees
            {
                UserName = "JohnDoe67Cook",
                FirstName = "John",
                LastName = "Doe",
                Email = "JohnDoe@gmail.com",
                Role = UserRole.Employee,
                Password = "password",
                PhoneNumber = "1234567890",
                IsActive = true,
                EmployeePosition = EmployeePosition.Cook,
                HireDate = DateTime.Now,
                Salary = 3000
            });

            await context.SaveChangesAsync();
            AuthController service = new AuthController(context);
            Users? user = await service.Login("JohnDoe67Cook", "password");
            Assert.That(user != null);
        }
    } 
}
