using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    ///     Data seeder that inserts test data into the database.
    /// </summary>
    internal class DataSeeder
    {
        private readonly RoeiJeRotDbContext _context;

        public DataSeeder(RoeiJeRotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Inserts test data into database.
        /// </summary>
        public void Seed()
        {
            if (!EnumerableExtensions.Any(_context.Users))
                try
                {
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT users ON");
                    _context.SaveChanges();

                    SeedUsers();
                    SeedBoatTypes();
                    SeedBoats();
                    SeedReservations();

                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT users OFF");
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
        }

        private void SeedUsers()
        {
            _context.Users.Add(new User
            {
                FirstName = "Paul", LastName = "Hiemstra", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = Hasher.Hash("abc"), Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.Users.Add(new User
            {
                FirstName = "Frank", LastName = "Demilt", City = "Zwolle", Country = "Nederland", HouseNumber = "2",
                Password = Hasher.Hash("abc"), Username = "abc", SailingLevel = 1, StreetName = "Duckweg"
            });
            _context.SaveChanges();
        }

        private void SeedBoats()
        {
            //Make for every boat type 5 boats
            foreach (BoatType type in _context.SailingBoatTypes)
                for (int i = 0; i < 5; i++)
                    _context.SailingBoats.Add(new SailingBoat { Status = 0, BoatTypeId = type.Id });

            _context.SaveChanges();
        }

        private void SeedBoatTypes()
        {
            _context.SailingBoatTypes.Add(new BoatType
                {PossiblePassengers = 3, RequiredLevel = 2, Name = "Grote kano"});
            _context.SailingBoatTypes.Add(
                new BoatType {PossiblePassengers = 1, RequiredLevel = 1, Name = "Kleine kano"});
            _context.SailingBoatTypes.Add(new BoatType()
            {
                PossiblePassengers = 5,
                Name = "Zeilboot",
                RequiredLevel = 5
            });
            _context.SailingBoatTypes.Add(new BoatType()
            {
                PossiblePassengers = 4,
                Name = "Zwaartboot",
                RequiredLevel = 6
            });
            _context.SailingBoatTypes.Add(new BoatType()
            {
                Name = "Flying Dutchman",
                PossiblePassengers = 5,
                RequiredLevel = 2
            });
            _context.SaveChanges();
        }

        private void SeedReservations()
        {
            _context.Reservations.Add(new SailingReservation
            {
                Date = DateTime.Now,
                Duration = TimeSpan.FromMinutes(50),
                ReservedByUserId = _context.Users.ToList()[0].Id,
                ReservedSailingBoatId = _context.SailingBoats.ToList()[0].Id
            });
            _context.Reservations.Add(new SailingReservation
            {
                Date = new DateTime(2020, 4, 9, 13, 30, 0),
                Duration = TimeSpan.FromMinutes(40),
                ReservedByUserId = _context.Users.ToList()[1].Id,
                ReservedSailingBoatId = _context.SailingBoats.ToList()[1].Id
            });
            _context.Reservations.Add(new SailingReservation
            {
                Date = new DateTime(2020, 4, 10, 13, 30, 0),
                Duration = TimeSpan.FromMinutes(90),
                ReservedByUserId = _context.Users.ToList()[1].Id,
                ReservedSailingBoatId = _context.SailingBoats.ToList()[1].Id
            });
            _context.Reservations.Add(new SailingReservation
            {
                Date = new DateTime(2020, 4, 9, 11, 30, 0),
                Duration = TimeSpan.FromMinutes(90),
                ReservedByUserId = _context.Users.ToList()[1].Id,
                ReservedSailingBoatId = _context.SailingBoats.ToList()[1].Id
            });
            _context.SaveChanges();
        }
    }
}