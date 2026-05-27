using Doner.Data;
using Microsoft.EntityFrameworkCore;

namespace Doner.Test.Helpers
{
    public class TestDbFactory
    {
        public static DonerDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DonerDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            DonerDBContext context = new DonerDBContext(options);

            context.Database.EnsureCreated();
            return context;
        }
    }
}
