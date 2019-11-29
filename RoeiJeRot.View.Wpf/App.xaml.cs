using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Config;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Views;

namespace RoeiJeRot.View.Wpf
{
    public partial class App : Application
    {
        public App()
        {
            Host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", false)
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
                        .AddSingleton<IReservationService, ReservationService>()
                        .AddSingleton<IAuthenticationService, AuthenticationService>()
                        .AddSingleton<LoginWindow>()
                        .AddSingleton<ReservationWindow>()
                        .AddSingleton<ReservationOverviewWindow>()
                        .AddSingleton<DataSeeder>();
                })
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .Build();

            var seeder = Host.Services.GetService<DataSeeder>();
            seeder.Seed();
        }

        internal static IHost Host { get; set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Host.StartAsync();

            var loginWindow = Host.Services.GetService<LoginWindow>();
            loginWindow.Show();
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