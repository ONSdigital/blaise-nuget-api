namespace Blaise.Nuget.Api.Tests.Unit.Api.Case
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataRecord;

    public class BlaiseCaseApiTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly Dictionary<string, string> _primaryKeyValues;
        private readonly string _serverParkName;
        private readonly string _questionnaireName;
        private readonly string _databaseFile;
        private readonly string _filter = "Id=10";
        private Mock<ICaseService> _caseServiceMock;
        private IBlaiseCaseApi _sut;

        public BlaiseCaseApiTests()
        {
            _connectionModel = new ConnectionModel();
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            _serverParkName = "Park1";
            _questionnaireName = "Questionnaire1";
            _databaseFile = "TestFile";
        }

        [SetUp]
        public void SetUpTests()
        {
            _caseServiceMock = new Mock<ICaseService>();

            _sut = new BlaiseCaseApi(
                _caseServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.That(() => new BlaiseCaseApi(), Throws.Nothing);
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            // act and assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.That(() => new BlaiseCaseApi(new ConnectionModel()), Throws.Nothing);
        }

        [Test]
        public void When_Calling_CaseExists_With_Valid_Arguments_Then_Correct_Service_Method_Should_Be_Called()
        {
            // arrange
            _caseServiceMock.Setup(mock => mock.CaseExists(
                It.IsAny<ConnectionModel>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.CaseExists(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                mock => mock.CaseExists(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName),
                Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            // arrange
            _caseServiceMock.Setup(d => d.CaseExists(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName)).Returns(caseExists);

            // act
            var result = _sut.CaseExists(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.EqualTo(caseExists));
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(null, _questionnaireName, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(
                _primaryKeyValues,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(
                _primaryKeyValues,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(
                _primaryKeyValues,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(It.IsAny<Dictionary<string, string>>());

            // act
            _sut.GetPrimaryKeyValues(dataRecordMock.Object);

            // assert
            _caseServiceMock.Verify(v => v.GetPrimaryKeyValues(dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Expected_Result_Is_Returned()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(_primaryKeyValues);

            // act
            var result = _sut.GetPrimaryKeyValues(dataRecordMock.Object);

            // assert
            Assert.That(result, Is.EqualTo(_primaryKeyValues));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.GetCases(_questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(v => v.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetFilteredCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var filter = "Id=10";
            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.GetFilteredCases(_questionnaireName, _serverParkName, filter);

            // assert
            _caseServiceMock.Verify(v => v.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, filter), Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetFilteredCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFilteredCases(string.Empty, _serverParkName, _filter));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetFilteredCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFilteredCases(null, _serverParkName, _filter));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetFilteredCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFilteredCases(_questionnaireName, string.Empty, _filter));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call__GetFilteredCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFilteredCases(_questionnaireName, null, _filter));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_Filter_When_I_Call_GetFilteredCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFilteredCases(_questionnaireName, _serverParkName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'filter' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Filter_When_I_Call__GetFilteredCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFilteredCases(_questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("filter"));
        }

        [Test]
        public void Given_A_File_When_I_Call_GetCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string databaseFile = "File1.bdix";

            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.GetCases(databaseFile);

            // assert
            _caseServiceMock.Verify(v => v.GetDataSet(_connectionModel, databaseFile, null), Times.Once);
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(null));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCase_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _caseServiceMock.Setup(d => d.GetDataRecord(_connectionModel, It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.GetCase(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(null, _questionnaireName, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCase_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(
                _primaryKeyValues,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(_primaryKeyValues, null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCase_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(
                _primaryKeyValues,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCase_For_A_File_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _caseServiceMock.Setup(d => d.GetDataRecord(_connectionModel, It.IsAny<Dictionary<string, string>>(), It.IsAny<string>()));

            // act
            _sut.GetCase(_primaryKeyValues, _databaseFile);

            // assert
            _caseServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _primaryKeyValues, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_GetCase_For_A_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(null, _databaseFile));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCase_For_A_File_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(
                _primaryKeyValues,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCase_For_A_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(_primaryKeyValues, null));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(_primaryKeyValues, fieldData) };

            _caseServiceMock.Setup(d => d.CreateNewDataRecords(_connectionModel, It.IsAny<List<CaseModel>>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.CreateCases(caseModels, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecords(_connectionModel, caseModels, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void When_Calling_CreateCases_With_Empty_ListOfCases_Then_ArgumentException_Should_Be_Thrown()
        {
            // arrange
            var caseModels = new List<CaseModel>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() =>
                _sut.CreateCases(caseModels, _questionnaireName, _serverParkName));

            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'cases' must be supplied"));
        }

        [Test]
        public void When_Calling_CreateCases_With_Null_ListOfCases_Then_ArgumentNullException_Should_Be_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _sut.CreateCases(null, _questionnaireName, _serverParkName));

            Assert.That(exception.ParamName, Is.EqualTo("cases"));
        }

        [Test]
        public void When_Calling_CreateCases_With_Empty_QuestionnaireName_Then_ArgumentException_Should_Be_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(_primaryKeyValues, fieldData) };
            var emptyQuestionnaireName = string.Empty;

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() =>
                _sut.CreateCases(caseModels, emptyQuestionnaireName, _serverParkName));

            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void When_Calling_CreateCases_With_Null_QuestionnaireName_Then_ArgumentNullException_Should_Be_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(_primaryKeyValues, fieldData) };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCases(caseModels, null, _serverParkName));

            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateCases_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(_primaryKeyValues, fieldData) };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCases(caseModels, _questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void When_CreateCases_Is_Called_With_Null_ServerParkName_Then_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(_primaryKeyValues, fieldData) };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCases(caseModels, _questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.CreateNewDataRecord(_connectionModel, It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.CreateCase(_primaryKeyValues, fieldData, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecord(_connectionModel, _primaryKeyValues, fieldData, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(null, fieldData, _questionnaireName, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                _primaryKeyValues,
                null,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'fieldData' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_CreateCase_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(
                _primaryKeyValues,
                fieldData,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_CallCreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                _primaryKeyValues,
                fieldData,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateCase_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(
                _primaryKeyValues,
                fieldData,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                _primaryKeyValues,
                fieldData,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CreateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.CreateNewDataRecord(
                _connectionModel,
                It.IsAny<IDataRecord>(),
                It.IsAny<string>(),
                It.IsAny<string>()));

            // act
            _sut.CreateCase(dataRecord.Object, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                v => v.CreateNewDataRecord(_connectionModel, dataRecord.Object, _questionnaireName, _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                null,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(
                dataRecord.Object,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_CallCreateCase_With_A_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                dataRecord.Object,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(
                dataRecord.Object,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(
                dataRecord.Object,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_For_Local_Connection_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.CreateNewDataRecord(_connectionModel, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()));

            // act
            _sut.CreateCase(_databaseFile, _primaryKeyValues, fieldData);

            // assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecord(_connectionModel, _databaseFile, _primaryKeyValues, fieldData), Times.Once);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(string.Empty, _primaryKeyValues, fieldData));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(null, _primaryKeyValues, fieldData));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_databaseFile, null, fieldData));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_databaseFile, _primaryKeyValues, null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'fieldData' must be supplied"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.UpdateDataRecord(
                _connectionModel,
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<string>()));

            // act
            _sut.UpdateCase(_primaryKeyValues, fieldData, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                v => v.UpdateDataRecord(
                    _connectionModel,
                    _primaryKeyValues,
                    fieldData,
                    _questionnaireName,
                    _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                (Dictionary<string, string>)null,
                fieldData,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                _primaryKeyValues,
                null,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'fieldData' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UpdateCase_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(
                _primaryKeyValues,
                fieldData,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                _primaryKeyValues,
                fieldData,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateCase_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(
                _primaryKeyValues,
                fieldData,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                _primaryKeyValues,
                fieldData,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateCase_With_DataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.UpdateDataRecord(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.UpdateCase(dataRecordMock.Object, fieldData, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                v => v.UpdateDataRecord(
                    _connectionModel,
                    dataRecordMock.Object,
                    fieldData,
                    _questionnaireName,
                    _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                (IDataRecord)null,
                fieldData,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                null,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'fieldData' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                fieldData,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                fieldData,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                fieldData,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateCase_With_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                fieldData,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateCase_With_DataRecord_And_A_Database_File_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.UpdateDataRecord(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>()));

            // act
            _sut.UpdateCase(dataRecordMock.Object, fieldData, _databaseFile);

            // assert
            _caseServiceMock.Verify(
                v => v.UpdateDataRecord(
                    _connectionModel,
                    dataRecordMock.Object,
                    fieldData,
                    _databaseFile),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_UpdateCase_With_DataRecord_And_A_Database_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                null,
                fieldData,
                _databaseFile));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_UpdateCase_With_DataRecord_And_A_Database_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                null,
                _databaseFile));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'fieldData' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_UpdateCase_With_DataRecord_And_A_Database_File_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(dataRecordMock.Object, fieldData, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_UpdateCase_With_DataRecord_And_A_Database_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(
                dataRecordMock.Object,
                fieldData,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            _caseServiceMock.Setup(d => d.FieldExists(
                _connectionModel,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.FieldExists(_questionnaireName, _serverParkName, fieldName);

            // CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            _caseServiceMock.Setup(d => d.FieldExists(
                _connectionModel,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(fieldExists);

            // act
            var result = _sut.FieldExists(_questionnaireName, _serverParkName, fieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_A_FieldName_And_An_Empty_QuestionnaireName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(
                string.Empty,
                _serverParkName,
                fieldName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_FieldName_And_A_Null_QuestionnaireName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(
                null,
                _serverParkName,
                fieldName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_A_FieldName_And_An_Empty_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(
                _questionnaireName,
                string.Empty,
                fieldName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_FieldName_And_A_Null_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(
                _questionnaireName,
                null,
                fieldName));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_FieldName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(
                _questionnaireName,
                _serverParkName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'fieldName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_FieldName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(
                _questionnaireName,
                _serverParkName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("fieldName"));
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            // arrange
            _caseServiceMock.Setup(d => d.FieldExists(
                _connectionModel,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.FieldExists(_questionnaireName, _serverParkName, fieldNameType);

            // CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldNameType.FullName()), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            // arrange
            _caseServiceMock.Setup(d => d.FieldExists(
                _connectionModel,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(fieldExists);

            // act
            var result = _sut.FieldExists(_questionnaireName, _serverParkName, FieldNameType.HOut);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(
                string.Empty,
                _serverParkName,
                FieldNameType.HOut));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(
                null,
                _serverParkName,
                FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(
                _questionnaireName,
                string.Empty,
                FieldNameType.HOut));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(
                _questionnaireName,
                null,
                FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(It.IsAny<IDataRecord>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            // CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(dataRecordMock.Object, fieldNameType.FullName()), Times.Once);
        }

        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.HOut, false)]
        [TestCase(FieldNameType.Mode, true)]
        [TestCase(FieldNameType.Mode, false)]
        [TestCase(FieldNameType.TelNo, true)]
        [TestCase(FieldNameType.TelNo, false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(FieldNameType fieldNameType, bool fieldExists)
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(dataRecordMock.Object, fieldNameType.FullName())).Returns(fieldExists);

            // act
            var result = _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_FieldExists_With_A_FieldName_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(It.IsAny<IDataRecord>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            // act
            _sut.FieldExists(dataRecordMock.Object, fieldName);

            // CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(dataRecordMock.Object, fieldName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_With_A_FieldName_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(dataRecordMock.Object, fieldName)).Returns(fieldExists);

            // act
            var result = _sut.FieldExists(dataRecordMock.Object, fieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_FieldExists_With_A_FieldName_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, fieldName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        [TestCase(FieldNameType.LastUpdated)]
        public void Given_A_FieldNameType_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            // arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetFieldValue(dataRecordMock.Object, fieldNameType.FullName()))
                .Returns(dataValueMock.Object);

            // act
            var result = _sut.GetFieldValue(dataRecordMock.Object, fieldNameType);

            // assert
            Assert.That(result, Is.EqualTo(dataValueMock.Object));
        }

        [Test]
        public void Given_A_FieldNameType_When_I_Call_GetFieldValue_With_A_Null_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const FieldNameType fieldValueType = FieldNameType.HOut;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, fieldValueType));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetFieldValue_Then_The_Correct_Value()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetFieldValue(dataRecordMock.Object, fieldName))
                .Returns(dataValueMock.Object);

            // act
            var result = _sut.GetFieldValue(dataRecordMock.Object, fieldName);

            // assert
            Assert.That(result, Is.EqualTo(dataValueMock.Object));
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetFieldValue_With_A_Null_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, fieldName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_FieldName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(dataRecordMock.Object, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'fieldName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_FieldName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var dataRecordMock = new Mock<IDataRecord>();

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(dataRecordMock.Object, null));
            Assert.That(exception.ParamName, Is.EqualTo("fieldName"));
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        [TestCase(FieldNameType.LastUpdated)]
        public void Given_A_primaryKeys_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            // arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d =>
                    d.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName))
                .Returns(dataRecordMock.Object);

            _caseServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<string>()))
                .Returns(dataValueMock.Object);

            // act
            var result = _sut.GetFieldValue(_primaryKeyValues, _questionnaireName, _serverParkName, fieldNameType);

            // assert
            Assert.That(result, Is.EqualTo(dataValueMock.Object));
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(
                null,
                _questionnaireName,
                _serverParkName,
                FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_A_primaryKeys_And_An_Empty_QuestionnaireName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(
                _primaryKeyValues,
                string.Empty,
                _serverParkName,
                FieldNameType.HOut));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_primaryKeys_And_A_Null_QuestionnaireName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(
                _primaryKeyValues,
                null,
                _serverParkName,
                FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_A_primaryKeys_And_An_Empty_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty,
                FieldNameType.HOut));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_primaryKeys_And_A_Null_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(
                _primaryKeyValues,
                _questionnaireName,
                null,
                FieldNameType.HOut));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            // act
            _sut.GetNumberOfCases(_questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                v => v.GetNumberOfCases(
                _connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            // arrange
            const int numberOfCases = 5;
            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(numberOfCases);

            // act
            var result = _sut.GetNumberOfCases(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.EqualTo(numberOfCases));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string databaseFile = "File1.bdix";

            _caseServiceMock.Setup(d => d.GetNumberOfCases(_connectionModel, It.IsAny<string>()));

            // act
            _sut.GetNumberOfCases(databaseFile);

            // assert
            _caseServiceMock.Verify(v => v.GetNumberOfCases(_connectionModel, databaseFile), Times.Once);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            // arrange
            const int numberOfCases = 5;

            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel,
                It.IsAny<string>())).Returns(numberOfCases);

            // act
            var result = _sut.GetNumberOfCases(_databaseFile);

            // assert
            Assert.That(result, Is.EqualTo(numberOfCases));
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveCase_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.RemoveCase(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(v => v.RemoveDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(null, _questionnaireName, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(
                _primaryKeyValues,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var primaryKey = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(primaryKey, null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(
                _primaryKeyValues,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveCases_Then_The_Correct_Service_Method_Is_Called()
        {
            // act
            _sut.RemoveCases(_questionnaireName, _serverParkName);

            // assert
            _caseServiceMock.Verify(
                v => v.RemoveDataRecords(_connectionModel, _questionnaireName, _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_RemoveCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCases(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_RemoveCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCases(
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveCases_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCases(
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCases(
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetRecordDataFields_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.GetFieldDataFromRecord(It.IsAny<IDataRecord>()));

            // act
            _sut.GetRecordDataFields(dataRecord.Object);

            // assert
            _caseServiceMock.Verify(
                v => v.GetFieldDataFromRecord(dataRecord.Object),
                Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetRecordDataFields_Then_The_Expected_Value_Is_Returned()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            var fieldDictionary = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.GetFieldDataFromRecord(It.IsAny<IDataRecord>()))
                .Returns(fieldDictionary);

            // act
            var result = _sut.GetRecordDataFields(dataRecord.Object);

            // assert
            Assert.That(result, Is.InstanceOf<Dictionary<string, string>>());
            Assert.That(result, Is.EqualTo(fieldDictionary));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetRecordDataFields_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetRecordDataFields(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var outcomeCode = 110;
            var dataRecord = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetOutcomeCode(It.IsAny<IDataRecord>())).Returns(outcomeCode);

            // act
            _sut.GetOutcomeCode(dataRecord.Object);

            // assert
            _caseServiceMock.Verify(v => v.GetOutcomeCode(dataRecord.Object), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Expected_Value_Is_Returned()
        {
            // arrange
            var outcomeCode = 110;
            var dataRecord = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetOutcomeCode(It.IsAny<IDataRecord>())).Returns(outcomeCode);

            // act
            var result = _sut.GetOutcomeCode(dataRecord.Object);

            // assert
            Assert.That(result, Is.InstanceOf<int>());
            Assert.That(result, Is.EqualTo(outcomeCode));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetOutcomeCode_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetOutcomeCode(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_LockDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string lockId = "Lock123";

            _caseServiceMock.Setup(
                d => d.LockDataRecord(
                    It.IsAny<ConnectionModel>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            // act
            _sut.LockDataRecord(_primaryKeyValues, _questionnaireName, _serverParkName, lockId);

            // assert
            _caseServiceMock.Verify(
                v => v.LockDataRecord(
                    _connectionModel,
                    _primaryKeyValues,
                    _questionnaireName,
                    _serverParkName,
                    lockId),
                Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_LockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.LockDataRecord(
                null,
                _questionnaireName,
                _serverParkName,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_LockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.LockDataRecord(
                _primaryKeyValues,
                string.Empty,
                _serverParkName,
                lockId));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_LockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.LockDataRecord(
                _primaryKeyValues,
                null,
                _serverParkName,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_LockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.LockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty,
                lockId));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_LockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.LockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                null,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_LockId_When_I_Call_LockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.LockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                _serverParkName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'lockId' must be supplied"));
        }

        [Test]
        public void Given_A_Null_LockId_When_I_Call_LockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.LockDataRecord(_primaryKeyValues, _questionnaireName, _serverParkName, null));
            Assert.That(exception.ParamName, Is.EqualTo("lockId"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UnLockDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            const string lockId = "Lock123";

            _caseServiceMock.Setup(
                d => d.UnLockDataRecord(
                    It.IsAny<ConnectionModel>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            // act
            _sut.UnLockDataRecord(_primaryKeyValues, _questionnaireName, _serverParkName, lockId);

            // assert
            _caseServiceMock.Verify(
                v => v.UnLockDataRecord(
                    _connectionModel,
                    _primaryKeyValues,
                    _questionnaireName,
                    _serverParkName,
                    lockId),
                Times.Once);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_UnLockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UnLockDataRecord(
                null,
                _questionnaireName,
                _serverParkName,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_UnLockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                string.Empty,
                _serverParkName,
                lockId));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_UnLockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                null,
                _serverParkName,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UnLockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty,
                lockId));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UnLockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            const string lockId = "Lock123";

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                null,
                lockId));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_LockId_When_I_Call_UnLockDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                _serverParkName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'lockId' must be supplied"));
        }

        [Test]
        public void Given_A_Null_LockId_When_I_Call_UnLockDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UnLockDataRecord(
                _primaryKeyValues,
                _questionnaireName,
                _serverParkName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("lockId"));
        }

        [Test]
        public void Given_A_Record_Is_Locked_When_I_Call_DataRecordIsLocked_Then_True_Is_returned()
        {
            // arrange
            _caseServiceMock.Setup(d => d.GetDataRecord(
                It.IsAny<ConnectionModel>(),
                _primaryKeyValues,
                _questionnaireName,
                _serverParkName)).Throws(new Exception());

            // act
            var result = _sut.DataRecordIsLocked(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_A_Record_Is_Not_Locked_When_I_Call_DataRecordIsLocked_Then_False_Is_returned()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.GetDataRecord(
                _connectionModel,
                _primaryKeyValues,
                _questionnaireName,
                _serverParkName)).Returns(dataRecord.Object);

            // act
            var result = _sut.DataRecordIsLocked(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_DataRecordIsLocked_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DataRecordIsLocked(
                null,
                _questionnaireName,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_DataRecordIsLocked_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DataRecordIsLocked(
                _primaryKeyValues,
                string.Empty,
                _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_DataRecordIsLocked_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DataRecordIsLocked(
                _primaryKeyValues,
                null,
                _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_DataRecordIsLocked_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.DataRecordIsLocked(
                _primaryKeyValues,
                _questionnaireName,
                string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_DataRecordIsLocked_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DataRecordIsLocked(
                _primaryKeyValues,
                _questionnaireName,
                null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetLastUpdated_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.GetLastUpdated(It.IsAny<IDataRecord>()));

            // act
            _sut.GetLastUpdated(dataRecord.Object);

            // assert
            _caseServiceMock.Verify(
                v => v.GetLastUpdated(dataRecord.Object),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetLastUpdated_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetLastUpdated(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetLastUpdatedAsString_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.GetLastUpdatedAsString(It.IsAny<IDataRecord>()));

            // act
            _sut.GetLastUpdatedAsString(dataRecord.Object);

            // assert
            _caseServiceMock.Verify(
                v => v.GetLastUpdatedAsString(dataRecord.Object),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetLastUpdatedAsString_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetLastUpdatedAsString(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CaseInUseInCati_Then_The_Correct_Service_Method_Is_Called()
        {
            // arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.CaseInUseInCati(It.IsAny<IDataRecord>()));

            // act
            _sut.CaseInUseInCati(dataRecord.Object);

            // assert
            _caseServiceMock.Verify(
                v => v.CaseInUseInCati(dataRecord.Object),
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_CaseInUseInCati_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseInUseInCati(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_MapCaseStatusModel_Then_An_Expected_CaseStatusModel_Is_Returned()
        {
            // arrange
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var caseStatusModel = new CaseStatusModel(_primaryKeyValues, outCome, lastUpdated);
            var dataRecord = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetCaseStatus(dataRecord.Object)).Returns(caseStatusModel);

            // act
            var result = _sut.GetCaseStatus(dataRecord.Object);

            // assert
            Assert.That(result, Is.InstanceOf<CaseStatusModel>());
            Assert.That(result.PrimaryKeyValues, Is.EqualTo(_primaryKeyValues));
            Assert.That(result.Outcome, Is.EqualTo(outCome));
            Assert.That(result.LastUpdated, Is.EqualTo(lastUpdated));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetCaseStatus_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseStatus(null));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'dataRecord' must be supplied"));
        }

        [Test]
        public void Given_A_Valid_Arguments_When_I_Call_GetCaseStatusModelList_Then_The_Expected_List_Of_CaseStatusModels_Is_Returned()
        {
            // arrange
            var caseStatusModelList = new List<CaseStatusModel>();

            _caseServiceMock.Setup(d => d.GetCaseStatusModelList(_connectionModel, _questionnaireName, _serverParkName)).Returns(caseStatusModelList);

            // act
            var result = _sut.GetCaseStatusModelList(_questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<IEnumerable<CaseStatusModel>>());
            Assert.That(result, Is.EqualTo(caseStatusModelList));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCaseStatusModelList_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseStatusModelList(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCaseStatusModelList_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseStatusModelList(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCaseStatusModelList_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseStatusModelList(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCaseStatusModelList_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseStatusModelList(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_A_Valid_Arguments_When_I_Call_GetCaseStatusModelList_For_A_File_Then_The_Expected_List_Of_CaseStatusModels_Is_Returned()
        {
            // arrange
            var caseStatusModelList = new List<CaseStatusModel>();

            _caseServiceMock.Setup(d => d.GetCaseStatusModelList(_connectionModel, _databaseFile)).Returns(caseStatusModelList);

            // act
            var result = _sut.GetCaseStatusModelList(_databaseFile);

            // assert
            Assert.That(result, Is.InstanceOf<IEnumerable<CaseStatusModel>>());
            Assert.That(result, Is.EqualTo(caseStatusModelList));
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_GetCaseStatusModelList_For_A_File_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseStatusModelList(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'databaseFile' must be supplied"));
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_GetCaseStatusModelList_For_A_File_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseStatusModelList(null));
            Assert.That(exception.ParamName, Is.EqualTo("databaseFile"));
        }

        [Test]
        public void Given_A_Valid_Arguments_When_I_Call_GetCaseModel_Then_The_Expected_List_Of_CaseStatusModels_Is_Returned()
        {
            // arrange
            var caseModel = new CaseModel(_primaryKeyValues, new Dictionary<string, string>());

            _caseServiceMock.Setup(d => d.GetCaseModel(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName)).Returns(caseModel);

            // act
            var result = _sut.GetCaseModel(_primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<CaseModel>());
            Assert.That(result, Is.EqualTo(caseModel));
        }

        [Test]
        public void Given_Null_PrimaryKeysValues_When_I_Call_GetCaseModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseModel(null, _questionnaireName, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("The argument 'primaryKeyValues' must be supplied"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCaseModel_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseModel(_primaryKeyValues, string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCaseModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseModel(_primaryKeyValues, null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCaseModel_Then_An_ArgumentException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseModel(_primaryKeyValues, _questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCaseModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseModel(_primaryKeyValues, _questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }
    }
}
