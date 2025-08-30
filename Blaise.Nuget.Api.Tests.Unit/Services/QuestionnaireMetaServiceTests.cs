namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.Meta;
    using System.Collections.Generic;
    using System.Linq;

    public class QuestionnaireMetaServiceTests
    {
        private ConnectionModel _connectionModel;

        private string _questionnaireName;

        private string _serverParkName;

        private Mock<IDataModelService> _dataModelServiceMock;

        private IQuestionnaireMetaService _sut;

        [SetUp]
        public void SetupTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";

            _dataModelServiceMock = new Mock<IDataModelService>();

            _sut = new QuestionnaireMetaService(_dataModelServiceMock.Object);
        }

        [Test]
        public void Given_A_Questionnaire_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetQuestionnaireModes_The_Correct_Modes_Are_Returned()
        {
            // arrange
            const string mode1 = "CATI";
            var mode1Mock = new Mock<IMode>();
            mode1Mock.Setup(m => m.Name).Returns(mode1);

            const string mode2 = "CAWI";
            var mode2Mock = new Mock<IMode>();
            mode2Mock.Setup(m => m.Name).Returns(mode2);

            var modelList = new List<IMode> { mode1Mock.Object, mode2Mock.Object };

            var modeCollection = new Mock<IModeCollection>();
            modeCollection.Setup(m => m.GetEnumerator()).Returns(modelList.GetEnumerator());

            var dataModelMock = new Mock<IDatamodel2>();
            dataModelMock.Setup(dm => dm.Modes).Returns(modeCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(
                _connectionModel,
                _questionnaireName,
                _serverParkName)).Returns(dataModelMock.Object);

            // act
            var result = _sut.GetQuestionnaireModes(_connectionModel, _questionnaireName, _serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Does.Contain("CAWI"));
            Assert.That(result, Does.Contain("CATI"));
        }

        [Test]
        public void Given_A_Questionnaire_Has_No_DataModel_When_I_Call_GetQuestionnaireModes_Then_An_Empty_List_Is_Returned()
        {
            // arrange
            _dataModelServiceMock.Setup(dm => dm.GetDataModel(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(null as IDatamodel);

            // act
            var result = _sut.GetQuestionnaireModes(_connectionModel, _questionnaireName, _serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_List_Of_DataEntrySettingsModel_Back()
        {
            // arrange
            var dataEntrySettingsList = new List<IDataEntrySettings>();

            var dataEntrySettingsCollection = new Mock<IDataEntrySettingsCollection>();
            dataEntrySettingsCollection.Setup(des => des.GetEnumerator())
                .Returns(dataEntrySettingsList.GetEnumerator());

            var datamodelMock = new Mock<IDatamodel>();
            datamodelMock.Setup(d => d.DataEntrySettings).Returns(dataEntrySettingsCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(datamodelMock.Object);

            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DataEntrySettingsModel>>());
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_I_Get_A_Valid_List_Of_DataEntrySettingsModel_Back()
        {
            // arrange

            // data entry settings 1
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

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(datamodelMock.Object);

            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DataEntrySettingsModel>>());
            Assert.That(result.Count, Is.EqualTo(2));

            var dataEntrySettings1 = result.FirstOrDefault(n => n.Type == dataEntrySettings1Name);
            Assert.That(dataEntrySettings1, Is.Not.Null);
            Assert.That(dataEntrySettings1.SessionTimeout, Is.EqualTo(dataEntrySettings1Timeout));
            Assert.That(dataEntrySettings1.SaveSessionOnTimeout, Is.True);
            Assert.That(dataEntrySettings1.SaveSessionOnQuit, Is.False);
            Assert.That(dataEntrySettings1.DeleteSessionOnTimeout, Is.True);
            Assert.That(dataEntrySettings1.DeleteSessionOnQuit, Is.False);
            Assert.That(dataEntrySettings1.ApplyRecordLocking, Is.True);

            var dataEntrySettings2 = result.FirstOrDefault(n => n.Type == dataEntrySettings2Name);
            Assert.That(dataEntrySettings2, Is.Not.Null);
            Assert.That(dataEntrySettings2.SessionTimeout, Is.EqualTo(dataEntrySettings2Timeout));
            Assert.That(dataEntrySettings2.SaveSessionOnTimeout, Is.False);
            Assert.That(dataEntrySettings2.SaveSessionOnQuit, Is.True);
            Assert.That(dataEntrySettings2.DeleteSessionOnTimeout, Is.False);
            Assert.That(dataEntrySettings2.DeleteSessionOnQuit, Is.True);
            Assert.That(dataEntrySettings2.ApplyRecordLocking, Is.False);
        }

        [Test]
        public void Given_I_Call_GetQuestionnaireDataEntrySettings_And_There_Are_No_Settings_Configured_I_Get_An_Empty_List_Back()
        {
            // arrange
            var dataEntrySettingsList = new List<IDataEntrySettings>();

            var dataEntrySettingsCollection = new Mock<IDataEntrySettingsCollection>();
            dataEntrySettingsCollection.Setup(des => des.GetEnumerator())
                .Returns(dataEntrySettingsList.GetEnumerator());

            var datamodelMock = new Mock<IDatamodel>();
            datamodelMock.Setup(d => d.DataEntrySettings).Returns(dataEntrySettingsCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(
                It.IsAny<ConnectionModel>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(datamodelMock.Object);

            // act
            var result = _sut.GetQuestionnaireDataEntrySettings(_connectionModel, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<DataEntrySettingsModel>>());
            Assert.That(result, Is.Empty);
        }
    }
}
