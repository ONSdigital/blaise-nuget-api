namespace Blaise.Nuget.Api.Tests.Unit.Api.Cati
{
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class BlaiseCatiApiTests
    {
        private Mock<ICatiService> _catiServiceMock;

        private Mock<ICaseService> _caseServiceMock;

        private readonly string _serverParkName;

        private readonly string _questionnaireName;

        private readonly string _primaryKeyValue;

        private readonly ConnectionModel _connectionModel;

        private IBlaiseCatiApi _sut;

        public BlaiseCatiApiTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _questionnaireName = "Questionnaire1";
            _primaryKeyValue = "90001";
        }

        [SetUp]
        public void SetUpTests()
        {
            _catiServiceMock = new Mock<ICatiService>();
            _caseServiceMock = new Mock<ICaseService>();

            _sut = new BlaiseCatiApi(_catiServiceMock.Object, _caseServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstalledQuestionnaires_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.GetInstalledQuestionnaires(_serverParkName);

            // assert
            _catiServiceMock.Verify(v => v.GetInstalledQuestionnaires(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstalledQuestionnaires_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledQuestionnaires(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstalledQuestionnaires_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledQuestionnaires(null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstalledQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.GetInstalledQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            _catiServiceMock.Verify(v => v.GetInstalledQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetInstalledQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledQuestionnaire(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetInstalledQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledQuestionnaire(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstalledQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledQuestionnaire(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstalledQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledQuestionnaire(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CreateDayBatch_Then_The_Correct_Service_Method_Is_Called(bool checkForTreatedCases)
        {
            // arrange
            var dayBatchDate = DateTime.Now;
            _caseServiceMock.Setup(c => c.GetNumberOfCases(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(1);

            // act
            _sut.CreateDayBatch(_questionnaireName, _serverParkName, dayBatchDate, checkForTreatedCases);

            // assert
            _catiServiceMock.Verify(
                v => v.CreateDayBatch(_connectionModel, _questionnaireName,
                _serverParkName, dayBatchDate, checkForTreatedCases), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CreateDayBatch_Then_A_DayBatchModel_Is_Returned(bool checkForTreatedCases)
        {
            // arrange
            var dayBatchDate = DateTime.Now;
            _catiServiceMock.Setup(cs =>
                    cs.CreateDayBatch(_connectionModel, _questionnaireName, _serverParkName, dayBatchDate, checkForTreatedCases))
                .Returns(new DayBatchModel());

            _caseServiceMock.Setup(c => c.GetNumberOfCases(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(1);

            // act
            var result = _sut.CreateDayBatch(_questionnaireName, _serverParkName, dayBatchDate, checkForTreatedCases);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DayBatchModel>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_Questionnaire_Has_No_Cases_When_I_Call_CreateDayBatch_Then_A_DataNotFoundException_Is_Thrown(bool checkForTreatedCases)
        {
            // arrange
            var dayBatchDate = DateTime.Now;

            _caseServiceMock.Setup(c => c.GetNumberOfCases(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(0);

            // act and assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.CreateDayBatch(_questionnaireName, _serverParkName, dayBatchDate, checkForTreatedCases));
            Assert.That(exception.Message, Is.EqualTo($"There are no cases available in '{_questionnaireName}' to create a daybatch"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dayBatchDate = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(string.Empty, _serverParkName, dayBatchDate, false));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dayBatchDate = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(null, _serverParkName, dayBatchDate, false));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dayBatchDate = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(_questionnaireName, string.Empty, dayBatchDate, false));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dayBatchDate = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(_questionnaireName, null, dayBatchDate, false));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDayBatch_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.GetDayBatch(_questionnaireName, _serverParkName);

            // assert
            _catiServiceMock.Verify(
                v => v.GetDayBatch(_connectionModel, _questionnaireName,
                _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDayBatch(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDayBatch(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDayBatch(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDayBatch(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddToDayBatch_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.AddToDayBatch(_questionnaireName, _serverParkName, _primaryKeyValue);

            // assert
            _catiServiceMock.Verify(
                v => v.AddToDayBatch(
                    _connectionModel,
                    _questionnaireName,
                    _serverParkName,
                    _primaryKeyValue),
                Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_AddToDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddToDayBatch(string.Empty, _serverParkName, _primaryKeyValue));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_AddToDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddToDayBatch(null, _serverParkName, _primaryKeyValue));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_AddToDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddToDayBatch(_questionnaireName, string.Empty, _primaryKeyValue));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_AddToDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddToDayBatch(_questionnaireName, null, _primaryKeyValue));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_AddToDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddToDayBatch(_questionnaireName, _serverParkName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'primaryKeyValue' must be supplied"));
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_AddToDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddToDayBatch(_questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("primaryKeyValue"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_SetSurveyDay_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act
            _sut.SetSurveyDay(_questionnaireName, _serverParkName, surveyDay);

            // assert
            _catiServiceMock.Verify(v => v.SetSurveyDay(_connectionModel, _questionnaireName, _serverParkName, surveyDay), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_SetSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDay(string.Empty, _serverParkName, surveyDay));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_SetSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDay(null, _serverParkName, surveyDay));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_SetSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDay(_questionnaireName, string.Empty, surveyDay));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_SetSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDay(_questionnaireName, null, surveyDay));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_SetSurveyDays_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act
            _sut.SetSurveyDays(_questionnaireName, _serverParkName, surveyDays);

            // assert
            _catiServiceMock.Verify(v => v.SetSurveyDays(_connectionModel, _questionnaireName, _serverParkName, surveyDays), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_SetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDays(string.Empty, _serverParkName, surveyDays));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_SetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDays(null, _serverParkName, surveyDays));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_SetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDays(_questionnaireName, string.Empty, surveyDays));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_SetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDays(_questionnaireName, null, surveyDays));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_List_Of_SurveyDays_When_I_Call_SetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDays(_questionnaireName, _serverParkName, new List<DateTime>()));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'surveyDays' must be supplied"));
        }

        [Test]
        public void Given_A_Null_List_Of_SurveyDays_When_I_Call_SetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDays(_questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("surveyDays"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyDays_Then_The_Expected_Result_Is_Returned()
        {
            var surveyDays = new List<DateTime>
            {
                new DateTime(2020, 12, 10),
                new DateTime(2020, 12, 11)
            };

            // arrange
            _catiServiceMock.Setup(p => p.GetSurveyDays(_connectionModel, _questionnaireName, _serverParkName)).Returns(surveyDays);

            // act
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DateTime>>());
            Assert.That(result, Is.EqualTo(surveyDays));
        }

        [Test]
        public void Given_There_Are_No_SurveyDays_In_The_Questionnaire_When_I_Call_GetSurveyDays_Then_A_Blank_List_Is_Returned()
        {
            var surveyDays = new List<DateTime>();

            // arrange
            _catiServiceMock.Setup(p => p.GetSurveyDays(_connectionModel, _questionnaireName, _serverParkName)).Returns(surveyDays);

            // act
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DateTime>>());
            Assert.That(result, Is.EqualTo(surveyDays));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyDays(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyDays(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyDays(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyDays(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveSurveyDay_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act
            _sut.RemoveSurveyDay(_questionnaireName, _serverParkName, surveyDay);

            // assert
            _catiServiceMock.Verify(v => v.RemoveSurveyDay(_connectionModel, _questionnaireName, _serverParkName, surveyDay), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDay(string.Empty, _serverParkName, surveyDay));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDay(null, _serverParkName, surveyDay));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDay(_questionnaireName, string.Empty, surveyDay));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDay = DateTime.Now;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDay(_questionnaireName, null, surveyDay));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveSurveyDays_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act
            _sut.RemoveSurveyDays(_questionnaireName, _serverParkName, surveyDays);

            // assert
            _catiServiceMock.Verify(v => v.RemoveSurveyDays(_connectionModel, _questionnaireName, _serverParkName, surveyDays), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDays(string.Empty, _serverParkName, surveyDays));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDays(null, _serverParkName, surveyDays));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDays(_questionnaireName, string.Empty, surveyDays));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDays(_questionnaireName, null, surveyDays));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_List_Of_SurveyDays_When_I_Call_RemoveSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDays(_questionnaireName, _serverParkName, new List<DateTime>()));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'surveyDays' must be supplied"));
        }

        [Test]
        public void Given_A_Null_List_Of_SurveyDays_When_I_Call_RemoveSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDays(_questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("surveyDays"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_MakeSuperAppointment_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.MakeSuperAppointment(_questionnaireName, _serverParkName, _primaryKeyValue);

            // assert
            _catiServiceMock.Verify(
                v => v.MakeSuperAppointment(
                    _connectionModel,
                    _questionnaireName,
                    _serverParkName,
                    _primaryKeyValue),
                Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_MakeSuperAppointment_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MakeSuperAppointment(string.Empty, _serverParkName, _primaryKeyValue));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_MakeSuperAppointment_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MakeSuperAppointment(null, _serverParkName, _primaryKeyValue));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_MakeSuperAppointment_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MakeSuperAppointment(_questionnaireName, string.Empty, _primaryKeyValue));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_MakeSuperAppointment_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MakeSuperAppointment(_questionnaireName, null, _primaryKeyValue));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_MakeSuperAppointment_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MakeSuperAppointment(_questionnaireName, _serverParkName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'primaryKeyValue' must be supplied"));
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_MakeSuperAppointment_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MakeSuperAppointment(_questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("primaryKeyValue"));
        }
    }
}
