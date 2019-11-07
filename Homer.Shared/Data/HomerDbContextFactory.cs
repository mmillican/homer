using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Homer.Shared.Data
{
    public class HomerDbContextFactory : IDesignTimeDbContextFactory<HomerDbContext>
    {
        public HomerDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

            configBuilder.AddEnvironmentVariables();
            var configuration = configBuilder.Build();

            var builder = new DbContextOptionsBuilder<HomerDbContext>();
            var connString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connString);

            return new HomerDbContext(builder.Options);

        }
    }
}
