namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Models;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.Security;

    public class RoleServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private Mock<ISecurityManagerFactory> _securityFactoryMock;
        private Mock<IRolePermissionMapper> _mapperMock;
        private Mock<ISecurityServer> _securityServerMock;
        private IRoleService _sut;

        public RoleServiceTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _securityServerMock = new Mock<ISecurityServer>();

            _securityFactoryMock = new Mock<ISecurityManagerFactory>();
            _securityFactoryMock.Setup(r => r.GetConnection(_connectionModel))
                .Returns(_securityServerMock.Object);

            _mapperMock = new Mock<IRolePermissionMapper>();

            // setup service under test
            _sut = new RoleService(_securityFactoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_The_Correct_List_Of_Roles_Are_Returned()
        {
            // arrange
            var roleMock = new Mock<IRole>();
            var roles = new List<IRole> { roleMock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            var result = _sut.GetRoles(_connectionModel);

            // assert
            Assert.That(result, Is.SameAs(roles));
        }

        [Test]
        public void Given_I_Call_GetRole_Then_The_Correct_Role_Is_Returned()
        {
            // arrange
            const string role1Name = "Name1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);

            const string role2Name = "Name2";
            var role2Mock = new Mock<IRole>();
            role2Mock.Setup(r => r.Name).Returns(role2Name);
            var roles = new List<IRole> { role1Mock.Object, role2Mock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            var result = _sut.GetRole(_connectionModel, role2Name);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(role2Mock.Object));
        }

        [TestCase("NAME1")]
        [TestCase("NaME1")]
        [TestCase("NAMe1")]
        [TestCase("name1")]
        [TestCase("Name1")]
        public void Given_I_Call_GetRole_Then_The_Correct_Role_Is_Returned_Regardless_Of_Case(string name)
        {
            // arrange
            const string role1Name = "Name1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);

            var roles = new List<IRole> { role1Mock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            var result = _sut.GetRole(_connectionModel, name);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(role1Mock.Object));
        }

        [Test]
        public void Given_The_Role_Does_Not_Exist_When_I_Call_GetRole_Then_A_DataNotFoundException_Is_Thrown()
        {
            // arrange
            var roleMock = new Mock<IRole>();
            roleMock.Setup(r => r.Name).Returns("Found");
            var roles = new List<IRole> { roleMock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetRole(_connectionModel, "NotFound"));
            Assert.That(exception.Message, Is.EqualTo("The role 'NotFound' was not found"));
        }

        [TestCase("NAME1")]
        [TestCase("NaME1")]
        [TestCase("NAMe1")]
        [TestCase("name1")]
        [TestCase("Name1")]
        public void Given_A_Role_Exists_When_I_Call_RoleExists_Then_True_Is_Returned(string name)
        {
            // arrange
            const string role1Name = "Name1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);

            var roles = new List<IRole> { role1Mock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            var result = _sut.RoleExists(_connectionModel, name);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_A_Role_Does_Not_Exist_When_I_Call_RoleExists_Then_False_Is_Returned()
        {
            // arrange
            var roleMock = new Mock<IRole>();
            roleMock.Setup(r => r.Name).Returns("Found");
            var roles = new List<IRole> { roleMock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            var result = _sut.RoleExists(_connectionModel, "NotFound");

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_I_Call_AddRole_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string name = "Admin";
            const string description = "Test";
            var permissions = new List<string> { "Permission1" };
            var actionPermissions = new List<ActionPermissionModel> { new ActionPermissionModel() };

            const int roleId = 1;
            _securityServerMock.Setup(s => s.AddRole(name, description)).Returns(roleId);
            _mapperMock.Setup(m => m.MapToActionPermissionModels(permissions)).Returns(actionPermissions);

            // act
            _sut.AddRole(_connectionModel, name, description, permissions);

            // assert
            _securityServerMock.Verify(v => v.AddRole(name, description));
            _mapperMock.Verify(v => v.MapToActionPermissionModels(permissions));
            _securityServerMock.Verify(v => v.UpdateRolePermissions(roleId, actionPermissions));
        }

        [Test]
        public void Given_A_Role_Exists_When_I_Call_RemoveRole_Then_The_Role_Is_Removed()
        {
            // arrange
            const string role1Name = "Name1";
            const int role1Id = 123;
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Id).Returns(role1Id);

            var roles = new List<IRole> { role1Mock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act
            _sut.RemoveRole(_connectionModel, role1Name);

            // assert
            _securityServerMock.Verify(v => v.RemoveRole(role1Id), Times.Once);
        }

        [Test]
        public void Given_A_Role_Does_Not_Exist_When_I_Call_RemoveRole_Then_A_DataNotFoundException_Is_Thrown()
        {
            // arrange
            var roleMock = new Mock<IRole>();
            roleMock.Setup(r => r.Name).Returns("Found");
            var roles = new List<IRole> { roleMock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.RemoveRole(_connectionModel, "NotFound"));
            Assert.That(exception.Message, Is.EqualTo("The role 'NotFound' was not found"));
        }

        [Test]
        public void Given_A_Role_Exists_When_I_Call_UpdateRolePermissions_Then_The_Permissions_Of_The_Role_Are_Updated()
        {
            // arrange
            const string role1Name = "Name1";
            const int role1Id = 123;
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);
            role1Mock.Setup(r => r.Id).Returns(role1Id);

            var roles = new List<IRole> { role1Mock.Object };

            var permissions = new List<string> { "Permission1" };
            var actionPermissions = new List<ActionPermissionModel> { new ActionPermissionModel() };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);
            _mapperMock.Setup(m => m.MapToActionPermissionModels(permissions)).Returns(actionPermissions);

            // act
            _sut.UpdateRolePermissions(_connectionModel, role1Name, permissions);

            // assert
            _securityServerMock.Verify(v => v.GetRoles(), Times.Once);
            _mapperMock.Verify(v => v.MapToActionPermissionModels(permissions));
            _securityServerMock.Verify(v => v.UpdateRolePermissions(role1Id, actionPermissions));
            _securityServerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_A_Role_Does_Not_Exist_When_I_Call_UpdateRolePermissions_Then_A_DataNotFoundException_Is_Thrown()
        {
            // arrange
            const string role1Name = "Name1";
            var role1Mock = new Mock<IRole>();
            role1Mock.Setup(r => r.Name).Returns(role1Name);

            var roles = new List<IRole> { role1Mock.Object };
            var permissions = new List<string> { "Permission1" };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.UpdateRolePermissions(_connectionModel, "NotFound", permissions));
            Assert.That(exception.Message, Is.EqualTo("The role 'NotFound' was not found"));
        }
    }
}
