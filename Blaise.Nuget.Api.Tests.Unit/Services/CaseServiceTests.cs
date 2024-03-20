using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
// ReSharper disable MissingXmlDoc

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class CaseServiceTests
    {
        private Mock<IDataModelService> _dataModelServiceMock;
        private Mock<IKeyService> _keyServiceMock;
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IFieldService> _fieldServiceMock;
        private Mock<IDataRecordMapper> _mapperServiceMock;

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly string _databaseFile;
        private readonly string _keyName;
        private readonly Dictionary<string, string> _primaryKeyValues;


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
            //act
            var primaryKeys = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(primaryKeys);

            var result = _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            //assert
            Assert.AreSame(primaryKeys, result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(It.IsAny<Dictionary<string, string>>());

            //act
            _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            //assert
            _keyServiceMock.Verify(v => v.GetPrimaryKeyValues(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataSet(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_File_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataSet(_connectionModel, _databaseFile);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_PrimaryKeyValues_And_An_QuestionnaireName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Record_Does_Not_Exist_When_I_Call_GetDataRecord_Then_A_DataNotFoundException_Is_Thrown()
        {
            //arrange
            _keyServiceMock.Setup(k => k.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName))
                .Returns(false);


            //act && assert
            Assert.Throws<DataNotFoundException>(() =>
                _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName));
        }

        [Test]
        public void Given_PrimaryKeyValues_And_A_DatabaseFile_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_connectionModel, _primaryKeyValues, _databaseFile);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _databaseFile, _keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_DatabaseFile_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile), Times.Once);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_FieldNameType_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //act
            _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_FieldNameType_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<FieldNameType>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, FieldNameType.HOut);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";

            //act
            _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            _fieldServiceMock.Setup(f => f.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeys = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };

            //act
            _sut.CaseExists(_connectionModel, primaryKeys, _questionnaireName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValues(_keyMock.Object, primaryKeys), Times.Once);
            _keyServiceMock.Verify(v => v.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Value_Is_Returned(bool caseExists)
        {
            //arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };

            _keyServiceMock.Setup(k => k.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName))
                .Returns(caseExists);

            //act
            var result = _sut.CaseExists(_connectionModel, primaryKeyValues, _questionnaireName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecords_Then_The_Correct_Services_Are_Called_For_One_Case()
        {
            // Arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(primaryKeyValues, fieldData) };
            var dataRecordsMock = new List<IDataRecord> { _dataRecordMock.Object };

            _mapperServiceMock
                .Setup(mapper => mapper.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValues, fieldData))
                .Returns(_dataRecordMock.Object);

            // Act
            _sut.CreateNewDataRecords(_connectionModel, caseModels, _questionnaireName, _serverParkName);

            // Assert
            _dataModelServiceMock.Verify(dataService => dataService.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(keyService => keyService.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(dataRecordService => dataRecordService.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(mapper => mapper.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValues, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(dataRecordService => dataRecordService.WriteDataRecords(_connectionModel, dataRecordsMock, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecords_Then_The_Correct_Services_Are_Called_For_Two_Cases()
        {
            // Arrange
            var primaryKeyValues1 = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var primaryKeyValues2 = new Dictionary<string, string> { { "QID.Serial_Number", "900002" } };
            var fieldData1 = new Dictionary<string, string>();
            var fieldData2 = new Dictionary<string, string>();
            var caseModels = new List<CaseModel> { new CaseModel(primaryKeyValues1, fieldData1), new CaseModel(primaryKeyValues2, fieldData2) };

            var dataRecord1Mock = new Mock<IDataRecord>();
            var dataRecord2Mock = new Mock<IDataRecord>();

            var dataRecordsMock = new List<IDataRecord> { dataRecord1Mock.Object, dataRecord2Mock.Object };

            _dataRecordServiceMock
                .SetupSequence(d => d.GetDataRecord(_dataModelMock.Object))
                .Returns(dataRecord1Mock.Object)
                .Returns(dataRecord2Mock.Object);

            _mapperServiceMock
                .Setup(mapper => mapper.MapDataRecordFields(dataRecord1Mock.Object, _keyMock.Object, primaryKeyValues1, fieldData1))
                .Returns(dataRecord1Mock.Object);

            _mapperServiceMock
                .Setup(mapper => mapper.MapDataRecordFields(dataRecord2Mock.Object, _keyMock.Object, primaryKeyValues2, fieldData2))
                .Returns(dataRecord2Mock.Object);

            // Act
            _sut.CreateNewDataRecords(_connectionModel, caseModels, _questionnaireName, _serverParkName);

            // Assert
            _dataModelServiceMock.Verify(dataService => dataService.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(keyService => keyService.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(dataRecordService => dataRecordService.GetDataRecord(_dataModelMock.Object), Times.Exactly(2));
            _mapperServiceMock.Verify(mapper => mapper.MapDataRecordFields(dataRecord1Mock.Object, _keyMock.Object, primaryKeyValues1, fieldData1), Times.Once);
            _mapperServiceMock.Verify(mapper => mapper.MapDataRecordFields(dataRecord2Mock.Object, _keyMock.Object, primaryKeyValues2, fieldData2), Times.Once);
            _dataRecordServiceMock.Verify(dataRecordService => dataRecordService.WriteDataRecords(_connectionModel, dataRecordsMock, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<IKey>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_connectionModel, primaryKeyValues, fieldData, _questionnaireName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValues, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CreateNewDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange

            //act
            _sut.CreateNewDataRecord(_connectionModel, _dataRecordMock.Object, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _questionnaireName, _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<IKey>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_connectionModel, _databaseFile, primaryKeyValues, fieldData);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _databaseFile), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValues, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900001" } };
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, primaryKeyValues, fieldData, _questionnaireName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValues(_keyMock.Object, primaryKeyValues), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object,
                _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, _dataRecordMock.Object, fieldData, _questionnaireName, _serverParkName);

            //assert
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_And_A_FileName_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, _dataRecordMock.Object, fieldData, _databaseFile);

            //assert
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_A_KeyValue_And_An_QuestionnaireName_And_ServerParkName_When_I_Call_RemoveDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveDataRecord(_connectionModel, _primaryKeyValues, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.DeleteDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_QuestionnaireName_And_ServerParkName_When_I_Call_RemoveDataRecords_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveDataRecords(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.DeleteDataRecords(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_FieldNameType_When_I_Call_GetFieldValue_Then_The_Correct_DataModel_Is_Returned(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldNameType)).Returns(fieldMock.Object);

            //act
            var result = _sut.GetFieldValue(_dataRecordMock.Object, fieldNameType);

            //assert
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_FieldNameType_When_I_Call_GetFieldValue_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldNameType)).Returns(fieldMock.Object);

            //act
            _sut.GetFieldValue(_dataRecordMock.Object, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.GetField(_dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetFieldValue_Then_The_Correct_DataModel_Is_Returned()
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldName)).Returns(fieldMock.Object);

            //act
            var result = _sut.GetFieldValue(_dataRecordMock.Object, fieldName);

            //assert
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetFieldValue_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldName)).Returns(fieldMock.Object);

            //act
            _sut.GetFieldValue(_dataRecordMock.Object, fieldName);

            //assert
            _fieldServiceMock.Verify(v => v.GetField(_dataRecordMock.Object, fieldName), Times.Once);
        }

        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.HOut, false)]
        [TestCase(FieldNameType.Mode, true)]
        [TestCase(FieldNameType.Mode, false)]
        [TestCase(FieldNameType.TelNo, true)]
        [TestCase(FieldNameType.TelNo, false)]
        public void Given_I_Call_FieldExists_Then_The_Correct_DataModel_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldNameType)).Returns(exists);

            //act
            var result = _sut.FieldExists(_dataRecordMock.Object, fieldNameType);

            //assert
            Assert.AreEqual(exists, result);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldNameType)).Returns(true);

            //act
            _sut.FieldExists(_dataRecordMock.Object, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetNumberOfCases(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetNumberOfRecords(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int numberOfCases = 5;

            _dataRecordServiceMock.Setup(dr => dr.GetNumberOfRecords(
                _connectionModel, _questionnaireName, _serverParkName)).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_A_File_I_Call_GetNumberOfCases_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetNumberOfCases(_connectionModel, _databaseFile);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetNumberOfRecords(_connectionModel, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_For_Local_Connection_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int numberOfCases = 5;

            _dataRecordServiceMock.Setup(dr => dr.GetNumberOfRecords(_connectionModel,
                _databaseFile)).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_connectionModel, _databaseFile);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_I_Call_MapFieldDictionaryFromRecord_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var fieldDictionary = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapFieldDictionaryFromRecord(_dataRecordMock.Object))
                .Returns(fieldDictionary);

            //act
            var result = _sut.GetFieldDataFromRecord(_dataRecordMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Dictionary<string, string>>(result);
            Assert.AreSame(fieldDictionary, result);
        }


        [TestCase("02-12-2021", "09:23:59", 12)]
        [TestCase("12-01-2021", "23:45:59", 01)]
        public void Given_The_Date_And_Time_Fields_Are_Set_When_I_Call_GetLastUpdated_Then_The_Correct_Value_Is_Returned(
            string dateField, string timeField, int month)
        {
            //arrange
            var expectedDateTime = DateTime.ParseExact($"{dateField} {timeField}", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns(dateField);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(dateFieldMock.Object);

            //setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns(timeField);
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(timeFieldMock.Object);


            //act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(expectedDateTime, result);
            Assert.AreEqual(month, result.Value.Month);
        }

        [TestCase("", "09:23:59")]
        [TestCase(null, "09:23:59")]
        [TestCase("01-01-2021", "")]
        [TestCase("01-01-2021", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void Given_The_Date_Or_Time_Field_Is_Not_Set_Are_Set_When_I_Call_GetLastUpdated_Then_Null_Is_Returned(
            string dateFieldValue, string timeFieldValue)
        {
            // Arrange
            SetupField(FieldNameType.LastUpdatedDate, dateFieldValue);
            SetupField(FieldNameType.LastUpdatedTime, timeFieldValue);

            // Act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            // Assert
            Assert.IsNull(result);
        }

        private void SetupField(FieldNameType fieldType, string fieldValue)
        {
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            dataValueMock.Setup(d => d.ValueAsText).Returns(fieldValue);
            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);

            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldType))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldType))
                .Returns(fieldMock.Object);
        }

        [TestCase("2017", "09:23:59")]
        [TestCase("02-2017", "09:23:59")]
        [TestCase("01-01-2021", "09")]
        public void Given_Invalid_Date_Or_Time_Format_When_GetLastUpdatedIsCalled_NullIsReturned(
            string dateField, string timeField)
        {
            // Arrange
            MockFieldWithMockDataValue(FieldNameType.LastUpdatedDate, dateField);
            MockFieldWithMockDataValue(FieldNameType.LastUpdatedTime, timeField);

            // Act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            // Assert
            Assert.IsNull(result);
        }

        private void MockFieldWithMockDataValue(FieldNameType fieldName, string fieldValue)
        {
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            dataValueMock.Setup(d => d.ValueAsText).Returns(fieldValue);
            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);

            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldName)).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldName)).Returns(fieldMock.Object);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Correct_Service_Method_Is_Called()
        {
            // Arrange
            const int outcomeCode = 110;
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldValueMock = new Mock<IDataValue>();
            fieldValueMock.Setup(f => f.IntegerValue).Returns(outcomeCode);

            _fieldServiceMock
                .Setup(f => f.GetField(dataRecordMock.Object, FieldNameType.HOut).DataValue)
                .Returns(fieldValueMock.Object);

            // Act
            _sut.GetOutcomeCode(dataRecordMock.Object);

            // Assert
            _fieldServiceMock.Verify(f => f.GetField(dataRecordMock.Object, FieldNameType.HOut), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int outcomeCode = 110;
            var dataRecord = new Mock<IDataRecord>();
            var fieldValue = new Mock<IDataValue>();
            fieldValue.Setup(f => f.IntegerValue).Returns(outcomeCode);

            _fieldServiceMock.Setup(f => f.GetField(It.IsAny<IDataRecord>(), FieldNameType.HOut).DataValue).Returns(fieldValue.Object);

            //act
            var result = _sut.GetOutcomeCode(dataRecord.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(outcomeCode, result);
        }

        [Test]
        public void Given_The_Date_Field_Does_Not_Exist_When_I_Call_GetLastUpdated_Then_Null_Is_Returned()
        {
            //arrange

            //setup date
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(false);

            //setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns("09:23:59");
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(timeFieldMock.Object);

            //act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Given_The_Time_Field_Does_Not_Exist_When_I_Call_GetLastUpdated_Then_Null_Is_Returned()
        {
            //arrange

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns("02-12-2021");
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(dateFieldMock.Object);

            //setup time
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(false);

            //act
            var result = _sut.GetLastUpdated(_dataRecordMock.Object);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Given_The_Field_Is_Set_When_I_Call_GetLastUpdatedAsString_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            var expectedDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns(expectedDateTime);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(dateFieldMock.Object);


            //act
            var result = _sut.GetLastUpdatedAsString(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual(expectedDateTime, result);
        }

        [Test]
        public void Given_The_LastUpdated_Field_Does_Not_Exist_When_I_Call_GetLastUpdatedAsString_Then_Null_Is_Returned()
        {
            //arrange

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns("02-12-2021");
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(true);

            //setup time
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(false);

            //act
            var result = _sut.GetLastUpdatedAsString(_dataRecordMock.Object);

            //assert
            Assert.IsNull(result);
        }

        [TestCase(-30)]
        [TestCase(-20)]
        [TestCase(-10)]
        [TestCase(0)]
        public void Given_LastUpdated_Is_Set_To_30_Minutes_Or_Less_When_I_Call_CaseInUseInCati_Then_True_Is_returned(int minutes)
        {
            //arrange 
            var dateTime = DateTime.Now.AddMinutes(minutes);

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns(dateTime.ToString("dd-MM-yyyy"));
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(dateFieldMock.Object);

            //setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns(dateTime.ToString("HH:mm:ss"));
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(timeFieldMock.Object);


            //act
            var result = _sut.CaseInUseInCati(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestCase(-31)]
        [TestCase(-50)]
        [TestCase(-100)]
        public void Given_LastUpdated_Is_Set_To_More_Than_30_minutes_When_I_Call_CaseInUseInCati_Then_False_Is_returned(int minutes)
        {
            //arrange
            var dateTime = DateTime.Now.AddMinutes(minutes);

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns(dateTime.ToString("dd-MM-yyyy"));
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(dateFieldMock.Object);

            //setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns(dateTime.ToString("HH:mm:ss"));
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(timeFieldMock.Object);


            //act
            var result = _sut.CaseInUseInCati(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_LastUpdated_Is_Not_set_When_I_Call_CaseInUseInCati_Then_False_Is_returned()
        {
            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.ValueAsText).Returns("");
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedDate))
                .Returns(dateFieldMock.Object);

            //setup time
            var timeDataValueMock = new Mock<IDataValue>();
            var timeFieldMock = new Mock<IField>();
            timeDataValueMock.Setup(d => d.ValueAsText).Returns("");
            timeFieldMock.Setup(f => f.DataValue).Returns(timeDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdatedTime))
                .Returns(timeFieldMock.Object);


            //act
            var result = _sut.CaseInUseInCati(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_GetCaseStatus_Then_An_Expected_CaseStatusModel_Is_Returned()
        {
            //arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(It.IsAny<IDataRecord>())).Returns(primaryKeyValues);

            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock.Setup(f => f.GetField(It.IsAny<IDataRecord>(), FieldNameType.HOut).DataValue).Returns(outcomeFieldValue.Object);

            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(dateFieldMock.Object);

            //act
            var result = _sut.GetCaseStatus(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CaseStatusModel>(result);
            Assert.AreEqual(primaryKeyValues, result.PrimaryKeyValues);
            Assert.AreEqual(outCome, result.Outcome);
            Assert.AreEqual(lastUpdated, result.LastUpdated);
        }

        [Test]
        public void Given_A_Valid_DataSet_When_I_Call_GetCaseStatusModelList_Then_An_Expected_List_Of_CaseStatusModel_Is_Returned()
        {
            //Arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            // Setup mocks for KeyService and FieldService
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValues(_dataRecordMock.Object)).Returns(primaryKeyValues);

            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.HOut).DataValue).Returns(outcomeFieldValue.Object);

            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);

            // Setup FieldService mocks for LastUpdated field
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(dateFieldMock.Object);

            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                    .Returns(false)
                    .Returns(false)
                    .Returns(true);

            // Setup DataRecordService mock
            _dataRecordServiceMock.Setup(d => d.GetDataSet(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(dataSetMock.Object);

            // Act
            var result = _sut.GetCaseStatusModelList(_connectionModel, _questionnaireName, _serverParkName).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CaseStatusModel>>(result);
            Assert.AreEqual(2, result.Count);

            foreach (var caseStatusModel in result)
            {
                // Verify the properties of the CaseStatusModel
                Assert.AreEqual(primaryKeyValues, caseStatusModel.PrimaryKeyValues);
                Assert.AreEqual(outCome, caseStatusModel.Outcome);
                Assert.AreEqual(lastUpdated, caseStatusModel.LastUpdated);
            }
        }

        [Test]
        public void Given_A_Valid_DataSet_When_I_Call_GetCaseStatusModelList_For_A_File_Then_An_Expected_List_Of_CaseStatusModel_Is_Returned()
        {
            // Arrange
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
                .Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.HOut).DataValue)
                .Returns(outcomeFieldValue.Object);

            // Mock Date Field
            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);
            _fieldServiceMock
                .Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(true);
            _fieldServiceMock
                .Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated))
                .Returns(dateFieldMock.Object);

            // Mock DataSet
            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(false)
                .Returns(true);
            _dataRecordServiceMock
                .Setup(d => d.GetDataSet(_connectionModel, _databaseFile))
                .Returns(dataSetMock.Object);

            // Act
            var result = _sut.GetCaseStatusModelList(_connectionModel, _databaseFile).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CaseStatusModel>>(result);
            Assert.AreEqual(2, result.Count);

            foreach (var caseStatusModel in result)
            {
                Assert.AreEqual(primaryKeyValues, caseStatusModel.PrimaryKeyValues);
                Assert.AreEqual(outCome, caseStatusModel.Outcome);
                Assert.AreEqual(lastUpdated, caseStatusModel.LastUpdated);
            }
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_GetCaseModel_Then_An_Expected_CaseModel_Is_Returned()
        {
            // Arrange
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "900000" } };
            var fieldDictionary = new Dictionary<string, string>();

            _mapperServiceMock
                .Setup(m => m.MapFieldDictionaryFromRecord(_dataRecordMock.Object))
                .Returns(fieldDictionary);

            // Act
            var result = _sut.GetCaseModel(_connectionModel, primaryKeyValues, _questionnaireName, _serverParkName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CaseModel>(result);
            Assert.AreEqual(primaryKeyValues, result.PrimaryKeyValues);
            Assert.AreSame(fieldDictionary, result.FieldData);
        }
    }
}
