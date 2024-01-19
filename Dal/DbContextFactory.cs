using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dal
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
                .AddJsonFile("launchSettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string connectionString = config.GetValue<string>
                ($"profiles:{environment}:environmentVariables:DB_CONNECTION");

            optionsBuilder.UseMySql(connectionString,
                    serverVersion: ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly("Dal"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}