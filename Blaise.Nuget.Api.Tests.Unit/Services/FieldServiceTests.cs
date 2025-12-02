namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class FieldServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private Mock<IDataModelService> _dataModelServiceMock;
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
        public void Given_A_Field_Name_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            const string FieldName = "QHAdmin.HOut";
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(FieldName)).Returns(It.IsAny<bool>());

            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            // act
            _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, FieldName);

            // assert
            _dataModelServiceMock.Verify(d => d.GetDataModel(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_Field_Name_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            // arrange
            const string FieldName = "QHAdmin.HOut";
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(FieldName)).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            // act
            var result = _sut.FieldExists(_connectionModel, _questionnaireName, _serverParkName, FieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_Data_Record_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            // arrange
            const string FieldName = "QHAdmin.HOut";
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(FieldName)).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(dr => dr.Datamodel).Returns(dataModelMock.Object);

            // act
            var result = _sut.FieldExists(dataRecordMock.Object, FieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldExists));
        }

        [Test]
        public void Given_A_Data_Record_When_I_Call_GetField_Then_The_Correct_Field_Is_Returned()
        {
            // arrange
            const string FieldName = "QHAdmin.HOut";
            var fieldMock = new Mock<IField>();

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(FieldName)).Returns(fieldMock.Object);

            // act
            var result = _sut.GetField(dataRecordMock.Object, FieldName);

            // assert
            Assert.That(result, Is.EqualTo(fieldMock.Object));
        }
    }
}
