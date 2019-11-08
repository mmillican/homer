using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Homer.Shared.Data
{
    public class HomerDbContextFactory : IDesignTimeDbContextFactory<HomerDbContext>
    {
        public HomerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HomerDbContext>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Homer;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new HomerDbContext(builder.Options);
        }
    }
}
