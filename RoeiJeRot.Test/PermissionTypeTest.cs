using NUnit.Framework;
using RoeiJeRot.Logic;

namespace RoeiJeRot.Test
{
    [TestFixture]
    internal class PermissionTypeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetPermissionTypeEnumFromStringShouldBeNone()
        {
            var permissionType = Roles.GetPermissionType(new string[] { });
            Assert.True(permissionType.HasFlag(PermissionType.None));
        }

        [Test]
        public void PermissionTypeEnumFromStringShouldContainFlags()
        {
            var permissionType =
                Roles.GetPermissionType(new[] {Roles.ADMIN, Roles.MC, Roles.WC, Roles.MEMBER, Roles.STAFF});
            Assert.True(permissionType.HasFlag(PermissionType.Mc));
            Assert.True(permissionType.HasFlag(PermissionType.Admin));
            Assert.True(permissionType.HasFlag(PermissionType.Member));
            Assert.True(permissionType.HasFlag(PermissionType.Wc));
        }
    }
}