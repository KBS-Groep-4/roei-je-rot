using NUnit.Framework;
using RoeiJeRot.Logic;

namespace RoeiJeRot.Test
{
    [TestFixture]
    internal class HasherTests
    {
        [TestCase("9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", "test")]
        [TestCase("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", "abc")]
        public void ComparePasswordToHash(string dbHash, string password)
        {
            //Act
            var value = Hasher.Compare(dbHash, password);

            //Assert
            Assert.AreEqual(value, true);
        }
    }
}