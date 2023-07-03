using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Mappers;
using Blaise.Nuget.Api.Core.Models;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Unit.Mappers
{
    public class RolePermissionMapperTests
    {
        private IRolePermissionMapper _sut;

        private List<string> _permissions;

        [SetUp]
        public void SetupTests()
        {
            _sut = new RolePermissionMapper();

            _permissions = new List<string> { "Permission1", "Permission2" };
        }

        [Test]
        public void Given_A_List_Of_Permissions_When_I_Call_MapToActionPermissionModels_I_Get_A_List_Of_ActionPermissionModels_Returned()
        {
            //act
            var result = _sut.MapToActionPermissionModels(_permissions);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ActionPermissionModel>>(result);
        }

        [Test]
        public void Given_A_List_Of_Permissions_When_I_Call_MapToActionPermissionModels_I_Get_An_Expected_List_Of_ActionPermissionModels_Returned()
        {
            //act
            var result = _sut.MapToActionPermissionModels(_permissions).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ActionPermissionModel>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Action == _permissions[0] && r.Permission == PermissionStatus.Allowed));
            Assert.IsTrue(result.Any(r => r.Action == _permissions[1] && r.Permission == PermissionStatus.Allowed));
        }
    }
}
