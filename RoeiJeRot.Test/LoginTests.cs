using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.Test
{
    class TestUserService : IUserService
    {
        public List<User> GetUsers()
        {
            List<User> list = new List<User>();
            list.Add(new User()
            {
                FirstName = "Paul",
                LastName = "Hiemstra",
                City = "Zwolle",
                Country = "Nederland",
                HouseNumber = "2",
                Password = Hasher.Hash("abc"),
                Username = "abc",
                SailingLevel = 1,
                StreetName = "Duckweg"
            });

            return list;
        }
    }
    class LoginTests
    {
        [TestCase("abc", "abc")]
        public void LoginUserPaulCorrect(string username, string password)
        {
            //Arrange
            LoginLogic logic = new LoginLogic(new TestUserService());

            //Act
            bool value = logic.AuthenticateUser(username, password);

            //Assert
            Assert.AreEqual(value, true);
        }

        [TestCase("abc", "abd")]
        public void LoginUserPaulIncorrect(string username, string password)
        {
            //Arrange
            LoginLogic logic = new LoginLogic(new TestUserService());

            //Act
            bool value = logic.AuthenticateUser(username, password);

            //Assert
            Assert.AreEqual(value, false);
        }
    }
}
