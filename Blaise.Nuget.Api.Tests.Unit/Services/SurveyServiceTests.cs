using Blaise.Nuget.Api.Contracts.Exceptions;
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
    public class SurveyServiceTests
    {
        private Mock<IServerParkService> _parkServiceMock;

        private Mock<ISurvey> _surveyMock;
        private Mock<ISurveyCollection> _surveyCollectionMock;
        private Mock<IServerPark> _serverParkMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private SurveyService _sut;

        public SurveyServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup surveys
            _surveyMock = new Mock<ISurvey>();
            _surveyMock.Setup(s => s.Name).Returns(_instrumentName);
            _surveyMock.Setup(s => s.InstrumentID).Returns(_instrumentId);

            var surveyItems = new List<ISurvey> { _surveyMock.Object };

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());

            //setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            _parkServiceMock = new Mock<IServerParkService>();
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, _serverParkName)).Returns(_serverParkMock.Object);
            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(new List<string> { _serverParkName });
            
            _sut = new SurveyService(_parkServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetSurveyNames_Then_I_Get_A_Correct_List_Of_Survey_Names_Returned()
        {
            //act
            var result = _sut.GetSurveyNames(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_instrumentName));
        }

        [Test]
        public void Given_No_Surveys_When_I_Call_GetSurveyNames_Then_An_Empty_List_Is_Returned()
        {
            //arrange
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(s => s.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetSurveyNames(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Given_No_Surveys_Are_On_A_ServerPark_When_I_Call_GetSurveyNames_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkDoesNotExist";

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetSurveyNames(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestCase("TestInstrumentName", true)]
        [TestCase("InstrumentNotFound", false)]
        public void Given_I_Call_SurveyExists_Then_I_Get_A_Correct_Value_Returned(string instrumentName, bool exists)
        {
            //act
            var result = _sut.SurveyExists(_connectionModel, instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_I_Call_GetSurveys_Then_I_Get_A_Correct_List_Of_Surveys_Returned()
        {
            //act
            var result = _sut.GetSurveys(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_surveyMock.Object));
        }

        [Test]
        public void Given_No_Surveys_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act
            var result = _sut.GetSurveyNames(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Given_No_Surveys_Are_On_A_ServerPark_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string serverParkName = "ServerParkDoesNotExist";

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() 
                => new List<ISurvey>().GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act
            var result = _sut.GetSurveyNames(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestCase("Instrument1")]
        [TestCase("instrument1")]
        [TestCase("INSTRUMENT1")]
        public void Given_I_Call_GetSurvey_Then_I_Get_The_Correct_Survey_Returned(string instrument1Name)
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1Name);

            const string instrument2Name = "Instrument2";
            var survey2Mock = new Mock<ISurvey>();
            survey2Mock.Setup(s => s.Name).Returns(instrument2Name);

            var surveyItems = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act
            var result = _sut.GetSurvey(_connectionModel, instrument1Name, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);
            Assert.AreSame(survey1Mock.Object, result);
            Assert.AreEqual(instrument1Name, result.Name);
        }

        [Test]
        public void Given_Survey_Does_Not_Exist_When_I_Call_GetSurvey_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string instrument1Name = "Instrument1";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1Name);

            const string instrument2Name = "Instrument2";

            var surveyItems = new List<ISurvey> { survey1Mock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurvey(_connectionModel, instrument2Name, _serverParkName));
            Assert.AreEqual($"No survey found for instrument name '{instrument2Name}'", exception.Message);
        }

        [Test]

        public void Given_Survey_Exists_When_I_Call_GetInstallDate_The_Correct_Date_Is_Returned()
        {
            //arrange
            var installDate = DateTime.Today;
            const string instrumentName = "Instrument1";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrumentName);
            survey1Mock.Setup(s => s.InstallDate).Returns(installDate);

            var surveyItems = new List<ISurvey> { survey1Mock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            
            //act
            var result = _sut.GetInstallDate(_connectionModel, instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(installDate, result);
        }

        [TestCase("Installing", SurveyStatusType.Installing)]
        [TestCase("Active", SurveyStatusType.Active)]
        [TestCase("Inactive", SurveyStatusType.Inactive)]
        [TestCase("Erroneous", SurveyStatusType.Erroneous)]
        [TestCase("Failed", SurveyStatusType.Failed)]
        [TestCase("Errored", SurveyStatusType.Other)]
        [TestCase("Error", SurveyStatusType.Other)]
        [TestCase("Invalid", SurveyStatusType.Other)]
        [TestCase("Not found", SurveyStatusType.Other)]
        [TestCase("Not available", SurveyStatusType.Other)]
        [TestCase("unknown", SurveyStatusType.Other)]

        public void Given_Survey_Exists_When_I_Call_GetSurveyStatus_The_Correct_Status_Is_Returned(string surveyStatus, SurveyStatusType surveyStatusType)
        {
            //arrange
            const string instrumentName = "Instrument1";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrumentName);
            survey1Mock.Setup(s => s.Status).Returns(surveyStatus);

            var surveyItems = new List<ISurvey> { survey1Mock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act
            var result = _sut.GetSurveyStatus(_connectionModel, instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(surveyStatusType, result);
        }


        [Test]
        public void Given_Survey_Does_Not_Exist_When_I_Call_GetSurveyStatus_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            const string instrument1Name = "Instrument1";
            var survey1Mock = new Mock<ISurvey>();
            survey1Mock.Setup(s => s.Name).Returns(instrument1Name);

            const string instrument2Name = "Instrument2";

            var surveyItems = new List<ISurvey> { survey1Mock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurveyStatus(_connectionModel, instrument2Name, _serverParkName));
            Assert.AreEqual($"No survey found for instrument name '{instrument2Name}'", exception.Message);
        }

        [TestCase("CATI", SurveyInterviewType.Cati)]
        [TestCase("CAPI", SurveyInterviewType.Capi)]
        [TestCase("CAWI", SurveyInterviewType.Cawi)]
        public void Given_Survey_Exists_When_I_Call_GetSurveyInterviewType_Then_The_Correct_SurveyInterviewType_Is_Returned(
            string interviewType, SurveyInterviewType surveyInterviewType)
        {
            //arrange
            const string instrumentName = "Instrument1";
            var surveyMock = new Mock<ISurvey>();
            surveyMock.Setup(s => s.Name).Returns(instrumentName);

            var surveyItems = new List<ISurvey> { surveyMock.Object };
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            var iConfigurationMock = new Mock<IConfiguration>();
            iConfigurationMock.Setup(c => c.InitialLayoutSetGroupName).Returns(interviewType);
            iConfigurationMock.Setup(c => c.InstrumentName).Returns(instrumentName);
            var configurations = new List<IConfiguration> { iConfigurationMock.Object };

            var machineConfigurationMock = new Mock<IMachineConfigurationCollection>();
            machineConfigurationMock.Setup(m => m.Configurations).Returns(configurations);
            surveyMock.Setup(s => s.Configuration).Returns(machineConfigurationMock.Object);

            //act
            var result = _sut.GetSurveyInterviewType(_connectionModel, instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(surveyInterviewType, result);
        }

        [Test]
        public void Given_I_Call_GetAllSurveys_Then_I_Get_A_Correct_List_Of_Surveys_Returned()
        {
            //arrange
            const string serverPark1Name = "ServerPark1";
            const string serverPark2Name = "ServerPark2";

            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var survey1Items = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object };
            var survey2Items = new List<ISurvey> { survey3Mock.Object };

            var surveyCollection1Mock = new Mock<ISurveyCollection>();
            surveyCollection1Mock.Setup(s => s.GetEnumerator()).Returns(() => survey1Items.GetEnumerator());

            var surveyCollection2Mock = new Mock<ISurveyCollection>();
            surveyCollection2Mock.Setup(s => s.GetEnumerator()).Returns(() => survey2Items.GetEnumerator());

            var serverPark1Mock = new Mock<IServerPark>();
            serverPark1Mock.Setup(s => s.Name).Returns(serverPark1Name);
            serverPark1Mock.Setup(s => s.Surveys).Returns(surveyCollection1Mock.Object);

            var serverPark2Mock = new Mock<IServerPark>();
            serverPark2Mock.Setup(s => s.Name).Returns(serverPark2Name);
            serverPark2Mock.Setup(s => s.Surveys).Returns(surveyCollection2Mock.Object);

            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(new List<string> { serverPark1Name, serverPark2Name });
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverPark1Name)).Returns(serverPark1Mock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverPark2Name)).Returns(serverPark2Mock.Object);

            //act
            var result = _sut.GetAllSurveys(_connectionModel).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }


        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_A_Guid_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Guid>(result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_The_Correct_InstrumentId_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_instrumentId, result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_And_The_Instrument_DoesNot_Exist_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            //arrange
            const string instrumentName = "InstrumentThatDoesNotExist";

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetInstrumentId(_connectionModel, instrumentName, _serverParkName));
            Assert.AreEqual($"Instrument '{instrumentName}' not found on server park '{_serverParkName}'", exception.Message);
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

            _surveyMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

            //act
            var result = _sut.GetMetaFileName(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(metaFileName, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_InstallInstrument_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string instrumentFile = @"d:\\opn2101a.pkg";
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.DataFileName).Returns(fileName);
            configurationMock.Setup(c => c.MetaFileName).Returns(dataModelFileName);

            var configurationItems = new List<IConfiguration> { configurationMock.Object };

            var configurationCollectionMock = new Mock<IMachineConfigurationCollection>();
            configurationCollectionMock.Setup(s => s.Configurations).Returns(configurationItems);

            _surveyMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

            //act
            _sut.InstallInstrument(_connectionModel,_instrumentName, _serverParkName,
                instrumentFile, SurveyInterviewType.Cati);

            //assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, _serverParkName), Times.Once);
            _serverParkMock.Verify(v => v.InstallSurvey(instrumentFile, SurveyInterviewType.Cati.FullName(),
                                        SurveyDataEntryType.StrictInterviewing.FullName(), DataOverwriteMode.Always), Times.Once);
        }

        [Test]
        public void Given_An_Instrument_Exists_When_I_Call_UninstallInstrument_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.UninstallInstrument(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, _serverParkName), Times.AtLeastOnce);
            _serverParkMock.Verify(v => v.RemoveSurvey(_instrumentId), Times.Once);
        }
    }
}
