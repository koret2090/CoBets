using System.IO;
using CoBetsDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TelegramBot.ContextFactory
{
    public class CoBetsDbContextFactory : IDesignTimeDbContextFactory<CoBetsDbContext>
    {
        private const string AppSettings = "appsettings.json";
        private const string ConnectionStringsSection = "DefaultConnection";

        public CoBetsDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettings)
                .Build();

            var connectionString = config.GetSection(ConnectionStringsSection).Value;

            var builder = new DbContextOptionsBuilder<CoBetsDbContext>();
            builder.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly("TelegramBot"));

            return new CoBetsDbContext(builder.Options);
        }
    }
}
