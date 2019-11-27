using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Config;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf
{
    public partial class App : Application
    {
        internal static IHost Host { get; set; }

        public App()
        {
            Host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.{Environment.MachineName}.json", true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<Config>(context.Configuration)
                        .AddSingleton<IConfig, Config>(_ => new Config(context.Configuration))
                        .AddDbContext<RoeiJeRotDbContext>(opts =>
                        {
                            opts.UseSqlServer(context.Configuration["connectionString"],
                                o => o.MigrationsAssembly("LocatieNu.Web.Api"));
                        })
                        .AddSingleton<IUserService, UserService>()
                        .AddSingleton<IBoatService, BoatService>()
                        .AddSingleton<MainWindow>()
                        .AddSingleton<DataSeeder>();
                })
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .Build();

            var seeder = Host.Services.GetService<DataSeeder>();
            seeder.Seed();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Host.StartAsync();

            var mainWindow = Host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (Host)
            {
                await Host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
