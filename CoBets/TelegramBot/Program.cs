using System;
using System.IO;
using CoBetsDatabase;
using CoBetsDatabase.Repositories.Implementation;
using CoBetsDatabase.Repositories.Interfaces;
using Controllers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TelegramBot
{
    static class Program
    {
        private const string AppSettings = "appsettings.json";
        
        static void Main(string[] args)
        {
            var logger = SetupLogger();
            Log.Logger = logger;
            
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Bot was stopped due to exception!");
                throw;
            }
            finally
            {
                logger.Dispose();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettings)
                .Build();
            
            var token = config.GetSection("Token").Value;
            var connectionString = config.GetSection("DefaultConnection").Value;
            
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<CoBetsDbContext>(option =>
                        option.UseNpgsql(connectionString));
                    services.AddSingleton(provider => token);
                    
                    services.AddScoped<ITeamRepository, TeamRepository>();
                    services.AddScoped<IPlayerRepository, PlayerRepository>();
                    services.AddTransient<TeamController>();
                    services.AddTransient<PlayerController>();
                    
                    services.AddSingleton<DatabaseManager>();
                    services.AddSingleton<BotMessages>();
                    services.AddSingleton<BotMenu>();
                    services.AddSingleton<Handlers>();
                    
                    services.AddHostedService<Bot>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseSerilog();
            
            

            return host;
        }

        private static Logger SetupLogger()
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettings)
                .Build();

            var lConfig = new LoggerConfiguration();
            lConfig = lConfig.ReadFrom.Configuration(configuration);
            var logger = lConfig.CreateLogger();

            return logger;
        }
    }
}
