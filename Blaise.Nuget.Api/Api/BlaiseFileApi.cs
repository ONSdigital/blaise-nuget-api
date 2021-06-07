using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseFileApi : IBlaiseFileApi
    {
        private readonly IFileService _fileService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseFileApi(
            IFileService fileService,
            ConnectionModel connectionModel)
        {
            _fileService = fileService;
            _connectionModel = connectionModel;
        }

        public BlaiseFileApi(ConnectionModel connectionModel = null)
        {
            _fileService = UnityProvider.Resolve<IFileService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public void UpdateInstrumentFileWithData(string serverParkName, string instrumentName, string instrumentFile)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _fileService.UpdateInstrumentFileWithData(_connectionModel, instrumentFile, instrumentName, serverParkName);
        }

        public void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _fileService.UpdateInstrumentPackageWithSqlConnection(instrumentName, instrumentFile);
        }

        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            fileName.ThrowExceptionIfNullOrEmpty("fileName");

            _fileService.CreateSettingsDataInterfaceFile(applicationType, fileName);
        }
    }
}
