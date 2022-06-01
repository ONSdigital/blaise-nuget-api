using System;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Tests.Unit.Api.File
{
    public class BlaiseFileApiTests
    {
        private Mock<IFileService> _fileServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly string _questionnaireFile;

        private IBlaiseFileApi _sut;

        public BlaiseFileApiTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "Questionnaire1";
            _serverParkName = "Park1";
            _questionnaireFile = "OPN2021a.zip";
        }
        [SetUp]
        public void SetUpTests()
        {
            _fileServiceMock = new Mock<IFileService>();

            _sut = new BlaiseFileApi(
                _fileServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseFileApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseFileApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseFileApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseFileApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_CreateDatabaseFile_The_Correct_Services_Are_Called()
        {
            //arrange

            _fileServiceMock.Setup(f => f.UpdateQuestionnaireFileWithData(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateQuestionnaireFileWithData(_serverParkName, _questionnaireName, _questionnaireFile);

            //assert
            _fileServiceMock.Verify(f => f.UpdateQuestionnaireFileWithData(_connectionModel,
                _questionnaireFile,_questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateQuestionnaireFileWithData(string.Empty, 
                _questionnaireName, _questionnaireFile));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateQuestionnaireFileWithData(null, 
                _questionnaireName, _questionnaireFile));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateQuestionnaireFileWithData(_serverParkName,
                string.Empty, _questionnaireFile));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateQuestionnaireFileWithData(_serverParkName, 
                null, _questionnaireFile));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateQuestionnaireFileWithData(_serverParkName, 
                _questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'questionnaireFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_UpdateQuestionnaireFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateQuestionnaireFileWithData(_serverParkName, 
                _questionnaireName, null));
            Assert.AreEqual("questionnaireFile", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_The_Correct_Services_Are_Called()
        {
            //arrange
            _fileServiceMock.Setup(f => f.UpdateQuestionnairePackageWithSqlConnection(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateQuestionnaireFileWithSqlConnection(_questionnaireName, _questionnaireFile);

            //assert
            _fileServiceMock.Verify(f => f.UpdateQuestionnairePackageWithSqlConnection(_questionnaireName,
                _questionnaireFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateQuestionnaireFileWithSqlConnection(string.Empty, _questionnaireFile));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateQuestionnaireFileWithSqlConnection(null, _questionnaireFile));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireFile_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateQuestionnaireFileWithSqlConnection(_questionnaireName, 
                string.Empty));
            Assert.AreEqual("A value for the argument 'questionnaireFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_QuestionnaireFile_When_I_Call_UpdateQuestionnaireFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateQuestionnaireFileWithSqlConnection(_questionnaireName, null));
            Assert.AreEqual("questionnaireFile", exception.ParamName);
        }

        [TestCase(ApplicationType.Cati)]
        [TestCase(ApplicationType.AuditTrail)]
        [TestCase(ApplicationType.Cari)]
        [TestCase(ApplicationType.Session)]
        [TestCase(ApplicationType.Configuration)]
        [TestCase(ApplicationType.Meta)]
        public void Given_Valid_Parameters_When_I_Call_CreateSettingsDataInterfaceFile_The_Correct_Services_Are_Called(ApplicationType applicationType)
        {
            //arrange
            var fileName = "OPN2101a.bcdi";
            _fileServiceMock.Setup(f => f.CreateSettingsDataInterfaceFile(It.IsAny<ApplicationType>(), It.IsAny<string>()));

            //act
            _sut.CreateSettingsDataInterfaceFile(applicationType, fileName);

            //assert
            _fileServiceMock.Verify(f => f.CreateSettingsDataInterfaceFile(applicationType,
                fileName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_FileName_When_I_Call_CreateSettingsDataInterfaceFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateSettingsDataInterfaceFile(ApplicationType.Cati, string.Empty));
            Assert.AreEqual("A value for the argument 'fileName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FileName_When_I_Call_CreateSettingsDataInterfaceFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateSettingsDataInterfaceFile(ApplicationType.Cati, null));
            Assert.AreEqual("fileName", exception.ParamName);
        }
    }
}
