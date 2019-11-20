using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Data seeder that inserts test data into the database.
    /// </summary>
    internal class DataSeeder
    {
        private readonly RoeiJeRotDbContext _context;

        public DataSeeder(RoeiJeRotDbContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Inserts test data into database.
        /// </summary>
        public void Seed()
        {
            if (!_context.Users.Any())
            {
                SeedUsers();
            }
        }

        private void SeedUsers()
        {
            _context.Users.Add(new User()
            {
                Id = 1,
                FirstName = "Paul", LastName = "Hiemstra", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = "abc", Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.Users.Add(new User()
            {
                Id = 2,
                FirstName = "Frank", LastName = "de Milt", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = "abc", Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.SaveChanges();
        }

        private void SeedBoats()
        {
            _context.SailingBoats.Add(new SailingBoat() { Name = "Sailing Boat 01", InService = true });
            _context.SailingBoats.Add(new SailingBoat() { Name = "Sailing Boat 01", InService = false });
            _context.SaveChanges();
        }

        private void SeedReservations()
        {
            _context.Reservations.Add(new SailingReservation() { Date = DateTime.Now, Duration = 50, ReservedByUserId = 1 });
            _context.Reservations.Add(new SailingReservation() { Date = DateTime.Now, Duration = 50, ReservedByUserId = 2 });
        }
    }
}
