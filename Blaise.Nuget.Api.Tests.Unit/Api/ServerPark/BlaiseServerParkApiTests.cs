namespace Blaise.Nuget.Api.Tests.Unit.Api.ServerPark
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.ServerManager;

    public class BlaiseServerParkApiTests
    {
        private Mock<IServerParkService> _parkServiceMock;

        private ConnectionModel _connectionModel;

        private string _serverParkName;

        private IBlaiseServerParkApi _sut;

        [SetUp]
        public void SetUpTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "serverParkName";

            _parkServiceMock = new Mock<IServerParkService>();
            _sut = new BlaiseServerParkApi(
                _parkServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            Assert.That(() => new BlaiseServerParkApi(), Throws.Nothing);
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            Assert.That(() => new BlaiseServerParkApi(new ConnectionModel()), Throws.Nothing);
        }

        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, _serverParkName)).Returns(serverParkMock.Object);

            // act
            _sut.GetServerPark(_serverParkName);

            // assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_ServerPark_Is_Returned()
        {
            // arrange
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, _serverParkName)).Returns(serverParkMock.Object);

            // act
            var result = _sut.GetServerPark(_serverParkName);

            // assert
            Assert.That(result, Is.SameAs(serverParkMock.Object));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetServerPark(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerPark(null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            // act
            _sut.GetServerParks();

            // assert
            _parkServiceMock.Verify(v => v.GetServerParks(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_ServerPark_Is_Returned()
        {
            // arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            // act
            var result = _sut.GetServerParks();

            // assert
            Assert.That(result, Is.SameAs(serverParkItems));
        }

        [Test]
        public void When_I_Call_GetNamesOfServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(It.IsAny<List<string>>());

            // act
            _sut.GetNamesOfServerParks();

            // assert
            _parkServiceMock.Verify(v => v.GetServerParkNames(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetNamesOfServerParks_Then_The_Expected_Server_Park_Names_Are_Returned()
        {
            // arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };

            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(serverParksNames);

            // act
            var result = _sut.GetNamesOfServerParks();

            // assert
            Assert.That(result, Is.SameAs(serverParksNames));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.ServerParkExists(_serverParkName);

            // assert
            _parkServiceMock.Verify(v => v.ServerParkExists(_connectionModel, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            // arrange
            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, _serverParkName)).Returns(serverParkExists);

            // act
            var result = _sut.ServerParkExists(_serverParkName);

            // assert
            Assert.That(result, Is.EqualTo(serverParkExists));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ServerParkExists(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }
    }
}
