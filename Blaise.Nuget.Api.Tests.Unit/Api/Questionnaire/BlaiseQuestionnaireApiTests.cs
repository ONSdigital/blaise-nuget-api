﻿using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Questionnaire
{
    using Blaise.Nuget.Api.Core.Interfaces.Providers;

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
                _connectionModel, _sqlServiceMock.Object
                , _configurationProviderMock.Object);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseQuestionnaireApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseQuestionnaireApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseQuestionnaireApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseQuestionnaireApi(new ConnectionModel()));

        }
        [Test]
        public void Given_Valid_Arguments_When_I_Call_QuestionnaireExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.QuestionnaireExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.QuestionnaireExists(_questionnaireName, _serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.QuestionnaireExists(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_QuestionnaireExists_Then_The_Expected_Result_Is_Returned(bool exists)
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.QuestionnaireExists(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(exists);

            //act            
            var result = _sut.QuestionnaireExists(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_QuestionnaireExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.QuestionnaireExists(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_QuestionnaireExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.QuestionnaireExists(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_QuestionnaireExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.QuestionnaireExists(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_QuestionnaireExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.QuestionnaireExists(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetQuestionnairesAcrossServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.GetAllQuestionnaires(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetQuestionnairesAcrossServerParks();

            //assert
            _questionnaireServiceMock.Verify(v => v.GetAllQuestionnaires(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetQuestionnairesAcrossServerParks_Then_The_Expected_Questionnaires_Are_Returned()
        {
            //arrange
            var questionnaire1Mock = new Mock<ISurvey>();
            var questionnaire2Mock = new Mock<ISurvey>();
            var questionnaire3Mock = new Mock<ISurvey>();

            var questionnaires = new List<ISurvey> { questionnaire1Mock.Object, questionnaire2Mock.Object, questionnaire3Mock.Object };

            _questionnaireServiceMock.Setup(p => p.GetAllQuestionnaires(_connectionModel)).Returns(questionnaires);

            //act
            var result = _sut.GetQuestionnairesAcrossServerParks().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(questionnaire1Mock.Object));
            Assert.True(result.Contains(questionnaire2Mock.Object));
            Assert.True(result.Contains(questionnaire3Mock.Object));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaires_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaires(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetQuestionnaires(_serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaires(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaires_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var questionnaire1Mock = new Mock<ISurvey>();
            var questionnaire2Mock = new Mock<ISurvey>();
            var questionnaireList = new List<ISurvey>
            {
                questionnaire1Mock.Object,
                questionnaire2Mock.Object
            };

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaires(_connectionModel, _serverParkName)).Returns(questionnaireList);

            //act            
            var result = _sut.GetQuestionnaires(_serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(questionnaire1Mock.Object));
            Assert.True(result.Contains(questionnaire2Mock.Object));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaires_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaires(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaires_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaires(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<ISurvey>());

            //act
            _sut.GetQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaire_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var questionnaire1Mock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaire1Mock.Object);

            //act            
            var result = _sut.GetQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);
            Assert.AreSame(questionnaire1Mock.Object, result);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaire(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaire(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaire(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaire(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireStatus_Then_The_Correct_Service_Method_Is_Called()
        {
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireStatus(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<QuestionnaireStatusType>());

            //act
            _sut.GetQuestionnaireStatus(_questionnaireName, _serverParkName);

            //assert
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
            //arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireStatus(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireStatusType);

            //act            
            var result = _sut.GetQuestionnaireStatus(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<QuestionnaireStatusType>(result);
            Assert.AreEqual(questionnaireStatusType, result);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireStatus(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireStatus(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireStatus(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireStatus_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireStatus(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireNames(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetNamesOfQuestionnaires(_serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaireNames(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireNames_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var questionnaireList = new List<string>
            {
                "Questionnaire",
                "Questionnaire"
            };

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireNames(_connectionModel, _serverParkName)).Returns(questionnaireList);

            //act            
            var result = _sut.GetNamesOfQuestionnaires(_serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains("Questionnaire"));
            Assert.True(result.Contains("Questionnaire"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireNames_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNamesOfQuestionnaires(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNamesOfQuestionnaires_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNamesOfQuestionnaires(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireId_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            //act
            _sut.GetIdOfQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireId_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var questionnaireId = Guid.NewGuid();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireId);

            //act
            var result = _sut.GetIdOfQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(questionnaireId, result);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfQuestionnaire(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfQuestionnaire(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfQuestionnaire(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetIdOfQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfQuestionnaire(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [TestCase(QuestionnaireInterviewType.Cati)]
        [TestCase(QuestionnaireInterviewType.Cawi)]
        [TestCase(QuestionnaireInterviewType.Capi)]
        public void Given_Valid_Arguments_When_I_Call_InstallQuestionnaire_Then_The_Correct_Service_Method_Is_Called(QuestionnaireInterviewType questionnaireInterviewType)
        {
            //arrange
            const string questionnaireFile = @"d:\\opn2101a.pkg";

            //act
            _sut.InstallQuestionnaire(_questionnaireName, _serverParkName, questionnaireFile, questionnaireInterviewType);

            //assert
            _questionnaireServiceMock.Verify(v => v.InstallQuestionnaire(_connectionModel, _questionnaireName,
                _serverParkName, questionnaireFile, questionnaireInterviewType), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string questionnaireFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(string.Empty, _serverParkName,
                questionnaireFile, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string questionnaireFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(null, _serverParkName,
                questionnaireFile, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string questionnaireFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(_questionnaireName, string.Empty,
                                                                        questionnaireFile, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string questionnaireFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(_questionnaireName, null,
                questionnaireFile, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_questionnaireFile_When_I_Call_InstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallQuestionnaire(_questionnaireName, _serverParkName,
                string.Empty, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("A value for the argument 'questionnaireFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_questionnaireFile_When_I_Call_InstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallQuestionnaire(_questionnaireName, _serverParkName,
                null, QuestionnaireInterviewType.Cati));
            Assert.AreEqual("questionnaireFile", exception.ParamName);
        }

        [Test]
        public void Given_DeleteCases_Is_True_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            //act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName, true);

            //assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(v => v.RemoveDataRecords(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_DeleteCases_Is_False_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            //act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(v => v.RemoveDataRecords(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_DeleteCases_Not_Provided_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Service_Methods_Are_Called()
        {
            //act
            _sut.UninstallQuestionnaire(this._questionnaireName, this._serverParkName);

            //assert
            _questionnaireServiceMock.Verify(v => v.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _caseServiceMock.Verify(v => v.RemoveDataRecords(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallQuestionnaire(this._questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallQuestionnaire(this._questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallQuestionnaire(string.Empty, this._serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UninstallQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallQuestionnaire(null, this._serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [TestCase(QuestionnaireInterviewType.Cati)]
        [TestCase(QuestionnaireInterviewType.Capi)]
        [TestCase(QuestionnaireInterviewType.Cawi)]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireInterviewType_Then_The_Expected_Result_Is_Returned(QuestionnaireInterviewType questionnaireInterviewType)
        {
            //arrange
            _questionnaireServiceMock.Setup(p => p.GetQuestionnaireInterviewType(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireInterviewType);

            //act            
            var result = _sut.GetQuestionnaireInterviewType(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<QuestionnaireInterviewType>(result);
            Assert.AreEqual(questionnaireInterviewType, result);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireInterviewType_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireInterviewType(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireInterviewType_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireInterviewType(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireInterviewType_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireInterviewType(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireInterviewType_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireInterviewType(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ActivateQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var questionnaireMock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireMock.Object);

            //act
            _sut.ActivateQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            questionnaireMock.Verify(v => v.Activate(), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ActivateQuestionnaire(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ActivateQuestionnaire(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ActivateQuestionnaire(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ActivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ActivateQuestionnaire(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_DeactivateQuestionnaire_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var questionnaireMock = new Mock<ISurvey>();

            _questionnaireServiceMock.Setup(p => p.GetQuestionnaire(_connectionModel, _questionnaireName, _serverParkName)).Returns(questionnaireMock.Object);

            //act
            _sut.DeactivateQuestionnaire(_questionnaireName, _serverParkName);

            //assert
            questionnaireMock.Verify(v => v.Deactivate(), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DeactivateQuestionnaire(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DeactivateQuestionnaire(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DeactivateQuestionnaire(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_DeactivateQuestionnaire_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DeactivateQuestionnaire(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetQuestionnaireModes_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetQuestionnaireModes(_questionnaireName, _serverParkName);

            //assert
            _questionnaireMetaServiceMock.Verify(v => v.GetQuestionnaireModes(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireModes(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireModes(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireModes(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireModes_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireModes(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_DataEntrySettingsModel_Back()
        {
            //arrange
            _questionnaireMetaServiceMock.Setup(s => s.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(new List<DataEntrySettingsModel>
                {
                    new DataEntrySettingsModel { Type = "StrictInterviewing", DeleteSessionOnTimeout = true, DeleteSessionOnQuit = true }
                });

            //act
            var result = _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DataEntrySettingsModel>>(result);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_Valid_DataEntrySettingsModel_Back(bool timeout, bool quit)
        {
            //arrange
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

            //act
            var result = _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var dataEntrySettings = result.First();
            Assert.IsNotNull(dataEntrySettings);
            Assert.AreEqual("StrictInterviewing", dataEntrySettings.Type);
            Assert.AreEqual(30, dataEntrySettings.SessionTimeout);
            Assert.AreEqual(timeout, dataEntrySettings.SaveSessionOnTimeout);
            Assert.AreEqual(quit, dataEntrySettings.SaveSessionOnQuit);
            Assert.AreEqual(timeout, dataEntrySettings.DeleteSessionOnTimeout);
            Assert.AreEqual(quit, dataEntrySettings.DeleteSessionOnQuit);
        }


        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireDataEntrySettings(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_questionnaireName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetQuestionnaireDataEntrySettings(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetQuestionnaireDataEntrySettings_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetQuestionnaireDataEntrySettings(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }
    }
}
