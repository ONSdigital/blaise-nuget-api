﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class CaseServiceTests
    {
        private Mock<IDataModelService> _dataModelServiceMock;
        private Mock<IKeyService> _keyServiceMock;
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IFieldService> _fieldServiceMock;
        private Mock<IDataMapperService> _mapperServiceMock;

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _databaseFile;
        private readonly string _keyName;


        private CaseService _sut;

        public CaseServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _databaseFile = "c:\\filePath\\opn2010.bdbx";
            _keyName = "TestKeyName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _dataModelServiceMock = new Mock<IDataModelService>();
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, _instrumentName, _serverParkName)).Returns(_dataModelMock.Object);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, _databaseFile)).Returns(_dataModelMock.Object);

            _keyServiceMock = new Mock<IKeyService>();
            _keyServiceMock.Setup(d => d.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName)).Returns(true);
            _keyServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);
            _keyServiceMock.Setup(d => d.GetPrimaryKey(_dataModelMock.Object)).Returns(_keyMock.Object);

            _dataRecordServiceMock = new Mock<IDataRecordService>();
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName))
                .Returns(_dataRecordMock.Object);

            _fieldServiceMock = new Mock<IFieldService>();

            _mapperServiceMock = new Mock<IDataMapperService>();

            _sut = new CaseService(
                _dataModelServiceMock.Object,
                _dataRecordServiceMock.Object,
                _keyServiceMock.Object,
                _fieldServiceMock.Object,
                _mapperServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Key_Is_Returned()
        {
            //act
            const string primaryKey = "Key1";
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(primaryKey);

            var result = _sut.GetPrimaryKeyValue(_dataRecordMock.Object);

            //assert
            Assert.AreSame(primaryKey, result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            //act
            _sut.GetPrimaryKeyValue(_dataRecordMock.Object);

            //assert
            _keyServiceMock.Verify(v => v.GetPrimaryKeyValue(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataSet(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _instrumentName, _serverParkName), Times.Once);
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
        public void Given_A_KeyValue_And_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_connectionModel, _keyName, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
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
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //act
            _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<FieldNameType>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, FieldNameType.HOut);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string primaryKeyValue = "Key1";

            //act
            _sut.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue), Times.Once);
            _keyServiceMock.Verify(v => v.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Value_Is_Returned(bool caseExists)
        {
            //arrange
            const string primaryKeyValue = "Key1";

            _keyServiceMock.Setup(k => k.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName))
                .Returns(caseExists);

            //act
            var result = _sut.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string primaryKeyValue = "Key1";
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_connectionModel, primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValue, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CreateNewDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange

            //act
            _sut.CreateNewDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName),
                Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string primaryKeyValue = "Key1";
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_connectionModel, _databaseFile, primaryKeyValue, fieldData);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _databaseFile), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _keyMock.Object, primaryKeyValue, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string primaryKeyValue = "Key1";
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object,
                _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(),
                    It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, _dataRecordMock.Object, fieldData, _instrumentName, _serverParkName);

            //assert
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_KeyValue_And_An_InstrumentName_And_ServerParkName_When_I_Call_RemoveDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveDataRecord(_connectionModel, _keyName, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.DeleteDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_RemoveDataRecords_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveDataRecords(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.DeleteDataRecords(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_I_Call_GetFieldValue_Then_The_Correct_DataModel_Is_Returned(FieldNameType fieldNameType)
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
        public void Given_I_Call_GetFieldValue_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
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
            _sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetNumberOfRecords(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int numberOfCases = 5;

            _dataRecordServiceMock.Setup(dr => dr.GetNumberOfRecords(
                _connectionModel, _instrumentName, _serverParkName)).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

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
            string dateField, string timeField)
        {
            //arrange
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
            Assert.IsNull(result);
        }

        [TestCase("2017", "09:23:59")]
        [TestCase("02-2017", "09:23:59")]
        [TestCase("01-01-2021", "09")]
        public void Given_The_Date_Or_Time_Field_Is_In_Incorrect_Format_When_I_Call_GetLastUpdated_Then_Null_Is_Returned(
            string dateField, string timeField)
        {
            //arrange

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
            Assert.IsNull(result);
        }

        [Test]
        public void Given_The_LiveDate_Is_Set_When_I_Call_GetLiveDate_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            var expectedDateTime = DateTime.Now.Date;

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.DateValue).Returns(expectedDateTime);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(dateFieldMock.Object);

            //act
            var result = _sut.GetLiveDate(_dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(expectedDateTime, result);
        }

        [Test]
        public void Given_The_LiveDate_Is_Not_Set_When_I_Call_GetLiveDate_Then_Null_Is_Returned()
        {
            //arrange

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.DateValue).Returns((DateTime?)null);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(dateFieldMock.Object);

            //act
            var result = _sut.GetLiveDate(_dataRecordMock.Object);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Given_The_LiveDate_Field_Does_Not_Exist_When_I_Call_GetLiveDate_Then_Null_Is_Returned()
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LiveDate))
                       .Returns(false);

            //act
            var result = _sut.GetLiveDate(_dataRecordMock.Object);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var outcomeCode = 110;
            var dataRecord = new Mock<IDataRecord>();
            var fieldValue = new Mock<IDataValue>();
            fieldValue.Setup(f => f.IntegerValue).Returns(outcomeCode);

            _fieldServiceMock.Setup(f => f.GetField(It.IsAny<IDataRecord>(), FieldNameType.HOut).DataValue).Returns(fieldValue.Object);


            //act
            _sut.GetOutcomeCode(dataRecord.Object);

            //assert
            _fieldServiceMock.Verify(v => v.GetField(dataRecord.Object, FieldNameType.HOut),
                Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var outcomeCode = 110;
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
            //arrange

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
        public void Given_A_Valid_DataRecord_When_I_Call_MapCaseStatusModel_Then_An_Expected_CaseStatusModel_Is_Returned()
        {
            //arrange
            const string primaryKeyValue = "900000";
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(primaryKeyValue);

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
            Assert.AreEqual(primaryKeyValue, result.PrimaryKey);
            Assert.AreEqual(outCome, result.Outcome);
            Assert.AreEqual(lastUpdated, result.LastUpdated);
        }

        [Test]
        public void Given_A_Valid_DataSet_When_I_Call_GetCaseStatusList_Then_An_Expected_List_Of_CaseStatusModel_Is_Returned()
        {
            //arrange
            const string primaryKeyValue = "900000";
            const int outCome = 110;
            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(_dataRecordMock.Object)).Returns(primaryKeyValue);

            var outcomeFieldValue = new Mock<IDataValue>();
            outcomeFieldValue.Setup(f => f.IntegerValue).Returns(outCome);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.HOut).DataValue).Returns(outcomeFieldValue.Object);

            var dateFieldValue = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateFieldValue.Setup(d => d.ValueAsText).Returns(lastUpdated);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateFieldValue.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LastUpdated)).Returns(dateFieldMock.Object);

            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                    .Returns(false)
                    .Returns(false)
                    .Returns(true);

            _dataRecordServiceMock.Setup(d => d.GetDataSet(_connectionModel, _instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            var result = _sut.GetCaseStatusList(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CaseStatusModel>>(result);
            Assert.AreEqual(2, result.Count);

            foreach (var caseStatusModel in result)
            {
                Assert.AreEqual(primaryKeyValue, caseStatusModel.PrimaryKey);
                Assert.AreEqual(outCome, caseStatusModel.Outcome);
                Assert.AreEqual(lastUpdated, caseStatusModel.LastUpdated);
            }
        }

        [Test]
        public void Given_No_dataRecord_The_LiveDate_Is_Set_When_I_Call_GetLiveDate_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            var expectedDateTime = DateTime.Now.Date;

            //setup date
            var dateDataValueMock = new Mock<IDataValue>();
            var dateFieldMock = new Mock<IField>();
            dateDataValueMock.Setup(d => d.DateValue).Returns(expectedDateTime);
            dateFieldMock.Setup(f => f.DataValue).Returns(dateDataValueMock.Object);
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(true);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, FieldNameType.LiveDate))
                .Returns(dateFieldMock.Object);

            //setup records
            var dataSetMock = new Mock<IDataSet>();
            dataSetMock.Setup(d => d.ActiveRecord).Returns(_dataRecordMock.Object);

            _dataRecordServiceMock.Setup(d => d.GetDataSet(_connectionModel, _instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            var result = _sut.GetLiveDate(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DateTime>(result);
            Assert.AreEqual(expectedDateTime, result);
        }
    }
}
