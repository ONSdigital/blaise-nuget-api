
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class SurveyMetaServiceTests
    {
        private ConnectionModel _connectionModel;
        private string _instrumentName;
        private string _serverParkName;

        private Mock<IDataModelService> _dataModelServiceMock;

        private ISurveyMetaService _sut;

        [SetUp]
        public void SetupTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";

            _dataModelServiceMock = new Mock<IDataModelService>();

            _sut = new SurveyMetaService(_dataModelServiceMock.Object);
        }

        [Test]
        public void Given_An_Instrument_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetSurveyModes_The_Correct_Modes_Are_Returned()
        {
            //arrange
            const string mode1 = "CATI";
            var mode1Mock = new Mock<IMode>();
            mode1Mock.Setup(m => m.Name).Returns(mode1);

            const string mode2 = "CAWI";
            var mode2Mock = new Mock<IMode>();
            mode2Mock.Setup(m => m.Name).Returns(mode2);

            var modelList = new List<IMode> {mode1Mock.Object, mode2Mock.Object};

            var modeCollection = new Mock<IModeCollection>();
            modeCollection.Setup(m => m.GetEnumerator()).Returns(modelList.GetEnumerator());

            var dataModelMock = new Mock<IDatamodel2>();
            dataModelMock.Setup(dm => dm.Modes).Returns(modeCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(_connectionModel,
                _instrumentName, _serverParkName)).Returns(dataModelMock.Object);

            //act
            var result = _sut.GetSurveyModes(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.Contains("CAWI", result);
            Assert.Contains("CATI", result);
        }

        [Test]
        public void Given_An_Instrument_Has_No_DataModel_When_I_Call_GetSurveyModes_Then_An_Empty_List_Is_Returned()
        {
            //arrange
            _dataModelServiceMock.Setup(dm => dm.GetDataModel(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(null as IDatamodel);

            //act
            var result = _sut.GetSurveyModes(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Given_I_Call_GetSurveyDataEntrySettings_I_Get_A_List_Of_SurveyEntrySettingsModel_Back()
        {
            //arrange
            var dataEntrySettingsList = new List<IDataEntrySettings>();

            var dataEntrySettingsCollection = new Mock<IDataEntrySettingsCollection>();
            dataEntrySettingsCollection.Setup(des => des.GetEnumerator())
                .Returns(dataEntrySettingsList.GetEnumerator());

            var datamodelMock = new Mock<IDatamodel>();
            datamodelMock.Setup(d => d.DataEntrySettings).Returns(dataEntrySettingsCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(datamodelMock.Object);

            //act
            var result = _sut.GetSurveyDataEntrySettings(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DataEntrySettingsModel>>(result);
        }

        [Test]
        public void Given_I_Call_GetSurveyDataEntrySettings_I_Get_A_Valid_List_Of_SurveyEntrySettingsModel_Back()
        {
            //arrange

            //data entry settings 1
            var dataEntrySettings1Name = "StrictInterviewing";
            var dataEntrySettings1Timeout = 30;
            var dataEntrySettings1Mock = new Mock<IDataEntrySettings>();
            dataEntrySettings1Mock.Setup(de => de.Name).Returns(dataEntrySettings1Name);
            dataEntrySettings1Mock.As<IDataEntrySettings4>().Setup(de => de.SessionTimeout).Returns(dataEntrySettings1Timeout);
            dataEntrySettings1Mock.As<IDataEntrySettings6>().Setup(de => de.SaveOnTimeout).Returns(true);
            dataEntrySettings1Mock.As<IDataEntrySettings6>().Setup(de => de.SaveOnQuit).Returns(false);
            dataEntrySettings1Mock.As<IDataEntrySettings6>().Setup(de => de.DeleteSessionOnTimeout).Returns(true);
            dataEntrySettings1Mock.As<IDataEntrySettings6>().Setup(de => de.DeleteSessionOnQuit).Returns(false);
            dataEntrySettings1Mock.As<IDataEntrySettings4>().Setup(de => de.ApplyRecordLocking).Returns(true);

            var dataEntrySettings2Name = "CatiInterviewing";
            var dataEntrySettings2Timeout = 15;
            var dataEntrySettings2Mock = new Mock<IDataEntrySettings>();
            dataEntrySettings2Mock.Setup(de => de.Name).Returns(dataEntrySettings2Name);
            dataEntrySettings2Mock.As<IDataEntrySettings4>().Setup(de => de.SessionTimeout).Returns(dataEntrySettings2Timeout);
            dataEntrySettings2Mock.As<IDataEntrySettings6>().Setup(de => de.SaveOnTimeout).Returns(false);
            dataEntrySettings2Mock.As<IDataEntrySettings6>().Setup(de => de.SaveOnQuit).Returns(true);
            dataEntrySettings2Mock.As<IDataEntrySettings6>().Setup(de => de.DeleteSessionOnTimeout).Returns(false);
            dataEntrySettings2Mock.As<IDataEntrySettings6>().Setup(de => de.DeleteSessionOnQuit).Returns(true);
            dataEntrySettings2Mock.As<IDataEntrySettings4>().Setup(de => de.ApplyRecordLocking).Returns(false);

            var dataEntrySettingsList = new List<IDataEntrySettings> { dataEntrySettings1Mock.Object, dataEntrySettings2Mock.Object };

            var dataEntrySettingsCollection = new Mock<IDataEntrySettingsCollection>();
            dataEntrySettingsCollection.Setup(des => des.GetEnumerator())
                .Returns(dataEntrySettingsList.GetEnumerator());

            var datamodelMock = new Mock<IDatamodel>();
            datamodelMock.Setup(d => d.DataEntrySettings).Returns(dataEntrySettingsCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(datamodelMock.Object);

            //act
            var result = _sut.GetSurveyDataEntrySettings(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DataEntrySettingsModel>>(result);
            Assert.AreEqual(2, result.Count);

            var dataEntrySettings1 = result.FirstOrDefault(n => n.Type == dataEntrySettings1Name);
            Assert.IsNotNull(dataEntrySettings1);
            Assert.AreEqual(dataEntrySettings1Timeout, dataEntrySettings1.SessionTimeout);
            Assert.True(dataEntrySettings1.SaveSessionOnTimeout);
            Assert.False(dataEntrySettings1.SaveSessionOnQuit);
            Assert.True(dataEntrySettings1.DeleteSessionOnTimeout);
            Assert.False(dataEntrySettings1.DeleteSessionOnQuit);
            Assert.True(dataEntrySettings1.ApplyRecordLocking);

            var dataEntrySettings2 = result.FirstOrDefault(n => n.Type == dataEntrySettings2Name);
            Assert.IsNotNull(dataEntrySettings2);
            Assert.AreEqual(dataEntrySettings2Timeout, dataEntrySettings2.SessionTimeout);
            Assert.False(dataEntrySettings2.SaveSessionOnTimeout);
            Assert.True(dataEntrySettings2.SaveSessionOnQuit);
            Assert.False(dataEntrySettings2.DeleteSessionOnTimeout);
            Assert.True(dataEntrySettings2.DeleteSessionOnQuit);
            Assert.False(dataEntrySettings2.ApplyRecordLocking);
        }

        [Test]
        public void Given_I_Call_GetSurveyDataEntrySettings_And_There_Are_No_Settings_Configured_I_Get_An_Empty_List_Back()
        {
            //arrange
            var dataEntrySettingsList = new List<IDataEntrySettings>();

            var dataEntrySettingsCollection = new Mock<IDataEntrySettingsCollection>();
            dataEntrySettingsCollection.Setup(des => des.GetEnumerator())
                .Returns(dataEntrySettingsList.GetEnumerator());

            var datamodelMock = new Mock<IDatamodel>();
            datamodelMock.Setup(d => d.DataEntrySettings).Returns(dataEntrySettingsCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(datamodelMock.Object);

            //act
            var result = _sut.GetSurveyDataEntrySettings(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DataEntrySettingsModel>>(result);
            Assert.IsEmpty(result);
        }
    }
}
