using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace RoeiJeRot.Test
{
    [TestFixture]
    internal class WindowManagerTest
    {
        [SetUp]
        public void Setup()
        {
            host = new HostBuilder().Build();
        }

        private IHost host;

        public void Test1()
        {
        }
    }
}