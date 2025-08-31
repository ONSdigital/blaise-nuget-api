namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.Meta;

    public class DataModelServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly string _databaseFile;
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;
        private Mock<ILocalDataLinkProvider> _localDataLinkProviderMock;
        private Mock<IDataLink6> _dataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
        private DataModelService _sut;

        public DataModelServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _databaseFile = "TestFilePath";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _dataLinkMock = new Mock<IDataLink6>();
            _dataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _questionnaireName, _serverParkName)).Returns(_dataLinkMock.Object);

            _localDataLinkProviderMock = new Mock<ILocalDataLinkProvider>();
            _localDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _databaseFile)).Returns(_dataLinkMock.Object);

            _sut = new DataModelService(_remoteDataLinkProviderMock.Object, _localDataLinkProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDataModel_I_Get_A_DataModel_Back()
        {
            // act
            var result = _sut.GetDataModel(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IDatamodel>());
        }

        [Test]
        public void Given_I_Call_GetDataModel_I_Get_The_Correct_DataModel_Back()
        {
            // act
            var result = _sut.GetDataModel(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.EqualTo(_dataModelMock.Object));
        }

        [Test]
        public void Given_No_DataModel_Available_When_I_Call_GetDataModel_A_NullReferenceException_Is_Thrown()
        {
            // arrange
            _dataLinkMock.Setup(d => d.Datamodel).Returns(null as IDatamodel);

            // act and assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel(_connectionModel, _questionnaireName, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo($"No datamodel was found for questionnaire '{_questionnaireName}' on server park '{_serverParkName}'"));
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataModel(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.AtLeastOnce);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_I_Get_A_DataModel_Back()
        {
            // act
            var result = _sut.GetDataModel(_connectionModel, _databaseFile);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IDatamodel>());
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_I_Get_The_Correct_DataModel_Back()
        {
            // act
            var result = _sut.GetDataModel(_connectionModel, _databaseFile);

            // assert
            Assert.That(result, Is.EqualTo(_dataModelMock.Object));
        }

        [Test]
        public void Given_No_DataModel_Available_When_I_Call_GetDataModel_For_Local_Connection_A_NullReferenceException_Is_Thrown()
        {
            // arrange
            _dataLinkMock.Setup(d => d.Datamodel).Returns(null as IDatamodel);

            // act and assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel(_connectionModel, _databaseFile));
            Assert.That(exception.Message, Is.EqualTo($"No datamodel was found for file '{_databaseFile}'"));
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataModel(_connectionModel, _databaseFile);

            // assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _databaseFile), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.AtLeastOnce);
        }
    }
}
