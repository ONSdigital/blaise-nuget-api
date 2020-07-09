﻿using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Providers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class RemoteDataLinkProviderTests
    {
        private Mock<IRemoteDataServerFactory> _connectionFactoryMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IConnectionExpiryService> _connectionExpiryServiceMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink4> _dataLinkMock;
        private Mock<IDatamodel> _dataModelMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private RemoteDataLinkProvider _sut;

        public RemoteDataLinkProviderTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _dataLinkMock = new Mock<IDataLink4>();
            _dataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_instrumentId, _serverParkName)).Returns(_dataLinkMock.Object);

            _connectionFactoryMock = new Mock<IRemoteDataServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_remoteDataServerMock.Object);

            _surveyServiceMock = new Mock<ISurveyService>();
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName)).Returns(_instrumentId);

            _connectionExpiryServiceMock = new Mock<IConnectionExpiryService>();
            _connectionExpiryServiceMock.Setup(c => c.ConnectionHasExpired()).Returns(false);

            _sut = new RemoteDataLinkProvider(
                _connectionFactoryMock.Object,
                _surveyServiceMock.Object,
                _connectionExpiryServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_The_Same_InstrumentName_And_ServerName_More_Than_Once_Then_The_Same_DataLink_Is_Used()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_The_Same_InstrumentName_And_ServerName_But_Connection_Has_expired_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);
            _connectionExpiryServiceMock.Setup(c => c.ConnectionHasExpired()).Returns(true);

            //act
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_InstrumentName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);
            _sut.GetDataLink(_connectionModel, "NewInstrumentName", _serverParkName);

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _instrumentName, "NewServerParkName");

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_InstrumentName_And_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataLink(_connectionModel, _instrumentName, _serverParkName);
            _sut.GetDataLink(_connectionModel, "NewInstrumentName", "NewServerParkName");

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
