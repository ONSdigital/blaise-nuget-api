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
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _instrumentFile;

        private IBlaiseFileApi _sut;

        public BlaiseFileApiTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _instrumentFile = "OPN2021a.zip";
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

            _fileServiceMock.Setup(f => f.UpdateInstrumentFileWithData(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateInstrumentFileWithData(_serverParkName, _instrumentName, _instrumentFile);

            //assert
            _fileServiceMock.Verify(f => f.UpdateInstrumentFileWithData(_connectionModel,
                _instrumentFile,_instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithData(string.Empty, 
                _instrumentName, _instrumentFile));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithData(null, 
                _instrumentName, _instrumentFile));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithData(_serverParkName,
                string.Empty, _instrumentFile));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithData(_serverParkName, 
                null, _instrumentFile));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithData(_serverParkName, 
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_UpdateInstrumentFileWithData_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithData(_serverParkName, 
                _instrumentName, null));
            Assert.AreEqual("instrumentFile", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_UpdateInstrumentFileWithSqlConnection_The_Correct_Services_Are_Called()
        {
            //arrange
            _fileServiceMock.Setup(f => f.UpdateInstrumentPackageWithSqlConnection(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, _instrumentFile);

            //assert
            _fileServiceMock.Verify(f => f.UpdateInstrumentPackageWithSqlConnection(_instrumentName,
                _instrumentFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(string.Empty, _instrumentFile));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(null, _instrumentFile));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, 
                string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, null));
            Assert.AreEqual("instrumentFile", exception.ParamName);
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
