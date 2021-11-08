using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Mappers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Mappers
{
    public class DataMapperServiceTests
    {
        private DataMapperService _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new DataMapperService();

        }

        [Test]
        public void Given_A_DataRecord_With_A_List_Of_Fields_When_I_Call_MapFieldDictionaryFromRecord_I_Get_A_Dictionary_Of_Fields_Returned()
        {
            var dataRecordMock = new Mock<IDataRecord>();

            var field1Mock = new Mock<IField>();
            field1Mock.Setup(f => f.FullName).Returns("Field1Name");
            field1Mock.Setup(f => f.DataValue.ValueAsText).Returns("Field1Value");

            var field2Mock = new Mock<IField>();
            field2Mock.Setup(f => f.FullName).Returns("Field2Name");
            field2Mock.Setup(f => f.DataValue.ValueAsText).Returns("Field2Value");


            dataRecordMock.As<IDataRecord2>().Setup(dr => dr.GetDataFields())
                .Returns(new List<IField> { field1Mock.Object, field2Mock.Object });

            //act
            var result = _sut.MapFieldDictionaryFromRecord(dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Dictionary<string, string>>(result);
            Assert.AreEqual(2, result.Count);

            Assert.True(result.ContainsKey("Field1Name"));
            Assert.AreEqual(result["Field1Name"], "Field1Value");

            Assert.True(result.ContainsKey("Field2Name"));
            Assert.AreEqual(result["Field2Name"], "Field2Value");
        }
    }
}