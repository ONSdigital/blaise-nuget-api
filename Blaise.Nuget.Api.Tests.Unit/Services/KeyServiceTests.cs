namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Services;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class KeyServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly Guid _questionnaireId;
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;
        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink6> _remoteDataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;
        private KeyService _sut;

        public KeyServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _questionnaireId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _remoteDataLinkMock = new Mock<IDataLink6>();
            _remoteDataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_questionnaireId, _serverParkName))
                .Returns(_remoteDataLinkMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _questionnaireName, _serverParkName))
                .Returns(_remoteDataLinkMock.Object);

            _sut = new KeyService(_remoteDataLinkProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_KeyExists_I_Get_A_Boolean_Back()
        {
            // arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(It.IsAny<bool>());

            // act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.InstanceOf<bool>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_KeyExists_I_Get_The_Correct_Boolean_Back(bool keyExists)
        {
            // arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(keyExists);

            // act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            // assert
            Assert.That(result, Is.EqualTo(keyExists));
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_The_Correct_Services_Are_Called()
        {
            // arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(It.IsAny<bool>());

            // act
            _sut.KeyExists(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            // assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.KeyExists(_keyMock.Object), Times.Once);
        }

        [TestCase("QID.Serial_Number", "900001", "900001")]
        [TestCase("QID.Serial_Number", " 900001 ", "900001")]
        [TestCase("QID.Serial_Number", "900001 ", "900001")]
        [TestCase("QID.Serial_Number", " 900001", "900001")]
        public void Given_I_Call_GetPrimaryKeyValues_I_Get_The_Correct_Value_Back(string primaryKeyName, string primaryKeyValue, string expectedValue)
        {
            // arrange
            var primaryKeyFieldMock = new Mock<IField>();
            primaryKeyFieldMock.Setup(f => f.FullName).Returns(primaryKeyName);
            primaryKeyFieldMock.Setup(f => f.DataValue.ValueAsText).Returns(primaryKeyValue);

            var fieldList = new List<IField> { primaryKeyFieldMock.Object };
            var fieldCollectionMock = new Mock<IFieldCollection>();
            fieldCollectionMock.Setup(fc => fc.GetEnumerator()).Returns(fieldList.GetEnumerator());

            _keyMock.Setup(k => k.Name).Returns("PRIMARY");
            _keyMock.Setup(k => k.Fields).Returns(fieldCollectionMock.Object);
            var keyCollection = new List<IKey> { _keyMock.Object };
            var mockKeyCollection = new Mock<IKeyCollection>();
            mockKeyCollection.Setup(col => col.GetEnumerator()).Returns(keyCollection.GetEnumerator());

            _dataRecordMock.Setup(d => d.Keys).Returns(mockKeyCollection.Object);

            // act
            var result = _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            // assert
            Assert.That(result[primaryKeyName], Is.EqualTo(expectedValue));
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValues_For_A_MultiKey_Questionnaire_I_Get_The_Correct_Value_Back()
        {
            // arrange
            var primaryKeyFieldMock1 = new Mock<IField>();
            primaryKeyFieldMock1.Setup(f => f.FullName).Returns("QID.Serial_Number");
            primaryKeyFieldMock1.Setup(f => f.DataValue.ValueAsText).Returns("900001");

            var primaryKeyFieldMock2 = new Mock<IField>();
            primaryKeyFieldMock2.Setup(f => f.FullName).Returns("MainSurveyID");
            primaryKeyFieldMock2.Setup(f => f.DataValue.ValueAsText).Returns("6B29FC40-CA47-1067-B31D");

            var fieldList = new List<IField> { primaryKeyFieldMock1.Object, primaryKeyFieldMock2.Object };
            var fieldCollectionMock = new Mock<IFieldCollection>();
            fieldCollectionMock.Setup(fc => fc.GetEnumerator()).Returns(fieldList.GetEnumerator());

            _keyMock.Setup(k => k.Name).Returns("PRIMARY");
            _keyMock.Setup(k => k.Fields).Returns(fieldCollectionMock.Object);
            var keyCollection = new List<IKey> { _keyMock.Object };
            var mockKeyCollection = new Mock<IKeyCollection>();
            mockKeyCollection.Setup(col => col.GetEnumerator()).Returns(keyCollection.GetEnumerator());

            _dataRecordMock.Setup(d => d.Keys).Returns(mockKeyCollection.Object);

            // act
            var result = _sut.GetPrimaryKeyValues(_dataRecordMock.Object);

            // assert
            Assert.That(result["QID.Serial_Number"], Is.EqualTo("900001"));
            Assert.That(result["MainSurveyID"], Is.EqualTo("6B29FC40-CA47-1067-B31D"));
        }
    }
}
