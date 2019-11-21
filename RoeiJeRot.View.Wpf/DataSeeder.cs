using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
                try
                {
                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT users ON");
                    _context.SaveChanges();

                    SeedUsers();
                    SeedBoatTypes();
                    SeedBoats();
                    SeedReservations();

                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT users OFF");
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }
        }

        private void SeedUsers()
        {
            _context.Users.Add(new User()
            {
                FirstName = "Paul", LastName = "Hiemstra", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = "abc", Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.Users.Add(new User()
            {
                FirstName = "Frank", LastName = "de Milt", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = "abc", Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.SaveChanges();
        }

        private void SeedBoats()
        {
            _context.SailingBoats.Add(new SailingBoat() { Name = "Sailing Boat 01", InService = true, BoatTypeId = 1});
            _context.SailingBoats.Add(new SailingBoat() { Name = "Sailing Boat 01", InService = false, BoatTypeId = 1 });
            _context.SaveChanges();
        }

        private void SeedBoatTypes()
        {
            _context.SailingBoatTypes.Add(new BoatType() {  PossiblePassengers = 3, RequiredLevel = 2 });
        }

        private void SeedReservations()
        {
            _context.Reservations.Add(new SailingReservation() { Date = DateTime.Now, Duration = 50, ReservedByUserId = 1, });
            _context.Reservations.Add(new SailingReservation() { Date = DateTime.Now, Duration = 50, ReservedByUserId = 2 });
            _context.SaveChanges();
        }
    }
}
