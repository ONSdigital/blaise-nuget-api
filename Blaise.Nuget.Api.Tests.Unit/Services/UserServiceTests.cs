namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.ServerManager;

    public class UserServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _serverParkName;
        private readonly string _userName;
        private readonly string _password;
        private readonly SecureString _securePassword;
        private Mock<IConnectedServerFactory> _connectionFactoryMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private Mock<IServerPark> _serverParkMock;
        private Mock<IConnectedServer> _connectedServerMock;
        private Mock<IServerParkCollection> _serverParkCollectionMock;
        private Mock<IUser> _userMock;
        private Mock<IUserServerParkCollection> _userServerParkCollectionMock;
        private Mock<IUserCollection> _userCollectionMock;
        private Mock<IUserPreferenceCollection> _userPreferenceCollectionMock;
        private Mock<IUserPreference> _userPreferenceMock;
        private UserService _sut;

        public UserServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "TestServerParkName";
            _userName = "User1";
            _password = "Password1";
            _securePassword = new SecureString();
        }

        [SetUp]
        public void SetUpTests()
        {
            // setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");

            var serverParkItems = new List<IServerPark> { _serverParkMock.Object };

            // mock user preference
            _userPreferenceMock = new Mock<IUserPreference>();
            _userPreferenceMock.Setup(s => s.Type).Returns("TestDefaultServerParkType");
            _userPreferenceMock.Setup(s => s.Value).Returns("TestDefaultServerParkValue");

            // mock user preference
            _userPreferenceCollectionMock = new Mock<IUserPreferenceCollection>();
            _userPreferenceCollectionMock.Setup(s => s.Add(It.IsAny<string>())).Returns(() => _userPreferenceMock.Object);
            _userPreferenceCollectionMock.Setup(s => s.GetItem(It.IsAny<string>())).Returns(() => _userPreferenceMock.Object);

            var userPreferenceItems = new List<IUserPreference> { _userPreferenceMock.Object };
            _userPreferenceCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => userPreferenceItems.GetEnumerator());

            _serverParkCollectionMock = new Mock<IServerParkCollection>();
            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            // setup user mocks
            _userServerParkCollectionMock = new Mock<IUserServerParkCollection>();

            _userMock = new Mock<IUser>();
            _userMock.As<IUser2>();
            _userMock.Setup(u => u.Name).Returns(_userName);

            _userMock.As<IUser2>().Setup(u => u.ServerParks).Returns(_userServerParkCollectionMock.Object);
            _userMock.As<IUser2>().Setup(u => u.Preferences).Returns(_userPreferenceCollectionMock.Object);

            var userItems = new List<IUser> { _userMock.Object };

            _userCollectionMock = new Mock<IUserCollection>();
            _userCollectionMock.Setup(u => u.GetEnumerator()).Returns(() => userItems.GetEnumerator());
            _userCollectionMock.Setup(u => u.GetItem(It.IsAny<string>())).Returns(_userMock.Object);

            // setup connection
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectedServerMock.Setup(c => c.ServerParks).Returns(_serverParkCollectionMock.Object);
            _connectedServerMock.Setup(c => c.GetServerPark(_serverParkName)).Returns(_serverParkMock.Object);
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_connectedServerMock.Object);
            _connectedServerMock.Setup(c => c.AddUser(It.IsAny<string>(), It.IsAny<SecureString>()))
                .Returns(_userMock.Object);

            // setup password service
            _passwordServiceMock = new Mock<IPasswordService>();
            _passwordServiceMock.Setup(p => p.CreateSecurePassword(It.IsAny<string>())).Returns(_securePassword);

            // setup service under test
            _sut = new UserService(_connectionFactoryMock.Object, _passwordServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetUsers_Then_A_List_Of_IUser_Objects_Are_Returned()
        {
            // arrange
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            // act
            var result = _sut.GetUsers(_connectionModel);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<IUser>>());
        }

        [Test]
        public void Given_Users_Exist_When_I_Call_GetUsers_Then_The_Correct_List_Of_Users_Are_returned()
        {
            // arrange
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            // act
            var result = _sut.GetUsers(_connectionModel);

            // assert
            Assert.That(result, Is.SameAs(_userCollectionMock.Object));
        }

        [Test]
        public void Given_I_Call_GetUser_Then_An_IUser_Object_Is_Returned()
        {
            // arrange
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            // act
            var result = _sut.GetUser(_connectionModel, _userName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IUser>());
        }

        [Test]
        public void Given_A_User_Exists_When_I_Call_GetUser_Then_The_Correct_User_Is_Returned()
        {
            // arrange
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            // act
            var result = _sut.GetUser(_connectionModel, _userName);

            // assert
            Assert.That(result.Name, Is.EqualTo(_userName));
        }

        [TestCase("User1")]
        [TestCase("USER1")]
        [TestCase("user1")]
        [TestCase("uSeR1")]
        public void Given_A_User_Exists_When_I_Call_UserExists_Then_True_Is_Returned(string userName)
        {
            // arrange
            _userMock.Setup(u => u.Name).Returns(_userName);

            // act
            var result = _sut.UserExists(_connectionModel, userName);

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_A_User_Does_Not_Exist_When_I_Call_UserExists_Then_False_Is_Returned()
        {
            // arrange
            _userMock.Setup(u => u.Name).Returns("NotFound");

            // act
            var result = _sut.UserExists(_connectionModel, _userName);

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddUser_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";
            const string role = "King";

            // act
            _sut.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, defaultServerPark);

            // assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.AddUser(_userName, _securePassword), Times.Once);

            _userMock.As<IUser2>().VerifySet(u => u.Role = role, Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userPreferenceCollectionMock.Verify(v => v.Add("CATI.Preferences"), Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_An_Error_Occurs_In_Setting_The_User_Role_When_I_Call_AddUser_Then_The_User_Is_Still_Added()
        {
            // arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string role = "King";
            const string defaultServerPark = "ServerPark1";

            _userMock.As<IUser2>().Setup(u => u.Role).Throws(new Exception());

            // act
            _sut.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, defaultServerPark);

            // assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.AddUser(_userName, _securePassword), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Give_Valid_Parameters_When_I_Call_UpdatePassword_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.UpdatePassword(_connectionModel, _userName, _password);

            // assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);
            _userMock.As<IUser2>().Verify(v => v.ChangePassword(_securePassword), Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateRole_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string role = "King";

            // act
            _sut.UpdateRole(_connectionModel, _userName, role);

            // assert
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);

            _userMock.As<IUser2>().VerifySet(u => u.Role = role, Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateServerParks_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark2";

            // act
            _sut.UpdateServerParks(_connectionModel, _userName, serverParkNameList, defaultServerPark);

            // assert
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);

            _userMock.Verify(u => u.ServerParks.Clear(), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userPreferenceCollectionMock.Verify(v => v.Add("CATI.Preferences"), Times.Once);

            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_When_I_Call_UpdateServerParks_Twice_Then_The_Add_Preferences_Is_Only_Called_Once()
        {
            // Mock user preference
            _userPreferenceMock = new Mock<IUserPreference>();
            _userPreferenceMock.Setup(s => s.Type).Returns("CATI.Preferences");
            _userPreferenceMock.Setup(s => s.Value).Returns($"<CatiDashboard><ServerPark>ServerPark1</ServerPark></CatiDashboard>");

            var userPreferenceItems = new List<IUserPreference> { _userPreferenceMock.Object };
            _userPreferenceCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => userPreferenceItems.GetEnumerator());

            // arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2"
            };

            const string defaultServerPark = "ServerPark1";

            // act
            _sut.UpdateServerParks(_connectionModel, _userName, serverParkNameList, defaultServerPark);

            // assert
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);

            _userMock.Verify(u => u.ServerParks.Clear(), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userPreferenceCollectionMock.Verify(v => v.Add("CATI.Preferences"), Times.Never);

            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveUser_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.RemoveUser(_connectionModel, _userName);

            // assert
            _connectedServerMock.Verify(v => v.RemoveUser(_userName), Times.Once);
        }

        [Test]
        public void Given_A_Valid_User_When_I_Call_ValidateUser_Then_True_Is_Returned()
        {
            // act
            var result = _sut.ValidateUser(_connectionModel, _userName, _password);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_An_Invalid_User_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            // arrange
            _connectionFactoryMock.SetupSequence(c =>
                    c.GetIsolatedConnection(It.IsAny<ConnectionModel>()))
                .Throws(new Exception())
                .Returns(_connectedServerMock.Object);

            // act
            var result = _sut.ValidateUser(_connectionModel, _userName, _password);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.False);
        }
    }
}
