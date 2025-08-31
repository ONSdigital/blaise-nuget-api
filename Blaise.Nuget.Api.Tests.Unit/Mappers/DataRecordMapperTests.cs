namespace Blaise.Nuget.Api.Tests.Unit.Mappers
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Core.Mappers;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataRecord;

    public class DataRecordMapperTests
    {
        private DataRecordMapper _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new DataRecordMapper();
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

            // act
            var result = _sut.MapFieldDictionaryFromRecord(dataRecordMock.Object);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<Dictionary<string, string>>());
            Assert.That(result.Count, Is.EqualTo(2));

            Assert.That(result.ContainsKey("Field1Name"), Is.True);
            Assert.That(result["Field1Name"], Is.EqualTo("Field1Value"));

            Assert.That(result.ContainsKey("Field2Name"), Is.True);
            Assert.That(result["Field2Name"], Is.EqualTo("Field2Value"));
        }
    }
}
