namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Providers;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.Meta;
    using System;
    using System.Threading;

    public class RemoteDataLinkProviderTests
    {
        private Mock<IRemoteDataServerFactory> _connectionFactoryMock;

        private Mock<IQuestionnaireService> _questionnaireServiceMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;

        private Mock<IDataLink4> _dataLinkMock;

        private Mock<IDatamodel> _dataModelMock;

        private readonly ConnectionModel _connectionModel;

        private readonly string _questionnaireName;

        private readonly string _serverParkName;

        private readonly DateTime _installDate;

        private readonly Guid _questionnaireId;

        private IRemoteDataLinkProvider _sut;

        public RemoteDataLinkProviderTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _installDate = DateTime.Today;
            _questionnaireId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _dataLinkMock = new Mock<IDataLink4>();
            _dataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_questionnaireId, _serverParkName)).Returns(_dataLinkMock.Object);

            _connectionFactoryMock = new Mock<IRemoteDataServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_remoteDataServerMock.Object);

            _questionnaireServiceMock = new Mock<IQuestionnaireService>();
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName)).Returns(_questionnaireId);
            _questionnaireServiceMock.Setup(s => s.GetInstallDate(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(_installDate);

            _sut = new RemoteDataLinkProvider(
                _connectionFactoryMock.Object,
                _questionnaireServiceMock.Object);
        }

        [Test]
        public void Given_InstallDate_Has_Not_Changed_When_I_Call_GetDataLink_With_The_Same_QuestionnaireName_And_ServerParkName_More_Than_Once_Then_The_Same_DataLink_Is_Used()
        {
            // arrange
            _connectionModel.ConnectionExpiresInMinutes = 1;
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Given_InstallDate_Has_Changed_I_Call_GetDataLink_With_The_Same_QuestionnaireName_And_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            // arrange
            _connectionModel.ConnectionExpiresInMinutes = 1;
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);
            _questionnaireServiceMock.SetupSequence(s => s.GetInstallDate(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(_installDate)
                .Returns(DateTime.Today.AddHours(1));

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_The_Same_QuestionnaireName_And_ServerParkName_But_Connection_Has_expired_Then_A_New_DataLink_Is_Established()
        {
            // arrange
            _connectionModel.ConnectionExpiresInMinutes = 0;
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            Thread.Sleep(2000);
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_QuestionnaireName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            _sut.GetDataLink(_connectionModel, "NewQuestionnaireName", _serverParkName);

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            _sut.GetDataLink(_connectionModel, _questionnaireName, "NewServerParkName");

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_GetDataLink_With_A_Different_QuestionnaireName_And_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            // act
            _sut.GetDataLink(_connectionModel, _questionnaireName, _serverParkName);
            _sut.GetDataLink(_connectionModel, "NewQuestionnaireName", "NewServerParkName");

            // assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
