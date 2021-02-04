﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Case
{
    public class BlaiseCaseApiTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private readonly ConnectionModel _connectionModel;

        private readonly string _primaryKeyValue;
        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly string _databaseFile;

        private IBlaiseCaseApi _sut;

        public BlaiseCaseApiTests()
        {
            _connectionModel = new ConnectionModel();
            _primaryKeyValue = "Key1";
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
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
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCaseApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCaseApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _caseServiceMock.Setup(d => d.CaseExists(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.CaseExists(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange
            const string primaryKeyValue = "Key1";

            _caseServiceMock.Setup(d => d.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName)).Returns(caseExists);

            //act
            var result = _sut.CaseExists(primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(string.Empty, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(null, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_primaryKeyValue, string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_primaryKeyValue, null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_primaryKeyValue,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_primaryKeyValue,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            //act
            _sut.GetPrimaryKeyValue(dataRecordMock.Object);

            //assert
            _caseServiceMock.Verify(v => v.GetPrimaryKeyValue(dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(_primaryKeyValue);

            //act
            var result = _sut.GetPrimaryKeyValue(dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_primaryKeyValue, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCases(_instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.GetDataSet(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            const string databaseFile = "File1.bdix";

            _caseServiceMock.Setup(d => d.GetDataSet(It.IsAny<string>()));

            //act
            _sut.GetCases(databaseFile);

            //assert
            _caseServiceMock.Verify(v => v.GetDataSet(databaseFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_GetCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCases(string.Empty));
            Assert.AreEqual("A value for the argument 'databaseFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_GetCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCases(null));
            Assert.AreEqual("databaseFile", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCase_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _caseServiceMock.Setup(d => d.GetDataRecord(_connectionModel, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCase(_primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(string.Empty, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(null, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(_primaryKeyValue,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string primaryKeyValue = "Key1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(primaryKeyValue, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCase(_primaryKeyValue,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCase(_primaryKeyValue,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.CreateNewDataRecord(_connectionModel, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.CreateCase(_primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CreateCase_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(string.Empty, fieldData, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(null, fieldData, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_primaryKeyValue, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateCase_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(_primaryKeyValue, fieldData,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_CallCreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_primaryKeyValue, fieldData,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateCase_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(_primaryKeyValue, fieldData,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_primaryKeyValue, fieldData,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CreateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.CreateNewDataRecord(_connectionModel, It.IsAny<IDataRecord>(),
                 It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.CreateCase(dataRecord.Object, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecord(_connectionModel, dataRecord.Object, _instrumentName, _serverParkName), 
                Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_CreateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(dataRecord.Object,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_CallCreateCase_With_A_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(dataRecord.Object,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(dataRecord.Object,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateCase_With_A_DataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecord = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(dataRecord.Object,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_For_Local_Connection_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.CreateNewDataRecord(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //act
            _sut.CreateCase(_databaseFile, _primaryKeyValue, fieldData);

            //assert
            _caseServiceMock.Verify(v => v.CreateNewDataRecord(_databaseFile, _primaryKeyValue, fieldData), Times.Once);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(string.Empty, _primaryKeyValue, fieldData));
            Assert.AreEqual("A value for the argument 'databaseFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(null, _primaryKeyValue, fieldData));
            Assert.AreEqual("databaseFile", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateCase(_databaseFile, string.Empty, fieldData));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_databaseFile, null, fieldData));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateCase_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateCase(_databaseFile, _primaryKeyValue, null));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateCase_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.UpdateDataRecord(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateCase(dataRecordMock.Object, fieldData, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData,
                _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(null, fieldData,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(dataRecordMock.Object, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateCase_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(dataRecordMock.Object, fieldData, string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(dataRecordMock.Object, fieldData,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateCase_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateCase(dataRecordMock.Object, fieldData,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateCase(dataRecordMock.Object,
                fieldData, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            _caseServiceMock.Setup(d => d.FieldExists(_connectionModel, It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<FieldNameType>())).Returns(It.IsAny<bool>());

            //act
            _sut.FieldExists(_instrumentName, _serverParkName, fieldNameType);

            //CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            //arrange
            _caseServiceMock.Setup(d => d.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<FieldNameType>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_instrumentName, _serverParkName, FieldNameType.HOut);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(string.Empty,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(_instrumentName,
                string.Empty, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(_instrumentName,
                null, FieldNameType.HOut));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>())).Returns(It.IsAny<bool>());

            //act
            _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            //CompletedFieldExists
            _caseServiceMock.Verify(v => v.FieldExists(dataRecordMock.Object, fieldNameType), Times.Once);
        }


        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.HOut, false)]
        [TestCase(FieldNameType.Mode, true)]
        [TestCase(FieldNameType.Mode, false)]
        [TestCase(FieldNameType.TelNo, true)]
        [TestCase(FieldNameType.TelNo, false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.FieldExists(dataRecordMock.Object, fieldNameType)).Returns(exists);

            //act
            var result = _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, FieldNameType.HOut));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_DataRecord_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d => d.GetFieldValue(dataRecordMock.Object, fieldNameType))
                .Returns(dataValueMock.Object);

            //act
            var result = _sut.GetFieldValue(dataRecordMock.Object, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const FieldNameType fieldValueType = FieldNameType.HOut;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, fieldValueType));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }
        
        [TestCase(FieldNameType.HOut)]
        [TestCase(FieldNameType.Mode)]
        [TestCase(FieldNameType.TelNo)]
        public void Given_A_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _caseServiceMock.Setup(d =>
                    d.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName))
                .Returns(dataRecordMock.Object);

            _caseServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>()))
                .Returns(dataValueMock.Object);

            //act
            var result = _sut.GetFieldValue(_primaryKeyValue, _instrumentName, _serverParkName, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(string.Empty, _instrumentName,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, _instrumentName,
                _serverParkName, FieldNameType.HOut)); Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_InstrumentName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_primaryKeyValue, string.Empty,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_InstrumentName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_primaryKeyValue, null,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_primaryKeyValue, _instrumentName,
                string.Empty, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_primaryKeyValue, _instrumentName,
                null, FieldNameType.HOut));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetNumberOfCases(_instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.GetNumberOfCases(
                _connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int numberOfCases = 5;
            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            const string databaseFile = "File1.bdix";

            _caseServiceMock.Setup(d => d.GetNumberOfCases(It.IsAny<string>()));

            //act
            _sut.GetNumberOfCases(databaseFile);

            //assert
            _caseServiceMock.Verify(v => v.GetNumberOfCases(databaseFile), Times.Once);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            const int numberOfCases = 5;

            _caseServiceMock.Setup(d => d.GetNumberOfCases(
                It.IsAny<string>())).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_databaseFile);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_An_Empty_DatabaseFile_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(string.Empty));
            Assert.AreEqual("A value for the argument 'databaseFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DatabaseFile_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null));
            Assert.AreEqual("databaseFile", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveCase_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveCase(_primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _caseServiceMock.Verify(v => v.RemoveDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(string.Empty, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(null, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(_primaryKeyValue,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            const string primaryKeyValue = "Key1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(primaryKeyValue, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(_primaryKeyValue,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(_primaryKeyValue,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetRecordDataFields_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();
            _caseServiceMock.Setup(d => d.GetFieldDataFromRecord(It.IsAny<IDataRecord>()));

            //act
            _sut.GetRecordDataFields(dataRecord.Object);

            //assert
            _caseServiceMock.Verify(v => v.GetFieldDataFromRecord(dataRecord.Object), 
                Times.Once);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetRecordDataFields_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();
            var fieldDictionary = new Dictionary<string, string>();

            _caseServiceMock.Setup(d => d.GetFieldDataFromRecord(It.IsAny<IDataRecord>()))
                .Returns(fieldDictionary);

            //act
            var result = _sut.GetRecordDataFields(dataRecord.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Dictionary<string, string>>(result);
            Assert.AreSame(fieldDictionary, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetRecordDataFields_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetRecordDataFields(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_GetOutcomeCode_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var outcomeCode = 110;
            var dataRecord = new Mock<IDataRecord>();
            var fieldValue = new Mock<IDataValue>();
            fieldValue.Setup(f => f.IntegerValue).Returns(outcomeCode);

            _caseServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), FieldNameType.HOut))
                .Returns(fieldValue.Object);

            //act
            _sut.GetOutcomeCode(dataRecord.Object);

            //assert
            _caseServiceMock.Verify(v => v.GetFieldValue(dataRecord.Object, FieldNameType.HOut), 
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

            _caseServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), FieldNameType.HOut))
                .Returns(fieldValue.Object);

            //act
            var result = _sut.GetOutcomeCode(dataRecord.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(outcomeCode, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetOutcomeCode_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetOutcomeCode(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }
    }
}
