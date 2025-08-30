namespace Blaise.Nuget.Api.Tests.Unit.Api.Questionnaire
{
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.ServerManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BlaiseQuestionnaireApiTests
    {
        private Mock<IQuestionnaireService> _questionnaireServiceMock;

        private Mock<IQuestionnaireMetaService> _questionnaireMetaServiceMock;

        private Mock<ICaseService> _caseServiceMock;

        private Mock<ISqlService> _sqlServiceMock;

        private Mock<IBlaiseConfigurationProvider> _configurationProviderMock;

        private readonly string _serverParkName;

        private readonly string _questionnaireName;

        private readonly ConnectionModel _connectionModel;

        private IBlaiseQuestionnaireApi _sut;

        public BlaiseQuestionnaireApiTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _questionnaireName = "Questionnaire1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _questionnaireServiceMock = new Mock<IQuestionnaireService>();
            _questionnaireMetaServiceMock = new Mock<IQuestionnaireMetaService>();
            _caseServiceMock = new Mock<ICaseService>();
            _sqlServiceMock = new Mock<ISqlService>();
            _configurationProviderMock = new Mock<IBlaiseConfigurationProvider>();

            _sut = new BlaiseQuestionnaireApi(
                _questionnaireServiceMock.Object,
                _questionnaireMetaServiceMock.Object,
                _caseServiceMock.Object,
                _connectionModel,
                _sqlServiceMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseQuestionnaireApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseQuestionnaireApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseQuestionnaireApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseQuestionnaireApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_QuestionnaireExists_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.QuestionnaireExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.QuestionnaireExists(_questionnaireName, _serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.QuestionnaireExists(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_QuestionnaireExists_Then_The_Expected_Result_Is_Returned(bool exists)
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.QuestionnaireExists(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(exists);

            // act
            var result = _sut.QuestionnaireExists(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exists));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_QuestionnaireExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.QuestionnaireExists(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_QuestionnaireExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.QuestionnaireExists(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_QuestionnaireExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.QuestionnaireExists(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_QuestionnaireExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.QuestionnaireExists(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void When_I_Call_GetQuestionnairesAcrossServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetAllQuestionnaires(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            // act
            _sut.GetQuestionnairesAcrossServerParks();

            // assert
            _questionnaireServiceMock.Verify(v => v.GetAllQuestionnaires(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetQuestionnairesAcrossServerParks_Then_The_Expected_Questionnaires_Are_Returned()
        {
            // arrange
            var questionnaire1Mock = new Mock<ISurvey>();
            var questionnaire2Mock = new Mock<ISurvey>();
            var questionnaire3Mock = new Mock<ISurvey>();

            var questionnaires = new List<ISurvey> { questionnaire1Mock.Object, questionnaire2Mock.Object, questionnaire3Mock.Object };

            _questionnaireServiceMock.Setup(p => p.GetAllQuestionnaires(_connectionModel)).Returns(questionnaires);

            // act
            var result = _sut.GetQuestionnairesAcrossServerParks().ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Does.Contain(questionnaire1Mock.Object));
            Assert.That(result, Does.Contain(questionnaire2Mock.Object));
            Assert.That(result, Does.Contain(questionnaire3Mock.Object));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaires_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaires(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            // act
            _sut.GetQuestionnaires(_serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaires(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaires_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            var questionnaire1Mock = new Mock<ISurvey>();
            var questionnaire2Mock = new Mock<ISurvey>();
            var questionnaireList = new List<ISurvey>
            {
                questionnaire1Mock.Object,
                questionnaire2Mock.Object
            };

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaires(_connectionModel, _serverParkName)).Returns(questionnaireList);

            // act
            var result = _sut.GetQuestionnaires(_serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Does.Contain(questionnaire1Mock.Object));
            Assert.That(result, Does.Contain(questionnaire2Mock.Object));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaires_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaires(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaires_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaires(null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<ISurvey>());

            // act
            _sut.GetQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaire_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            var questionnaire1Mock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaire1Mock.Object);

            // act
            var result = _sut.GetQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ISurvey>());
            Assert.That(result, Is.SameAs(questionnaire1Mock.Object));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaire(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaire(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaire(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaire(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireStatus_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireStatus(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<QuestionnaireStatusType>());

            // act
            _sut.GetQuestionnaireStatus(_questionnaireName, _serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaireStatus(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(QuestionnaireStatusType.Active)]
        [TestCase(QuestionnaireStatusType.Inactive)]
        [TestCase(QuestionnaireStatusType.Installing)]
        [TestCase(QuestionnaireStatusType.Erroneous)]
        [TestCase(QuestionnaireStatusType.Failed)]
        [TestCase(QuestionnaireStatusType.Other)]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireStatus_Then_The_Expected_Result_Is_Returned(QuestionnaireStatusType questionnaireStatusType)
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireStatus(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireStatusType);

            // act
            var result = _sut.GetQuestionnaireStatus(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<QuestionnaireStatusType>());
            Assert.That(result, Is.EqualTo(questionnaireStatusType));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireStatus(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireStatus(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireStatus(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireStatus(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireNames_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireNames(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            // act
            _sut.GetNamesOfQuestionnaires(_serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaireNames(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireNames_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            var questionnaireList = new List<string>
            {
                "Questionnaire",
                "Questionnaire"
            };

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireNames(_connectionModel, _serverParkName)).Returns(questionnaireList);

            // act
            var result = _sut.GetNamesOfQuestionnaires(_serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Does.Contain("Questionnaire"));
            Assert.That(result, Does.Contain("Questionnaire"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireNames_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNamesOfQuestionnaires(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNamesOfQuestionnaires_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNamesOfQuestionnaires(null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireId_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            // act
            _sut.GetIdOfQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireId_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            var questionnaireId = Guid.NewGuid();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireId);

            // act
            var result = _sut.GetIdOfQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(questionnaireId));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfQuestionnaire(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfQuestionnaire(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfQuestionnaire(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfQuestionnaire(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [TestCase(QuestionnaireInterviewType.Cati)]
        [TestCase(QuestionnaireInterviewType.Cawi)]
        [TestCase(QuestionnaireInterviewType.Capi)]
        public void Given_Valid_Arguments_When_I_Call_InstallQuestionnaire_Then_The_Correct_Service_Method_Is_Called(QuestionnaireInterviewType questionnaireInterviewType)
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";
            var installOptions = new InstallOptions();

            // act
            _sut.InstallQuestionnaire(_questionnaireName, _serverParkName, QuestionnaireFile, installOptions);

            // assert
            _questionnaireServiceMock.Verify(
                v => v.InstallQuestionnaire(_connectionModel, _questionnaireName,
                _serverParkName, QuestionnaireFile, installOptions), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(string.Empty, _serverParkName,
                QuestionnaireFile, installOptions));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(null, _serverParkName,
                QuestionnaireFile, installOptions));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(_questionnaireName, string.Empty,
                                                                        QuestionnaireFile, installOptions));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(_questionnaireName, null,
                QuestionnaireFile, installOptions));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireFile_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(_questionnaireName, _serverParkName,
                string.Empty, installOptions));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireFile_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var installOptions = new InstallOptions();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(_questionnaireName, _serverParkName,
                null, installOptions));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireFile"));
        }

        [Test]
        public void Given_A_Null_InstallOptions_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string QuestionnaireFile = @"d:\\opn2101a.pkg";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(_questionnaireName, _serverParkName,
                QuestionnaireFile, null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'installOptions' must be supplied"));
        }

        [Test]
        public void Given_DeleteCases_Is_True_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            // act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName, deleteCases: true);

            // assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(v => v.RemoveDataRecords(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_DeleteCases_Is_False_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            // act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(
                v => v.RemoveDataRecords(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_DeleteCases_Not_Provided_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            // act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName);

            // assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(
                v => v.RemoveDataRecords(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallQuestionnaire(this._questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallQuestionnaire(this._questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallQuestionnaire(string.Empty, this._serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallQuestionnaire(null, this._serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [TestCase(QuestionnaireInterviewType.Cati, QuestionnaireDataEntryType.StrictInterviewing)]
        [TestCase(QuestionnaireInterviewType.Capi, QuestionnaireDataEntryType.StrictInterviewing)]
        [TestCase(QuestionnaireInterviewType.Cawi, QuestionnaireDataEntryType.StrictInterviewing)]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireConfigurationModel_Then_The_Expected_Result_Is_Returned(QuestionnaireInterviewType questionnaireInterviewType, QuestionnaireDataEntryType questionnaireDataEntryType)
        {
            // arrange
            var questionnaireConfigurationModel = new QuestionnaireConfigurationModel
            {
                QuestionnaireInterviewType = questionnaireInterviewType,
                QuestionnaireDataEntryType = questionnaireDataEntryType
            };
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireConfigurationModel(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireConfigurationModel);

            // act
            var result = _sut.GetQuestionnaireConfigurationModel(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.QuestionnaireInterviewType, Is.InstanceOf<QuestionnaireInterviewType>());
            Assert.That(result.QuestionnaireInterviewType, Is.EqualTo(questionnaireInterviewType));
            Assert.That(result.QuestionnaireDataEntryType, Is.InstanceOf<QuestionnaireDataEntryType>());
            Assert.That(result.QuestionnaireDataEntryType, Is.EqualTo(questionnaireDataEntryType));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireConfigurationModel_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireConfigurationModel(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireConfigurationModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireConfigurationModel(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireConfigurationModel_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireConfigurationModel(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireConfigurationModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireConfigurationModel(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ActivateQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var questionnaireMock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireMock.Object);

            // act
            _sut.ActivateQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            questionnaireMock.Verify(v => v.Activate(), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ActivateQuestionnaire(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ActivateQuestionnaire(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ActivateQuestionnaire(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ActivateQuestionnaire(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_DeactivateQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var questionnaireMock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireMock.Object);

            // act
            _sut.DeactivateQuestionnaire(_questionnaireName, _serverParkName);

            // assert
            questionnaireMock.Verify(v => v.Deactivate(), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DeactivateQuestionnaire(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DeactivateQuestionnaire(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DeactivateQuestionnaire(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DeactivateQuestionnaire(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireModes_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.GetQuestionnaireModes(_questionnaireName, _serverParkName);

            // assert
            _questionnaireMetaServiceMock.Verify(v => v.GetQuestionnaireModes(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireModes(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireModes(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireModes(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireModes(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_DataEntrySettingsModel_Back()
        {
            // arrange
            _questionnaireMetaServiceMock.Setup(s => s.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(new List<DataEntrySettingsModel>
                {
                    new DataEntrySettingsModel { Type = "StrictInterviewing", DeleteSessionOnTimeout = true, DeleteSessionOnQuit = true }
                });

            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DataEntrySettingsModel>>());
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_Valid_DataEntrySettingsModel_Back(bool timeout, bool quit)
        {
            // arrange
            _questionnaireMetaServiceMock.Setup(s => s.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(new List<DataEntrySettingsModel>
                {
                    new DataEntrySettingsModel
                    {
                        Type = "StrictInterviewing",
                        SessionTimeout = 30,
                        SaveSessionOnTimeout = timeout,
                        SaveSessionOnQuit = quit,
                        DeleteSessionOnTimeout = timeout,
                        DeleteSessionOnQuit = quit
                    }
                });

            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, _serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));

            var dataEntrySettings = result.First();
            Assert.That(dataEntrySettings, Is.Not.Null);
            Assert.That(dataEntrySettings.Type, Is.EqualTo("StrictInterviewing"));
            Assert.That(dataEntrySettings.SessionTimeout, Is.EqualTo(30));
            Assert.That(dataEntrySettings.SaveSessionOnTimeout, Is.EqualTo(timeout));
            Assert.That(dataEntrySettings.SaveSessionOnQuit, Is.EqualTo(quit));
            Assert.That(dataEntrySettings.DeleteSessionOnTimeout, Is.EqualTo(timeout));
            Assert.That(dataEntrySettings.DeleteSessionOnQuit, Is.EqualTo(quit));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_questionnaireName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireDataEntrySettings(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireDataEntrySettings(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }
    }
}
