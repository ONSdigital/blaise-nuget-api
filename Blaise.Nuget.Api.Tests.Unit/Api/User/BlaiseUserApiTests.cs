﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Api.User
{
    public class BlaiseUserApiTests
    {
        private Mock<IUserService> _userServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _userName;
        private readonly string _password;

        private IBlaiseUserApi _sut;

        public BlaiseUserApiTests()
        {
            _connectionModel = new ConnectionModel();
            _userName = "User1";
            _password = "Password1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _userServiceMock = new Mock<IUserService>();

            _sut = new BlaiseUserApi(
                _userServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseUserApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseUserApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseUserApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseUserApi(new ConnectionModel()));
        }

 [Test]
        public void When_I_Call_GetUsers_Then_The_Correct_List_Of_UserDto_Are_Returned()
        {
            //arrange
            var userMock = new Mock<IUser>();
            var userList = new List<IUser>{userMock.Object};

            _userServiceMock.Setup(u => u.GetUsers(_connectionModel)).Returns(userList);

            //act
            var result = _sut.GetUsers();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<IUser>>(result);
            Assert.AreSame(userList, result);
        }

        [Test]
        public void When_I_Call_GetUser_Then_The_Correct_UserDto_Is_Returned()
        {
            //arrange
            var userMock = new Mock<IUser>();

            _userServiceMock.Setup(u => u.GetUser(_connectionModel, _userName)).Returns(userMock.Object);

            //act
            var result = _sut.GetUser(_userName);

            //assert
           Assert.IsNotNull(result);
           Assert.IsInstanceOf<IUser>(result);
           Assert.AreSame(userMock.Object, result);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_GetUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetUser(string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_GetUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetUser(null));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_UserExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UserExists(_userName);

            //assert
            _userServiceMock.Verify(v => v.UserExists(_connectionModel, _userName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_I_Call_UserExists_Then_The_Expected_Result_Is_Returned(bool userExists)
        {
            //arrange
            _userServiceMock.Setup(u => u.UserExists(_connectionModel, _userName)).Returns(userExists);

            //act
            var result = _sut.UserExists(_userName);

            //assert
            Assert.AreEqual(userExists, result);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UserExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UserExists(string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UserExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UserExists(null));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_AddUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            //act
            _sut.AddUser(_userName, _password, role, serverParkNameList, defaultServerPark);

            //assert
            _userServiceMock.Verify(v => v.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, defaultServerPark), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(string.Empty, _password, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(null, _password, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, string.Empty, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, null, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("password", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, _password, string.Empty, serverParkNameList, defaultServerPark));
            Assert.AreEqual("A value for the argument 'role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, _password, null, serverParkNameList, defaultServerPark));
            Assert.AreEqual("role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DefaultServerPark_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, _password, role, serverParkNameList, string.Empty));
            Assert.AreEqual("A value for the argument 'DefaultServerPark' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DefaultServerPark_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string role = "King";
            
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, _password, role, serverParkNameList, null));
            Assert.AreEqual("DefaultServerPark", exception.ParamName);
        }

        [Test]
        public void When_I_Call_ChangePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UpdatePassword(_userName, _password);

            //assert
            _userServiceMock.Verify(v => v.UpdatePassword(_connectionModel, _userName, _password), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(string.Empty, _password));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UpdatePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(null, _password));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
             //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdatePassword(_userName, string.Empty));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Password_When_I_Call_UpdatePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdatePassword(_userName, null));
            Assert.AreEqual("password", exception.ParamName);
        }

        [Test]
        public void When_I_Call_UpdateRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            const string role = "King";

            //act
            _sut.UpdateRole(_userName, role);

            //assert
            _userServiceMock.Verify(v => v.UpdateRole(_connectionModel, _userName, role), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UpdateRole_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRole(string.Empty, role));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UpdateRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRole(null, role));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_UpdateRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateRole(_userName, string.Empty));
            Assert.AreEqual("A value for the argument 'role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Role_When_I_Call_UpdateRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateRole(_userName, null));
            Assert.AreEqual("role", exception.ParamName);
        }

        [Test]
        public void When_I_Call_UpdateServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            //act
            _sut.UpdateServerParks(_userName, serverParkNameList, defaultServerPark);

            //assert
           _userServiceMock.Verify(v => v.UpdateServerParks(_connectionModel, _userName, serverParkNameList, defaultServerPark), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UpdateServerParks_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateServerParks(string.Empty, serverParkNameList, defaultServerPark));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UpdateServerParks_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateServerParks(null, serverParkNameList, defaultServerPark));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DefaultServerPark_When_I_Call_UpdateServerParks_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateServerParks(_userName, serverParkNameList, string.Empty));
            Assert.AreEqual("A value for the argument 'defaultServerPark' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DefaultServerPark_When_I_Call_UpdateServerParks_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateServerParks(_userName, serverParkNameList, null));
            Assert.AreEqual("defaultServerPark", exception.ParamName);
        }

        [Test]
        public void When_I_Call_RemoveUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveUser(_userName);

            //assert
            _userServiceMock.Verify(v => v.RemoveUser(_connectionModel, _userName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_RemoveUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveUser(string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_RemoveUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveUser(null));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_ValidateUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.ValidateUser(_userName, _password);

            //assert
            _userServiceMock.Verify(v => v.ValidateUser(_connectionModel, _userName, _password), Times.Once);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_ValidateUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ValidateUser(string.Empty, _password));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_ValidateUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ValidateUser(null, _password));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_ValidateUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ValidateUser(_userName, string.Empty));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Password_When_I_Call_ValidateUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ValidateUser(_userName, null));
            Assert.AreEqual("password", exception.ParamName);
        }
    }
}
