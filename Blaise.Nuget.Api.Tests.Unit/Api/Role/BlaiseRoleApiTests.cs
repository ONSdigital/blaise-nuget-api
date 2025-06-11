using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Role
{
    public class BlaiseRoleApiTests
    {
        private Mock<IRoleService> _roleServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _name;
        private readonly string _description;
        private readonly List<string> _permissions;

        private IBlaiseRoleApi _sut;

        public BlaiseRoleApiTests()
        {
            _connectionModel = new ConnectionModel();
            _name = "Admin";
            _description = "Test";
            _permissions = new List<string> { "Permission1" };
        }

        [SetUp]
        public void SetUpTests()
        {
            _roleServiceMock = new Mock<IRoleService>();

            _sut = new BlaiseRoleApi(_roleServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseRoleApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseRoleApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseRoleApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseRoleApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetRoles_Then_The_Correct_List_Of_Roles_Are_Returned()
        {
            //arrange
            var roleMock = new Mock<IRole>();
            var roles = new List<IRole> { roleMock.Object };

            _roleServiceMock.Setup(r => r.GetRoles(_connectionModel)).Returns(roles);

            //act
            var result = _sut.GetRoles();

            //assert
            Assert.AreSame(roles, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetRole_Then_The_Correct_Role_Is_Returned()
        {
            //arrange
            var roleMock = new Mock<IRole>();

            _roleServiceMock.Setup(r => r.GetRole(_connectionModel, _name)).Returns(roleMock.Object);

            //act
            var result = _sut.GetRole(_name);

            //assert
            Assert.AreSame(roleMock.Object, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_GetRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetRole(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_GetRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetRole(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_RoleExists_Then_The_Correct_Value_Is_Returned(bool exists)
        {

            _roleServiceMock.Setup(r => r.RoleExists(_connectionModel, _name)).Returns(exists);

            //act
            var result = _sut.RoleExists(_name);

            //assert
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_RoleExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RoleExists(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_RoleExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RoleExists(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.AddRole(_name, _description, _permissions);

            //assert
            _roleServiceMock.Verify(v => v.AddRole(_connectionModel, _name, _description, _permissions), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(string.Empty, _description, _permissions));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(null, _description, _permissions));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveRole(_name);

            //assert
            _roleServiceMock.Verify(v => v.RemoveRole(_connectionModel, _name), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_RemoveRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveRole(string.Empty));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_RemoveRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveRole(null));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateRolePermissions_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UpdateRolePermissions(_name, _permissions);

            //assert
            _roleServiceMock.Verify(v => v.UpdateRolePermissions(_connectionModel, _name, _permissions)
                , Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_UpdateRolePermissions_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRolePermissions(string.Empty, _permissions));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_UpdateRolePermissions_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRolePermissions(null, _permissions));
            Assert.AreEqual("name", exception.ParamName);
        }
    }
}
