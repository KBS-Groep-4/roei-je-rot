using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Config;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.Views;
using RoeiJeRot.View.Wpf.Views.UserControls;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf
{
    public partial class App : Application
    {
        private WindowManager _windowManager;

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
                            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.{Environment.MachineName}.json", true)
                                .Build();
                            
                            opts.UseSqlServer(configuration["connectionString"],
                                o => o.MigrationsAssembly("LocatieNu.Web.Api"));
                        })
                        .AddSingleton<Window>()
                        .AddSingleton<WindowManager>()

                        .AddSingleton<IUserService, UserService>()
                        .AddSingleton<IBoatService, BoatService>()
                        .AddSingleton<IReservationService, ReservationService>()
                        .AddSingleton<IAuthenticationService, AuthenticationService>()
                        .AddScoped<IMailService, MailService>()


                        .AddTransient<MainWindow>()
                        .AddTransient<LoginWindow>()
                        .AddTransient<ReservationScreen>()
                        .AddTransient<ReservationOverviewScreen>()

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

            _windowManager = Host.Services.GetService<WindowManager>();
            _windowManager.ShowLogin();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (Host)
            {
                await Host.StopAsync(TimeSpan.FromSeconds(5));
                _windowManager?.CurrentWindow.Close();
            }
        }
    }
}