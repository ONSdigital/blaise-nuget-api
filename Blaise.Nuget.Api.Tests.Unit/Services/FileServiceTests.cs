using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class FileServiceTests
    {
        private Mock<IBlaiseConfigurationProvider> _configurationMock;
        private Mock<IDataInterfaceProvider> _dataInterfaceMock;
        private Mock<ICaseService> _caseServiceMock;

        private FileService _sut;

        [SetUp]
        public void SetUpTests()
        {
            _configurationMock = new Mock<IBlaiseConfigurationProvider>();
            _dataInterfaceMock = new Mock<IDataInterfaceProvider>();
            _caseServiceMock = new Mock<ICaseService>();

            _sut = new FileService(_configurationMock.Object, _dataInterfaceMock.Object, _caseServiceMock.Object, null);
        }

        [TestCase(ApplicationType.Cati)]
        [TestCase(ApplicationType.Configuration)]
        [TestCase(ApplicationType.Cari)]
        [TestCase(ApplicationType.AuditTrail)]
        [TestCase(ApplicationType.Meta)]
        [TestCase(ApplicationType.Session)]
        public void
            Given_An_ApplicationType_When_I_Call_CreateSettingsDataInterfaceFile_Then_The_DatabaseName_Is_Updated_In_The_Connection_String(
                ApplicationType applicationType)
        {
            //arrange
            const string fileName = "LMS2101_BK1";
            const string existingConnectionString = "User Id=username;Server=ipaddress;Database=blaise;Password=password";

            var expectedConnectionString
                = $"User Id=username;Server=ipaddress;Database={applicationType.ToString().ToLower()};Password=password";

            _configurationMock.Setup(c => c.DatabaseConnectionString)
                .Returns(existingConnectionString);

            //act
            _sut.CreateSettingsDataInterfaceFile(applicationType, fileName);

            //assert
            _dataInterfaceMock.Verify(v => v.CreateSettingsDataInterface(
                expectedConnectionString, applicationType, fileName), Times.Once);
        }
    }
}
