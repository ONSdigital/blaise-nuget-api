namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.Cati.Runtime;
    using StatNeth.Blaise.API.Cati.Specification;
    using StatNeth.Blaise.API.ServerManager;

    public class CatiServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly Guid _questionnaireId;
        private readonly DateTime _surveyDay;
        private Mock<IRemoteCatiManagementServerProvider> _catiProviderMock;
        private Mock<IQuestionnaireService> _questionnaireServiceMock;
        private Mock<ICatiInstrumentManager> _catiQuestionnaireManagerMock;
        private Mock<IRemoteCatiManagementServer> _catiManagementServerMock;
        private Mock<ISurveyDayCollection> _surveyDayCollection;
        private CatiService _sut;

        public CatiServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _questionnaireId = Guid.NewGuid();

            _surveyDay = DateTime.Today;
        }

        [SetUp]
        public void SetUpTests()
        {
            var surveyDayMock = new Mock<ISurveyDay>();
            surveyDayMock.Setup(d => d.Date).Returns(_surveyDay);
            var surveyDays = new List<ISurveyDay> { surveyDayMock.Object };

            _surveyDayCollection = new Mock<ISurveyDayCollection>();
            _surveyDayCollection.Setup(s => s.GetEnumerator()).Returns(surveyDays.GetEnumerator());

            _catiQuestionnaireManagerMock = new Mock<ICatiInstrumentManager>();
            _catiQuestionnaireManagerMock.Setup(cim => cim.CreateDaybatch(It.IsAny<DateTime>()));
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Setup(cim => cim.AddSurveyDays(It.IsAny<List<DateTime>>()));
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Setup(cim => cim.RemoveSurveyDays(It.IsAny<List<DateTime>>()));
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager3>().Setup(cim => cim.CreateDaybatch(It.IsAny<DateTime>(), It.IsAny<bool>()));
            _catiQuestionnaireManagerMock.Setup(cim => cim.Specification.SurveyDays).Returns(_surveyDayCollection.Object);

            _catiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(
                It.IsAny<Guid>())).Returns(_catiQuestionnaireManagerMock.Object);

            _catiProviderMock = new Mock<IRemoteCatiManagementServerProvider>();
            _catiProviderMock.Setup(r => r.GetCatiManagementForServerPark(_connectionModel, _serverParkName))
                .Returns(_catiManagementServerMock.Object);

            _questionnaireServiceMock = new Mock<IQuestionnaireService>();
            _questionnaireServiceMock.Setup(ss => ss.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(_questionnaireId);

            // setup service under test
            _sut = new CatiService(_catiProviderMock.Object, _questionnaireServiceMock.Object);
        }

        [Test]
        public void Given_Questionnaires_Are_Installed_When_I_Call_GetInstalledQuestionnaires_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string questionnaire1 = "OPN2004a";
            const string questionnaire2 = "OPN2010a";
            var installedQuestionnaires = new Dictionary<string, Guid>
            {
                { questionnaire1, Guid.NewGuid() },
                { questionnaire2, Guid.NewGuid() },
            };

            var questionnaireMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);
            _questionnaireServiceMock.Setup(s => s.GetQuestionnaire(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(questionnaireMock.Object);

            // act
            _sut.GetInstalledQuestionnaires(_connectionModel, _serverParkName);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _questionnaireServiceMock.Verify(v => v.GetQuestionnaire(_connectionModel, questionnaire1, _serverParkName), Times.Once);
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaire(_connectionModel, questionnaire2, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Questionnaires_Are_Installed_When_I_Call_GetInstalledQuestionnaires_Then_An_Correct_List_Is_Returned()
        {
            // arrange
            const string questionnaire1 = "OPN2004a";
            const string questionnaire2 = "OPN2010a";
            var installedQuestionnaires = new Dictionary<string, Guid>
            {
                { questionnaire1, Guid.NewGuid() },
                { questionnaire2, Guid.NewGuid() },
            };

            var questionnaireMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);
            _questionnaireServiceMock.Setup(s => s.GetQuestionnaire(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(questionnaireMock.Object);

            // act
            var result = _sut.GetInstalledQuestionnaires(_connectionModel, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ISurvey>>());
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Given_No_Questionnaires_Are_Installed_When_I_Call_GetInstalledQuestionnaires_Then_An_Empty_List_Is_Returned()
        {
            // arrange
            var installedQuestionnaires = new Dictionary<string, Guid>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);

            // act
            var result = _sut.GetInstalledQuestionnaires(_connectionModel, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ISurvey>>());
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Given_A_Questionnaires_Is_Installed_When_I_Call_GetInstalledQuestionnaire_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string questionnaire1 = "OPN2004a";
            const string questionnaire2 = "OPN2010a";
            var installedQuestionnaires = new Dictionary<string, Guid>
            {
                { questionnaire1, Guid.NewGuid() },
                { questionnaire2, Guid.NewGuid() },
                { _questionnaireName, Guid.NewGuid() },
            };

            var questionnaireMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);
            _questionnaireServiceMock.Setup(s => s.GetQuestionnaire(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(questionnaireMock.Object);

            // act
            _sut.GetInstalledQuestionnaire(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _questionnaireServiceMock.Verify(v => v.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _questionnaireServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_A_Questionnaire_Is_Installed_When_I_Call_GetInstalledQuestionnaire_Then_The_Correct_Questionnaire_Is_Returned()
        {
            // arrange
            const string questionnaire1 = "OPN2004a";
            const string questionnaire2 = "OPN2010a";
            var installedQuestionnaires = new Dictionary<string, Guid>
            {
                { questionnaire1, Guid.NewGuid() },
                { questionnaire2, Guid.NewGuid() },
                { _questionnaireName, Guid.NewGuid() },
            };

            var questionnaireMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);
            _questionnaireServiceMock.Setup(s => s.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(questionnaireMock.Object);

            // act
            var result = _sut.GetInstalledQuestionnaire(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<ISurvey>());
            Assert.That(result, Is.SameAs(questionnaireMock.Object));
        }

        [Test]
        public void Given_A_Questionnaire_Is_Not_Installed_When_I_Call_GetInstalledQuestionnaire_Then_A_DataNotFound_Exception_Is_Thrown()
        {
            // arrange
            const string questionnaire1 = "OPN2004a";
            const string questionnaire2 = "OPN2010a";
            var installedQuestionnaires = new Dictionary<string, Guid>
            {
                { questionnaire1, Guid.NewGuid() },
                { questionnaire2, Guid.NewGuid() },
            };

            var questionnaireMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedQuestionnaires);
            _questionnaireServiceMock.Setup(s => s.GetQuestionnaire(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(questionnaireMock.Object);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetInstalledQuestionnaire(_connectionModel, _questionnaireName, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo($"No questionnaire called '{_questionnaireName}' was found on server park '{_serverParkName}'"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_Survey_Day_Exists_For_Day_Batch_Date_When_I_Call_CreateDayBatch_Then_The_Correct_Services_Are_Called(bool checkForTreatedCases)
        {
            // arrange
            var dayBatchDate = _surveyDay;
            var caseIds = new List<string> { "90001", "90002" };

            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_questionnaireId, null))
                .Returns(caseIds);
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_questionnaireId))
                .Returns(dayBatchDate);

            // act
            _sut.CreateDayBatch(_connectionModel, _questionnaireName, _serverParkName, dayBatchDate, checkForTreatedCases);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager3>().Verify(
                v => v
                .CreateDaybatch(dayBatchDate, checkForTreatedCases), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_Survey_Day_Does_Not_Exist_For_Day_Batch_Date_When_I_Call_CreateDayBatch_Then_A_DataNotFoundException_Is_Thrown(bool checkForTreatedCases)
        {
            // arrange
            var dayBatchDate = _surveyDay.AddDays(1);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.CreateDayBatch(
                _connectionModel,
                _questionnaireName,
                _serverParkName,
                dayBatchDate,
                checkForTreatedCases));
            Assert.That(exception.Message, Is.EqualTo($"A survey day does not exist for the required daybatch date '{dayBatchDate.Date}'"));
        }

        [Test]
        public void Given_A_Day_Batch_Exists_When_I_Call_GetDayBatch_Then_The_Correct_DayBatchModel_Is_Returned()
        {
            // arrange
            var dayBatchDate = _surveyDay;
            var caseIds = new List<string> { "90001", "90002" };

            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_questionnaireId, string.Empty))
                .Returns(caseIds);
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_questionnaireId))
                .Returns(dayBatchDate);

            // act
            var result = _sut.GetDayBatch(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<DayBatchModel>());
            Assert.That(result.DayBatchDate, Is.EqualTo(dayBatchDate));
            Assert.That(result.CaseIds, Is.EqualTo(caseIds));
        }

        [Test]
        public void Given_A_Day_Batch_Does_Not_Exist_When_I_Call_GetDayBatch_Then_Null_Is_Returned()
        {
            // arrange
            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_questionnaireId, string.Empty))
                .Returns(new List<string>());
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_questionnaireId))
                .Returns(null);

            // act
            var result = _sut.GetDayBatch(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Given_I_Call_SetSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.SetSurveyDay(_connectionModel, _questionnaireName, _serverParkName, DateTime.Today);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Verify(v => v.AddSurveyDays(new List<DateTime> { DateTime.Today }), Times.Once);
        }

        [Test]
        public void Given_I_Call_SetSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1),
            };

            // act
            _sut.SetSurveyDays(_connectionModel, _questionnaireName, _serverParkName, surveyDays);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Verify(v => v.AddSurveyDays(surveyDays), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetSurveyDays(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _catiManagementServerMock.Verify(
                v => v
                .LoadCatiInstrumentManager(_questionnaireId).Specification.SurveyDays, Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.RemoveSurveyDay(_connectionModel, _questionnaireName, _serverParkName, DateTime.Today);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Verify(v => v.RemoveSurveyDays(new List<DateTime> { DateTime.Today }), Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1),
            };

            // act
            _sut.RemoveSurveyDays(_connectionModel, _questionnaireName, _serverParkName, surveyDays);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiQuestionnaireManagerMock.As<ICatiInstrumentManager2>().Verify(v => v.RemoveSurveyDays(surveyDays), Times.Once);
        }

        [Test]
        public void Given_I_Call_MakeSuperAppointment_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            var primaryKeyValue = "900001";

            // act
            _sut.MakeSuperAppointment(_connectionModel, _questionnaireName, _serverParkName, primaryKeyValue);

            // assert
            _catiProviderMock.Verify(
                v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(
                v => v
                .LoadCatiInstrumentManager(_questionnaireId), Times.Once);
            _catiManagementServerMock.Verify(
                v => v
                .LoadCatiInstrumentManager(_questionnaireId).MakeSuperAppointment(primaryKeyValue), Times.Once);
        }
    }
}
