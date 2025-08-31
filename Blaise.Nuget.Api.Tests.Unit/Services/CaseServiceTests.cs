namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class CaseServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly string _databaseFile;
        private readonly string _keyName;
        private readonly Dictionary<string, string> _primaryKeyValues;
        private Mock<IDataModelService> _dataModelServiceMock;
        private Mock<IKeyService> _keyServiceMock;
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IFieldService> _fieldServiceMock;
        private Mock<IDataRecordMapper> _mapperServiceMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;
        private CaseService _sut;

        public CaseServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _databaseFile = "c:\\filePath\\opn2010.bdbx";
            _keyName = "TestKeyName";
            _primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _dataModelServiceMock = new Mock<IDataModelService>();
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, _questionnaireName, _serverParkName)).Returns(_dataModelMock.Object);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, _databaseFile)).Returns(_dataModelMock.Object);

            _keyServiceMock = new Mock<IKeyService>();
            _keyServiceMock.Setup(d => d.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName)).Returns(true);
            _keyServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);
            _keyServiceMock.Setup(d => d.GetPrimaryKey(_dataModelMock.Object)).Returns(_keyMock.Object);

            _dataRecordServiceMock = new Mock<IDataRecordService>();
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName))
                .Returns(_dataRecordMock.Object);

            _fieldServiceMock = new Mock<IFieldService>();

            _mapperServiceMock = new Mock<IDataRecordMapper>();

            _sut = new CaseService(
                _dataModelServiceMock.Object,
                _dataRecordServiceMock.Object,
                _keyServiceMock.Object,
                _fieldServiceMock.Object,
                _mapperServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValues_Then_The_Correct_Key_Is_Returned()
        {
            // act
            var primaryKeys = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(primaryKeys);

            var result = _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            // assert
            Assert.That(result, Is.SameAs(primaryKeys));
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(It.IsAny<Dictionary<string, string>>());

            // act
            _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            // assert
            _keyServiceMock.Verify(v => v.GetPrimaryKeyValues(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null);

            // assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null), Times.Once);
        }

        [Test]
        public void Given_A_File_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataSet(_connectionModel, _databaseFile, null);

            // assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _databaseFile, null), Times.Once);
        }

        [Test]
        public void Given_PrimaryKeyValues_And_An_QuestionnaireName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Record_Does_Not_Exist_When_I_Call_GetDataRecord_Then_A_DataNotFoundException_Is_Thrown()
        {
            // arrange
            _keyServiceMock.Setup(k => k.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName))
                .Returns(false);

            // act and assert
            Assert.Throws<DataNotFoundException>(() =>
                _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName));
        }

        [Test]
        public void Given_PrimaryKeyValues_And_A_DatabaseFile_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _databaseFile);

            // assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _databaseFile, _keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_DatabaseFile_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            // act
            _sut.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile);

            // assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";

            // act
            _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            // assert
            _fieldServiceMock.Verify(v => v.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            _fieldServiceMock.Setup(f => f.FieldExists(
                _connectionModel,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns(fieldExists);

            // act
            var result = _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called_For_DataRecord()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldName)).Returns(true);

            // act
            _sut.FieldExists(_dataRecordMock.Object, fieldName);

            // assert
            _fieldServiceMock.Verify(v => v.FieldExists(_dataRecordMock.Object, fieldName), Times.Once);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetFieldValue_Then_The_Correct_DataModel_Is_Returned()
        {
            // arrange
            const string fieldName = "QHAdmin.HOut";
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldName)).Returns(fieldMock.Object);

            // act
            var result = _sut.GetFieldValue(_dataRecordMock.Object, fieldName);

            // assert
            Assert.That(result, Is.EqualTo(dataValueMock.Object));
        }

        [Test]
        public void Given_The_Date_Field_Does_Not_Exist_When_I_Call_GetLastUpdated_Then_Null_Is_Returned()
        {
            // arrange

            // setup date
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate.FullName()))
                .Returns(false);

            // setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns("09:23:59");
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime.FullName()))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime.FullName()))
                .Returns(timeFieldMock.Object);

            // act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_GetCaseStatus_Then_An_Expected_CaseStatusModel_Is_Returned()
        {
            // arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(primaryKeyValues);

            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock.Setup(f => f.GetField(It.IsAny<IDataRecord>(), FieldNameType.HOut.FullName()).DataValue).Returns(outcomeFieldValue.Object);

            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName())).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName())).Returns(dateFieldMock.Object);

            // act
            var result = _sut.GetCaseStatus(_dataRecordMock.Object);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CaseStatusModel>());
            Assert.That(result.PrimaryKeyValues, Is.EqualTo(primaryKeyValues));
            Assert.That(result.Outcome, Is.EqualTo(outCome));
            Assert.That(result.LastUpdated, Is.EqualTo(lastUpdated));
        }

        [Test]
        public void Given_A_Valid_DataSet_When_I_Call_GetCaseStatusModelList_Then_An_Expected_List_Of_CaseStatusModel_Is_Returned()
        {
            // arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            // Setup mocks for KeyService and FieldService
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(_dataRecordMock.Object)).Returns(primaryKeyValues);

            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.HOut.FullName()).DataValue).Returns(outcomeFieldValue.Object);

            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);

            // Setup FieldService mocks for LastUpdated field
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName())).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName())).Returns(dateFieldMock.Object);

            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                    .Returns(false)
                    .Returns(false)
                    .Returns(true);

            // Setup DataRecordService mock
            _dataRecordServiceMock.Setup(d => d.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null))
                .Returns(dataSetMock.Object);

            // act
            var result = _sut.GetCaseStatusModelList(_connectionModel, _questionnaireName, _serverParkName).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<CaseStatusModel>>());
            Assert.That(result.Count, Is.EqualTo(2));

            foreach (var caseStatusModel in result)
            {
                // Verify the properties of the CaseStatusModel
                Assert.That(caseStatusModel.PrimaryKeyValues, Is.EqualTo(primaryKeyValues));
                Assert.That(caseStatusModel.Outcome, Is.EqualTo(outCome));
                Assert.That(caseStatusModel.LastUpdated, Is.EqualTo(lastUpdated));
            }
        }

        [Test]
        public void Given_A_Valid_DataSet_When_I_Call_GetCaseStatusModelList_For_A_File_Then_An_Expected_List_Of_CaseStatusModel_Is_Returned()
        {
            // arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            // Mock Key Service
            _keyServiceMock
                .Setup(k => k.GetPrimaryKeyValues(_dataRecordMock.Object))
                .Returns(primaryKeyValues);

            // Mock Outcome Field
            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock
                .Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.HOut.FullName()).DataValue)
                .Returns(outcomeFieldValue.Object);

            // Mock Date Field
            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);
            _fieldServiceMock
                .Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName()))
                .Returns(true);
            _fieldServiceMock
                .Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated.FullName()))
                .Returns(dateFieldMock.Object);

            // Mock DataSet
            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(false)
                .Returns(true);
            _dataRecordServiceMock
                .Setup(d => d.GetDataSet(_connectionModel, _databaseFile, null))
                .Returns(dataSetMock.Object);

            // act
            var result = _sut.GetCaseStatusModelList(_connectionModel, _databaseFile).ToList();

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<CaseStatusModel>>());
            Assert.That(result.Count, Is.EqualTo(2));

            foreach (var caseStatusModel in result)
            {
                Assert.That(caseStatusModel.PrimaryKeyValues, Is.EqualTo(primaryKeyValues));
                Assert.That(caseStatusModel.Outcome, Is.EqualTo(outCome));
                Assert.That(caseStatusModel.LastUpdated, Is.EqualTo(lastUpdated));
            }
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_GetCaseModel_Then_An_Expected_CaseModel_Is_Returned()
        {
            // arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            var fieldDictionary = new Dictionary<string, string>();

            _mapperServiceMock
                .Setup(m => m.MapFieldDictionaryFromRecord(_dataRecordMock.Object))
                .Returns(fieldDictionary);

            // act
            var result = _sut.GetCaseModel(_connectionModel, primaryKeyValues, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CaseModel>());
            Assert.That(result.PrimaryKeyValues, Is.EqualTo(primaryKeyValues));
            Assert.That(result.FieldData, Is.SameAs(fieldDictionary));
        }

        private void SetupField(FieldNameType fieldType, string fieldValue)
        {
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            dataValueMock.Setup(d => d.ValueAsText).Returns(fieldValue);
            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);

            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldType.FullName()))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldType.FullName()))
                .Returns(fieldMock.Object);
        }

        private void MockFieldWithMockDataValue(FieldNameType fieldType, string fieldValue)
        {
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            dataValueMock.Setup(d => d.ValueAsText).Returns(fieldValue);
            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);

            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldType.FullName())).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldType.FullName())).Returns(fieldMock.Object);
        }
    }
}
