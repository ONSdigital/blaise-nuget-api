﻿
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class FieldServiceTests
    {
        private Mock<IDataModelService> _dataModelServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;

        private IFieldService _sut;

        public FieldServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelServiceMock = new Mock<IDataModelService>();

            _sut = new FieldService(_dataModelServiceMock.Object);
        }
        
        [Test]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(fieldName)).Returns(It.IsAny<bool>());

            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            //assert
            _dataModelServiceMock.Verify(d => d.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_FieldName_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(fieldName)).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            var result = _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, fieldName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var dataRecord2Mock = new Mock<IDataRecord2>();
            var fieldCollectionMock = new Mock<IFieldCollection>();

            var iFieldMock = new Mock<IField>();

            if (fieldExists)
            {
                iFieldMock.Setup(f => f.FullName).Returns(fieldName);
            }
            else
            {
                iFieldMock.Setup(f => f.FullName).Returns("Does Not Exist");
            }
            var fields = new List<IField> {iFieldMock.Object};

            fieldCollectionMock.Setup(f => f.GetEnumerator())
                .Returns(fields.GetEnumerator());

            dataRecord2Mock.Setup(d => d.Fields).Returns(fieldCollectionMock.Object);

            //act
            var result = _sut.FieldExists(dataRecord2Mock.Object, fieldName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_FieldName_When_I_Call_GetField_Then_The_Correct_Field_Is_Returned()
        {
            //arrange
            const string fieldName = "QHAdmin.HOut";
            var fieldMock = new Mock<IField>();

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(fieldName)).Returns(fieldMock.Object);

            //act
            var result = _sut.GetField(dataRecordMock.Object, fieldName);

            //assert
            Assert.AreEqual(fieldMock.Object, result);
        }
    }
}
