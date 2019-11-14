using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
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

namespace RoeiJeRot.View.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
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
                        .AddSingleton<MainWindow>()
                        .AddDbContext<RoeiJeRotDbContext>(opts =>
                        {
                            opts.UseSqlServer(context.Configuration["connectionString"],
                                o => o.MigrationsAssembly("LocatieNu.Web.Api"));
                        });
                })
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
