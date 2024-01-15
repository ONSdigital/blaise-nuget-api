﻿using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Extensions;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class QuestionnaireServiceTests
    {
        private Mock<IServerParkService> _parkServiceMock;

        private Mock<ISurvey> _questionnaireMock;
        private Mock<ISurveyCollection> _questionnaireCollectionMock;
        private Mock<IServerPark> _serverParkMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly Guid _questionnaireId;

        private QuestionnaireService _sut;

        public QuestionnaireServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _questionnaireId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup questionnaires
            _questionnaireMock = new Mock<ISurvey>();
            _questionnaireMock.Setup(s => s.Name).Returns(_questionnaireName);
            _questionnaireMock.Setup(s => s.InstrumentID).Returns(_questionnaireId);

            var questionnaireItems = new List<ISurvey> { _questionnaireMock.Object };

            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());

            //setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            _parkServiceMock = new Mock<IServerParkService>();
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, _serverParkName)).Returns(_serverParkMock.Object);
            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(new List<string> { _serverParkName });
            
            _sut = new QuestionnaireService(_parkServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireNames_Then_I_Get_A_Correct_List_Of_Questionnaire_Names_Returned()
        {
            //act
            var result = _sut.GetQuestionnaireNames(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_questionnaireName));
        }

        [Test]
        public void Given_No_Questionnaires_When_I_Call_GetQuestionnaireNames_Then_An_Empty_List_Is_Returned()
        {
            //arrange
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);
            _parkServiceMock.Setup(s => s.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetQuestionnaireNames(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Given_No_Questionnaires_Are_On_A_ServerPark_When_I_Call_GetQuestionnaireNames_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkDoesNotExist";

            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetQuestionnaireNames(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestCase("TestQuestionnaireName", true)]
        [TestCase("QuestionnaireNotFound", false)]
        public void Given_I_Call_QuestionnaireExists_Then_I_Get_A_Correct_Value_Returned(string questionnaireName, bool exists)
        {
            //act
            var result = _sut.QuestionnaireExists(_connectionModel, questionnaireName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaires_Then_I_Get_A_Correct_List_Of_Questionnaire_Returned()
        {
            //act
            var result = _sut.GetQuestionnaires(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_questionnaireMock.Object));
        }

        [Test]
        public void Given_No_Questionnaires_When_I_Call_GetQuestionnaires_Then_An_Empty_Lst_Is_Returned()
        {
            //arrange
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            //act
            var result = _sut.GetQuestionnaires(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void Given_No_Surveys_Are_On_A_ServerPark_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkDoesNotExist";

            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetQuestionnaireNames(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestCase("Questionnaire1")]
        [TestCase("questionnaire1")]
        [TestCase("QUESTIONNAIRE1")]
        public void Given_I_Call_GetQuestionnaire_Then_I_Get_The_Correct_Questionnaire_Returned(string questionnaire1Name)
        {
            //arrange
            var questionnaire1Mock = new Mock<ISurvey>();
            questionnaire1Mock.Setup(s => s.Name).Returns(questionnaire1Name);

            const string questionnaire2Name = "questionnaire2";
            var questionnaire2Mock = new Mock<ISurvey>();
            questionnaire2Mock.Setup(s => s.Name).Returns(questionnaire2Name);

            var questionnaireItems = new List<ISurvey> { questionnaire1Mock.Object, questionnaire2Mock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            //act
            var result = _sut.GetQuestionnaire(_connectionModel, questionnaire1Name, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);
            Assert.AreSame(questionnaire1Mock.Object, result);
            Assert.AreEqual(questionnaire1Name, result.Name);
        }

        [Test]
        public void Given_Survey_Does_Not_Exist_When_I_Call_GetSurvey_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string questionnaire1Name = "questionnaire1";
            var questionnaire1Mock = new Mock<ISurvey>();
            questionnaire1Mock.Setup(s => s.Name).Returns(questionnaire1Name);

            const string questionnaire2Name = "questionnaire2";

            var questionnaireItems = new List<ISurvey> { questionnaire1Mock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetQuestionnaire(_connectionModel, questionnaire2Name, _serverParkName));
            Assert.AreEqual($"No questionnaire found for questionnaire name '{questionnaire2Name}'", exception.Message);
        }

        [Test]

        public void Given_Survey_Exists_When_I_Call_GetInstallDate_The_Correct_Date_Is_Returned()
        {
            //arrange
            var installDate = DateTime.Today;
            const string questionnaireName = "questionnaire1";
            var questionnaire1Mock = new Mock<ISurvey>();
            questionnaire1Mock.Setup(s => s.Name).Returns(questionnaireName);
            questionnaire1Mock.Setup(s => s.InstallDate).Returns(installDate);

            var questionnaireItems = new List<ISurvey> { questionnaire1Mock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);
            
            //act
            var result = _sut.GetInstallDate(_connectionModel, questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(installDate, result);
        }

        [TestCase("Installing", QuestionnaireStatusType.Installing)]
        [TestCase("Active", QuestionnaireStatusType.Active)]
        [TestCase("Inactive", QuestionnaireStatusType.Inactive)]
        [TestCase("Erroneous", QuestionnaireStatusType.Erroneous)]
        [TestCase("Failed", QuestionnaireStatusType.Failed)]
        [TestCase("Errored", QuestionnaireStatusType.Other)]
        [TestCase("Error", QuestionnaireStatusType.Other)]
        [TestCase("Invalid", QuestionnaireStatusType.Other)]
        [TestCase("Not found", QuestionnaireStatusType.Other)]
        [TestCase("Not available", QuestionnaireStatusType.Other)]
        [TestCase("unknown", QuestionnaireStatusType.Other)]

        public void Given_Questionnaire_Exists_When_I_Call_GetQuestionnaireStatus_The_Correct_Status_Is_Returned(string questionnaireStatus, QuestionnaireStatusType questionnaireStatusType)
        {
            //arrange
            const string questionnaireName = "questionnaire1";
            var questionnaire1Mock = new Mock<ISurvey>();
            questionnaire1Mock.Setup(s => s.Name).Returns(questionnaireName);
            questionnaire1Mock.Setup(s => s.Status).Returns(questionnaireStatus);

            var questionnaireItems = new List<ISurvey> { questionnaire1Mock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            //act
            var result = _sut.GetQuestionnaireStatus(_connectionModel, questionnaireName, _serverParkName);

            //assert
            Assert.AreEqual(questionnaireStatusType, result);
        }


        [Test]
        public void Given_A_Questionnaire_Does_Not_Exist_When_I_Call_GetQuestionnaireStatus_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string questionnaire1Name = "questionnaire1";
            var questionnaire1Mock = new Mock<ISurvey>();
            questionnaire1Mock.Setup(s => s.Name).Returns(questionnaire1Name);

            const string questionnaire2Name = "questionnaire2";

            var questionnaireItems = new List<ISurvey> { questionnaire1Mock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetQuestionnaireStatus(_connectionModel, questionnaire2Name, _serverParkName));
            Assert.AreEqual($"No questionnaire found for questionnaire name '{questionnaire2Name}'", exception.Message);
        }

        [TestCase("CATI", QuestionnaireInterviewType.Cati)]
        [TestCase("CAPI", QuestionnaireInterviewType.Capi)]
        [TestCase("CAWI", QuestionnaireInterviewType.Cawi)]
        public void Given_Questionnaire_Exists_When_I_Call_GetQuestionnaireInterviewType_Then_The_Correct_QuestionnaireInterviewType_Is_Returned(
            string interviewType, QuestionnaireInterviewType questionnaireInterviewType)
        {
            //arrange
            const string questionnaireName = "questionnaire1";
            var questionnaireMock = new Mock<ISurvey>();
            questionnaireMock.Setup(s => s.Name).Returns(questionnaireName);

            var questionnaireItems = new List<ISurvey> { questionnaireMock.Object };
            _questionnaireCollectionMock = new Mock<ISurveyCollection>();
            _questionnaireCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => questionnaireItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_questionnaireCollectionMock.Object);

            var iConfigurationMock = new Mock<IConfiguration>();
            iConfigurationMock.Setup(c => c.InitialLayoutSetGroupName).Returns(interviewType);
            iConfigurationMock.Setup(c => c.InstrumentName).Returns(questionnaireName);
            var configurations = new List<IConfiguration> { iConfigurationMock.Object };

            var machineConfigurationMock = new Mock<IMachineConfigurationCollection>();
            machineConfigurationMock.Setup(m => m.Configurations).Returns(configurations);
            questionnaireMock.Setup(s => s.Configuration).Returns(machineConfigurationMock.Object);

            //act
            var result = _sut.GetQuestionnaireInterviewType(_connectionModel, questionnaireName, _serverParkName);

            //assert
            Assert.AreEqual(questionnaireInterviewType, result);
        }

        [Test]
        public void Given_I_Call_GetAllQuestionnaires_Then_I_Get_A_Correct_List_Of_Questionnaires_Returned()
        {
            //arrange
            const string serverPark1Name = "ServerPark1";
            const string serverPark2Name = "ServerPark2";

            var questionnaire1Mock = new Mock<ISurvey>();
            var questionnaire2Mock = new Mock<ISurvey>();
            var questionnaire3Mock = new Mock<ISurvey>();

            var questionnaire1Items = new List<ISurvey> { questionnaire1Mock.Object, questionnaire2Mock.Object };
            var questionnaire2Items = new List<ISurvey> { questionnaire3Mock.Object };

            var questionnaireCollection1Mock = new Mock<ISurveyCollection>();
            questionnaireCollection1Mock.Setup(s => s.GetEnumerator()).Returns(() => questionnaire1Items.GetEnumerator());

            var questionnaireCollection2Mock = new Mock<ISurveyCollection>();
            questionnaireCollection2Mock.Setup(s => s.GetEnumerator()).Returns(() => questionnaire2Items.GetEnumerator());

            var serverPark1Mock = new Mock<IServerPark>();
            serverPark1Mock.Setup(s => s.Name).Returns(serverPark1Name);
            serverPark1Mock.Setup(s => s.Surveys).Returns(questionnaireCollection1Mock.Object);

            var serverPark2Mock = new Mock<IServerPark>();
            serverPark2Mock.Setup(s => s.Name).Returns(serverPark2Name);
            serverPark2Mock.Setup(s => s.Surveys).Returns(questionnaireCollection2Mock.Object);

            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(new List<string> { serverPark1Name, serverPark2Name });
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverPark1Name)).Returns(serverPark1Mock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverPark2Name)).Returns(serverPark2Mock.Object);

            //act
            var result = _sut.GetAllQuestionnaires(_connectionModel).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(questionnaire1Mock.Object));
            Assert.True(result.Contains(questionnaire2Mock.Object));
            Assert.True(result.Contains(questionnaire3Mock.Object));
        }


        [Test]
        public void Given_I_Call_GetQuestionnaireId_Then_I_Get_A_Guid_Returned()
        {
            //act
            var result = _sut.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Guid>(result);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireId_Then_I_Get_The_Correct_QuestionnaireId_Returned()
        {
            //act
            var result = _sut.GetQuestionnaireId(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            Assert.AreEqual(_questionnaireId, result);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireId_And_The_Questionnaire_Does_Not_Exist_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            //arrange
            const string questionnaireName = "QuestionnaireThatDoesNotExist";

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetQuestionnaireId(_connectionModel, questionnaireName, _serverParkName));
            Assert.AreEqual($"Questionnaire '{questionnaireName}' not found on server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetMetaFileName_Then_The_Correct_Name_Is_Returned()
        {
            //arrange
            const string metaFileName = "MetaFileName";

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.MetaFileName).Returns(metaFileName);


            var configurationItems = new List<IConfiguration> { configurationMock.Object };

            var configurationCollectionMock = new Mock<IMachineConfigurationCollection>();
            configurationCollectionMock.Setup(s => s.Configurations).Returns(configurationItems);

            _questionnaireMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

            //act
            var result = _sut.GetMetaFileName(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(metaFileName, result);
        }

        //[Test]
        //public void Given_Valid_Arguments_When_I_Call_InstallQuestionnaire_Then_The_Correct_Services_Are_Called()
        //{
        //    //arrange
        //    const string questionnaireFile = @"d:\\opn2101a.pkg";
        //    const string fileName = "OPN.bdix";
        //    const string dataModelFileName = "OPN.bmix";

        //    var configurationMock = new Mock<IConfiguration>();
        //    configurationMock.Setup(c => c.DataFileName).Returns(fileName);
        //    configurationMock.Setup(c => c.MetaFileName).Returns(dataModelFileName);

        //    var configurationItems = new List<IConfiguration> { configurationMock.Object };

        //    var configurationCollectionMock = new Mock<IMachineConfigurationCollection>();
        //    configurationCollectionMock.Setup(s => s.Configurations).Returns(configurationItems);

        //    _questionnaireMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

        //    //act
        //    _sut.InstallQuestionnaire(_connectionModel,_questionnaireName, _serverParkName,
        //        questionnaireFile, QuestionnaireInterviewType.Cati);

        //    //assert
        //    _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, _serverParkName), Times.Once);
        //    _serverParkMock.Verify(v => v.InstallSurvey(questionnaireFile, QuestionnaireInterviewType.Cati.FullName(),
        //                                QuestionnaireDataEntryType.StrictInterviewing.FullName(), DataOverwriteMode.Always), Times.Once);
        //}

        [Test]
        public void Given_An_Questionnaire_Exists_When_I_Call_UninstallQuestionnaire_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.UninstallQuestionnaire(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, _serverParkName), Times.AtLeastOnce);
            _serverParkMock.Verify(v => v.RemoveSurvey(_questionnaireId), Times.Once);
        }
    }
}
