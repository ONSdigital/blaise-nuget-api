using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Providers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class DataInterfaceProviderTests
    {
        private Mock<IDataInterfaceFactory> _dataInterfaceFactoryMock;

        private Mock<IDataInterface> _dataInterfaceMock;
        private Mock<IGeneralDataInterface> _generalDataInterfaceMock;

        private readonly string _sourceFile;
        private readonly string _connectionString;

        private DataInterfaceProvider _sut;

        public DataInterfaceProviderTests()
        {
            _sourceFile = "OPN2101A.bdbx";
            _connectionString = "testConnection";
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup surveys
            _dataInterfaceMock = new Mock<IDataInterface>();
            _dataInterfaceMock.Setup(d => d.ConnectionInfo.SetConnectionString(It.IsAny<string>(), It.IsAny<string>()));
            _dataInterfaceMock.Setup(d => d.CreateTableDefinitions());
            _dataInterfaceMock.Setup(d => d.CreateDatabaseObjects(It.IsAny<string>(), It.IsAny<bool>()));
            _dataInterfaceMock.Setup(d => d.SaveToFile(It.IsAny<bool>()));

            _generalDataInterfaceMock = new Mock<IGeneralDataInterface>();
            _generalDataInterfaceMock.Setup(d => d.ConnectionInfo.SetConnectionString(It.IsAny<string>(), It.IsAny<string>()));
            _generalDataInterfaceMock.Setup(d => d.CreateDatabaseObjects(It.IsAny<string>(), It.IsAny<bool>()));
            _generalDataInterfaceMock.Setup(d => d.SaveToFile(It.IsAny<bool>()));

            _dataInterfaceFactoryMock = new Mock<IDataInterfaceFactory>();
            _dataInterfaceFactoryMock.Setup(d => d.GetDataInterfaceForFile(It.IsAny<string>()))
                .Returns(_dataInterfaceMock.Object);
            _dataInterfaceFactoryMock.Setup(d => d.GetDataInterfaceForSql(It.IsAny<string>()))
                .Returns(_dataInterfaceMock.Object);
            _dataInterfaceFactoryMock.Setup(d => d.GetSettingsDataInterfaceForSql(It.IsAny<string>(),
                It.IsAny<ApplicationType>())).Returns(_generalDataInterfaceMock.Object);


            _sut = new DataInterfaceProvider(_dataInterfaceFactoryMock.Object);
        }
        
        [Test]
        public void Given_I_Call_CreateFileDataInterface_Then_The_Correct_DataInterface_Is_Created()
        {
            //arrange
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";

            //act
            _sut.CreateFileDataInterface(_sourceFile, fileName, dataModelFileName);

            //assert
            _dataInterfaceFactoryMock.Verify(v => v.GetDataInterfaceForFile(_sourceFile), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateTableDefinitions(), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateDatabaseObjects(null, true), Times.Once);
            _dataInterfaceFactoryMock.Verify(v => v.UpdateDataFileSource(_dataInterfaceMock.Object, _sourceFile), Times.Once);
            _dataInterfaceMock.Verify(v => v.SaveToFile(true), Times.Once);

        }

        [Test]
        public void Given_I_Call_CreateFileDataInterface_Then_I_Get_A_Correct_DataInterface_Returned()
        {
            //arrange
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";

            //act
            var result = _sut.CreateFileDataInterface(_sourceFile, fileName, dataModelFileName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IDataInterface>(result);
            Assert.AreSame(_dataInterfaceMock.Object, result);
        }

        [Test]
        public void Given_I_Call_CreateSqlDataInterface_Then_The_Correct_DataInterface_Is_Created()
        {
            //arrange
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";
            _dataInterfaceMock.Setup(d => d.ConnectionInfo.GetConnectionString(null)).Returns(_connectionString);

            //act
            _sut.CreateSqlDataInterface(_connectionString, fileName, dataModelFileName);

            //assert
            _dataInterfaceFactoryMock.Verify(v => v.GetDataInterfaceForSql(_connectionString), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateTableDefinitions(), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateDatabaseObjects(_connectionString, true), Times.Once);
            _dataInterfaceMock.Verify(v => v.SaveToFile(true), Times.Once);
        }

        [Test]
        public void Given_I_Call_CreateSqlDataInterface_Then_I_Get_A_Correct_DataInterface_Returned()
        {
            //arrange
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";
            _dataInterfaceMock.Setup(d => d.ConnectionInfo.GetConnectionString(null)).Returns(_connectionString);
            
            //act
            var result =  _sut.CreateSqlDataInterface(_connectionString, fileName, dataModelFileName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IDataInterface>(result);
            Assert.AreSame(_dataInterfaceMock.Object, result);
        }

        [TestCase(ApplicationType.Cati)]
        [TestCase(ApplicationType.AuditTrail)]
        [TestCase(ApplicationType.Cari)]
        [TestCase(ApplicationType.Session)]
        [TestCase(ApplicationType.Configuration)]
        [TestCase(ApplicationType.Meta)]
        public void Given_I_Call_CreateSettingsDataInterface_Then_The_Correct_DataInterface_Is_Created(ApplicationType applicationType)
        {
            //arrange
            const string fileName = "OPN.bcdi";
            _generalDataInterfaceMock.Setup(d => d.ConnectionInfo.GetConnectionString(null)).Returns(_connectionString);

            //act
            _sut.CreateSettingsDataInterface(_connectionString, applicationType, fileName);

            //assert
            _dataInterfaceFactoryMock.Verify(v => v.GetSettingsDataInterfaceForSql(_connectionString, applicationType), Times.Once);
            _generalDataInterfaceMock.Verify(v => v.CreateDatabaseObjects(_connectionString, true), Times.Once);
            _generalDataInterfaceMock.Verify(v => v.SaveToFile(true), Times.Once);
        }

        [Test]
        public void Given_I_Call_CreateSettingsDataInterface_Then_I_Get_A_Correct_DataInterface_Returned()
        {
            //arrange
            const string fileName = "OPN.bcdi";
            _generalDataInterfaceMock.Setup(d => d.ConnectionInfo.GetConnectionString(null)).Returns(_connectionString);

            //act
            var result = _sut.CreateSettingsDataInterface(_connectionString, ApplicationType.Cati, fileName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IGeneralDataInterface>(result);
            Assert.AreSame(_generalDataInterfaceMock.Object, result);
        }
    }
}
