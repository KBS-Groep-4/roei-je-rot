using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;

namespace RoeiJeRot.View.Wpf
{
    internal class DataSeeder
    {
        private readonly RoeiJeRotDbContext _context;

        public DataSeeder(RoeiJeRotDbContext context)
        {
            _context = context;

        }

        public void Seed()
        {
            if (!_context.Users.Any()) {
                SeedUsers();
            }
        }

        public void SeedUsers()
        {
            _context.Users.Add(new User() {FirstName = "Pauul", LastName = "Stra"});
            _context.SaveChanges();
        }
    }
}
