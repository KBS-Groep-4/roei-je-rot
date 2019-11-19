using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RoeiJeRot.Database.Database
{
    /// <summary>
    /// Class that provides access to the database.
    /// </summary>
    public class RoeiJeRotDbContext : DbContext
    {
        public RoeiJeRotDbContext()
        {
        }

        public RoeiJeRotDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// A list with boats from the database.
        /// </summary>
        public DbSet<Boat> Boats { get; set; }

        /// <summary>
        /// A list with users from the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", true)
                .Build();
            
            optionsBuilder.UseSqlServer(configuration["connectionString"]);
        }
    }
}
