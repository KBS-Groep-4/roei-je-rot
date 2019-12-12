using System.Collections.Generic;
using NUnit.Framework;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.Test
{
    internal class TestUserService : IUserService
    {
        public List<User> GetUsers()
        {
            var list = new List<User>();

            list.Add(new User
            {
                Password = Hasher.Hash("abc"),
                Username = "abc"
            });

            return list;
        }

        public User GetUserByUserName(string username)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class LoginTests
    {
        [TestCase("abc", "abc")]
        public void LoginUserPaulCorrect(string username, string password)
        {
            //Arrange
            var logic = new AuthenticationService(new TestUserService());

            //Act
            var value = logic.AuthenticateUser(username, password);

            //Assert
            Assert.AreEqual(value, true);
        }

        [TestCase("abc", "abd")]
        public void LoginUserPaulIncorrect(string username, string password)
        {
            //Arrange
            var logic = new AuthenticationService(new TestUserService());

            //Act
            var value = logic.AuthenticateUser(username, password);

            //Assert
            Assert.AreEqual(value, false);
        }
    }
}