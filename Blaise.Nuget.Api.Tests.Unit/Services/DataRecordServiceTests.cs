using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataRecordServiceTests
    {
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;
        private Mock<ILocalDataLinkProvider> _localDataLinkProviderMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink6> _remoteDataLinkMock;
        private Mock<IDataLink6> _localDataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataSet> _dataSetMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;
        private readonly string _databaseFile;
        private readonly Guid _questionnaireId;

        private DataRecordService _sut;

        public DataRecordServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _questionnaireName = "TestQuestionnaireName";
            _serverParkName = "TestServerParkName";
            _databaseFile = "c:\\filePath\\opn2010.dbdx";
            _questionnaireId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataSetMock = new Mock<IDataSet>();
            _dataRecordMock = new Mock<IDataRecord>();

            _remoteDataLinkMock = new Mock<IDataLink6>();
            _remoteDataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _localDataLinkMock = new Mock<IDataLink6>();

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_questionnaireId, _serverParkName)).Returns(_remoteDataLinkMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _questionnaireName, _serverParkName)).Returns(_remoteDataLinkMock.Object);

            _localDataLinkProviderMock = new Mock<ILocalDataLinkProvider>();
            _localDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _databaseFile)).Returns(_localDataLinkMock.Object);

            _sut = new DataRecordService(
                _remoteDataLinkProviderMock.Object,
                _localDataLinkProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDataSet_I_Get_A_DataSet_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IDataSet>());
        }

        [Test]
        public void Given_I_Call_GetDataSet_I_Get_The_Correct_DataSet_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(_dataSetMock.Object));
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDataSet>());

            //act
            _sut.GetDataSet(_connectionModel, _questionnaireName, _serverParkName, null);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Read(null, null), Times.Once);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataSet_I_Get_A_DataSet_Back()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_connectionModel, _databaseFile, null);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IDataSet>());
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataSet_I_Get_The_Correct_DataSet_Back()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_connectionModel, _databaseFile, null);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(_dataSetMock.Object));
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            _sut.GetDataSet(_connectionModel, _databaseFile, null);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _databaseFile), Times.Once);
            _localDataLinkMock.Verify(v => v.Read(null, null), Times.Once);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataRecord_I_Get_The_Correct_DataRecord_Back()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.ReadRecord(_keyMock.Object)).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_connectionModel, _databaseFile, _keyMock.Object);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(_dataRecordMock.Object));
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.ReadRecord(_keyMock.Object)).Returns(_dataRecordMock.Object);

            //act
            _sut.GetDataRecord(_connectionModel, _databaseFile, _keyMock.Object);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _databaseFile), Times.Once);
            _localDataLinkMock.Verify(v => v.ReadRecord(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_I_Get_A_DataRecord_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IDataRecord>());
        }

        [Test]
        public void Given_A_QuestionnaireName_And_ServerParkName_When_I_Call_GetDataRecord_I_Get_The_Correct_DataRecord_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(_dataRecordMock.Object));
        }

        [Test]
        public void Given_A_QuestionnaireName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(It.IsAny<IDataRecord>());

            //act
            _sut.GetDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.ReadRecord(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_QuestionnaireName_And_ServerParkName_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Write(It.IsAny<IDataRecord>()));

            //act
            _sut.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _questionnaireName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Write(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_DatabaseFile_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.Write(It.IsAny<IDataRecord>()));

            //act
            _sut.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _databaseFile);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _databaseFile), Times.Once);
            _localDataLinkMock.Verify(v => v.Write(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_When_I_Call_RemoveDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Delete(It.IsAny<IKey>()));

            //act
            _sut.DeleteDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Delete(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveDataRecords_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.DeleteDataRecords(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.DeleteAll(), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfRecords_I_Get_The_Correct_Number_Back()
        {
            //arrange
            _dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetNumberOfRecords(_connectionModel, _questionnaireName, _serverParkName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Given_I_Call_GetNumberOfRecords_For_Local_Connection_I_Get_The_Correct_Number_Back()
        {
            //arrange
            _dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            _localDataLinkMock.Setup(d => d.Read(It.IsAny<string>(), It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetNumberOfRecords(_connectionModel, _databaseFile);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_LockDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string lockId = "Lock123";
            _remoteDataLinkMock.Setup(d => d.Lock(It.IsAny<IKey>(), lockId));

            //act
            _sut.LockDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName, lockId);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Lock(_keyMock.Object, lockId), Times.Once);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_UnLockDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string lockId = "Lock123";
            _remoteDataLinkMock.Setup(d => d.Unlock(It.IsAny<IKey>(), lockId));

            //act
            _sut.UnLockDataRecord(_connectionModel, _keyMock.Object, _questionnaireName, _serverParkName, lockId);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _questionnaireName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Unlock(_keyMock.Object, lockId), Times.Once);
        }
    }
}
