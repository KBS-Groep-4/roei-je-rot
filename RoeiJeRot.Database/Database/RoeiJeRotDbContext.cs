using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RoeiJeRot.Database.Database
{
    /// <summary>
    ///     Class that provides access to the database.
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
        ///     A list with boats from the database.
        /// </summary>
        public DbSet<SailingBoat> SailingBoats { get; set; }

        /// <summary>
        ///     A list with users from the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     A list with reservations from the database.
        /// </summary>
        public DbSet<SailingReservation> Reservations { get; set; }

        /// <summary>
        ///     A list with sailing boat damage reports from the database.
        /// </summary>
        public DbSet<SailingBoatDamageReport> SailingBoatDamageReports { get; set; }

        /// <summary>
        ///     A list with sailing boat competitions from the database.
        /// </summary>
        public DbSet<SailingCompetition> SailingCompetitions { get; set; }

        /// <summary>
        ///     A list with sailing boat competition participants from the database.
        /// </summary>
        public DbSet<SailingCompetitionParticipant> SailingCompetitionParticipants { get; set; }

        public DbSet<BoatType> SailingBoatTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> PermissionUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", true)
                .Build();

            optionsBuilder.UseSqlServer(configuration["connectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SailingCompetition>()
                .HasMany(x => x.SailingCompetitionParticipants)
                .WithOne(x => x.SailingCompetition)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}