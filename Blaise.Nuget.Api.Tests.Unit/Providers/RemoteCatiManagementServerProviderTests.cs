namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Providers;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.Cati.Runtime;

    public class RemoteCatiManagementServerProviderTests
    {
        private Mock<ICatiManagementServerFactory> _catiFactoryMock;

        private Mock<IRemoteCatiManagementServer> _remoteCatiManagementMock;

        private readonly ConnectionModel _connectionModel;

        private readonly string _serverParkName;

        private RemoteCatiManagementServerProvider _sut;

        public RemoteCatiManagementServerProviderTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _remoteCatiManagementMock = new Mock<IRemoteCatiManagementServer>();
            _remoteCatiManagementMock.Setup(r => r.SelectServerPark(_serverParkName));

            _catiFactoryMock = new Mock<ICatiManagementServerFactory>();
            _catiFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_remoteCatiManagementMock.Object);

            _sut = new RemoteCatiManagementServerProvider(_catiFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_GetCatiManagementForServerPark_Then_The_Correct_Methods_Are_Called()
        {
            // act
            _sut.GetCatiManagementForServerPark(_connectionModel, _serverParkName);

            // assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _remoteCatiManagementMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetCatiManagementForServerPark_Then_The_Correct_Connection_Is_Returned()
        {
            // act
            var result = _sut.GetCatiManagementForServerPark(_connectionModel, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IRemoteCatiManagementServer>());
            Assert.That(result, Is.SameAs(_remoteCatiManagementMock.Object));
        }
    }
}
