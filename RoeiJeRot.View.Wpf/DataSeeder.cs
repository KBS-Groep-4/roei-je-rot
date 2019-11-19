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
            if (!_context.Users.Any()) {
                SeedUsers();
            }
        }

        private void SeedUsers()
        {
            _context.Users.Add(new User() {FirstName = "Paul", LastName = "Hiemstra"});
            _context.Users.Add(new User() { FirstName = "Frank", LastName = "de Milt" });
            _context.SaveChanges();
        }
    }
}
