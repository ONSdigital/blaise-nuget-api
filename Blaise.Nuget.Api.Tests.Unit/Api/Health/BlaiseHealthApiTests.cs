namespace Blaise.Nuget.Api.Tests.Unit.Api.Health
{
    using System;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.Cati.Runtime;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.ServerManager;

    public class BlaiseHealthApiTests
    {
        private Mock<IConnectedServerFactory> _connectionFactoryMock;

        private Mock<IRemoteDataServerFactory> _remoteConnectionFactoryMock;

        private Mock<ICatiManagementServerFactory> _catiManagementFactoryMock;

        private ConnectionModel _connectionModel;

        private Mock<IConnectedServer> _connectedServerMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;

        private Mock<IRemoteCatiManagementServer> _remoteCatiManagementServerMock;

        private IBlaiseHealthApi _sut;

        [SetUp]
        public void SetUpTests()
        {
            _connectionModel = new ConnectionModel
            {
                ServerName = "ServerA",
                UserName = "TestUser",
                Password = "TestPassword",
                Binding = "http",
                Port = 1,
                RemotePort = 2,
                ConnectionExpiresInMinutes = 90,
            };

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _remoteConnectionFactoryMock = new Mock<IRemoteDataServerFactory>();
            _catiManagementFactoryMock = new Mock<ICatiManagementServerFactory>();

            _sut = new BlaiseHealthApi(
                _connectionFactoryMock.Object,
                _remoteConnectionFactoryMock.Object,
                _catiManagementFactoryMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseHealthApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            Assert.That(() => new BlaiseHealthApi(), Throws.Nothing);
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseHealthApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            Assert.That(() => new BlaiseHealthApi(new ConnectionModel()), Throws.Nothing);
        }

        [Test]
        public void Given_A_Valid_ConnectionModel_When_I_Call_ConnectionModelIsHealthy_Then_True_Is_Returned()
        {
            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_An_Invalid_ServerName_When_I_Call_ConnectionModelIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _connectionModel.ServerName = string.Empty;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_An_Invalid_UserName_When_I_Call_ConnectionModelIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _connectionModel.UserName = string.Empty;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_An_Invalid_Password_When_I_Call_ConnectionModelIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _connectionModel.Password = string.Empty;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_An_Invalid_Binding_When_I_Call_ConnectionModelIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _connectionModel.Binding = string.Empty;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_An_Invalid_Port_When_I_Call_ConnectionModelIsHealthy_Then_False_Returned()
        {
            // arrange
            _connectionModel.Port = 0;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_An_Invalid_RemotePort_When_I_Call_ConnectionModelIsHealthy_Then_False_Returned()
        {
            // arrange
            _connectionModel.RemotePort = 0;

            // act
            var result = _sut.ConnectionModelIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_Blaise_Connection_Is_Up_When_I_Call_ConnectionToBlaiseIsHealthy_Then_True_Is_Returned()
        {
            // arrange
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_connectedServerMock.Object);

            // act
            var result = _sut.ConnectionToBlaiseIsHealthy();

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Blaise_Connection_Is_Down_When_I_Call_ConnectionToBlaiseIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            // act
            var result = _sut.ConnectionToBlaiseIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_Blaise_Remote_Connection_Is_Up_When_I_Call_RemoteConnectionToBlaiseIsHealthy_Then_True_Is_Returned()
        {
            // arrange
            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteConnectionFactoryMock.Setup(r => r.GetConnection(_connectionModel))
                .Returns(_remoteDataServerMock.Object);

            // act
            var result = _sut.RemoteConnectionToBlaiseIsHealthy();

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Blaise_Remote_Connection_Is_Down_When_I_Call_RemoteConnectionToBlaiseIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _remoteConnectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            // act
            var result = _sut.RemoteConnectionToBlaiseIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_Blaise_Remote_Cati_Connection_Is_Up_When_I_Call_RemoteConnectionToCatiIsHealthy_Then_True_Is_Returned()
        {
            // arrange
            _remoteCatiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementFactoryMock.Setup(c => c.GetConnection(_connectionModel))
                .Returns(_remoteCatiManagementServerMock.Object);

            // act
            var result = _sut.RemoteConnectionToCatiIsHealthy();

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Blaise_Remote_Cati_Connection_Is_Down_When_I_Call_RemoteConnectionToCatiIsHealthy_Then_False_Is_Returned()
        {
            // arrange
            _catiManagementFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Throws(new Exception());

            // act
            var result = _sut.RemoteConnectionToCatiIsHealthy();

            // assert
            Assert.That(result, Is.False);
        }
    }
}
