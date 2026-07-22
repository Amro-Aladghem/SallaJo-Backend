using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SallaJo.ContextFactory
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.Development.json")
             .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(configuration.GetSection("SQL_CONNECTION_STRING").Value,
            b => b.MigrationsAssembly("SallaJo"));

            return new AppDbContext(builder.Options);
        }
    }
}
