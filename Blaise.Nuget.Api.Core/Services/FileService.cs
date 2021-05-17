using System;
using System.IO;
using System.IO.Compression;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseConfigurationProvider _configurationProvider;
        private readonly IDataInterfaceProvider _dataInterfaceService;
        private readonly ICaseService _caseService;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider, 
            IDataInterfaceProvider dataInterfaceService, 
            ICaseService caseService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
            _caseService = caseService;
        }

        public void UpdateInstrumentFileWithData(ConnectionModel connectionModel, string instrumentFile, string instrumentName, string serverParkName)
        {
            var instrumentPath = ExtractInstrumentPackage(instrumentFile);
            var dataSourceFilePath = GetFullFilePath(instrumentPath, instrumentName, DatabaseSourceExt);
            var dataInterfaceFilePath = GetFullFilePath(instrumentPath, instrumentName, DatabaseFileNameExt);
            var dataModelFilePath = GetFullFilePath(instrumentPath, instrumentName, DatabaseModelExt);

            DeleteFileIfExists(dataSourceFilePath);

            _dataInterfaceService.CreateFileDataInterface(dataSourceFilePath, 
                dataInterfaceFilePath, dataModelFilePath);

            var cases = _caseService.GetDataSet(connectionModel, instrumentName, serverParkName);

            while (!cases.EndOfSet)
            {
                _caseService.WriteDataRecord(connectionModel, (IDataRecord2)cases.ActiveRecord, dataInterfaceFilePath);

                cases.MoveNext();
            }

            CreateInstrumentPackage(instrumentPath, instrumentFile);
        }

        public void UpdateInstrumentPackageWithSqlConnection(string instrumentName, string instrumentFile)
        {
            var instrumentPath = ExtractInstrumentPackage(instrumentFile);
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;
            var fileName = GetFullFilePath(instrumentPath, instrumentName, DatabaseFileNameExt);
            var dataModelFileName = GetFullFilePath(instrumentPath, instrumentName, DatabaseModelExt);

            _dataInterfaceService.CreateSqlDataInterface(databaseConnectionString, fileName,
                dataModelFileName);

            CreateInstrumentPackage(instrumentPath, instrumentFile);
        }

        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString
                .Replace("Database=blaise", $"Database={applicationType.ToString().ToLower()}");

            _dataInterfaceService.CreateSettingsDataInterface(databaseConnectionString, applicationType, fileName);
        }

        private static string ExtractInstrumentPackage(string instrumentFile)
        {
            var instrumentPath =  $"{Path.GetDirectoryName(instrumentFile)}\\{Guid.NewGuid()}";

            if (Directory.Exists(instrumentPath))
            {
                Directory.Delete(instrumentPath, true);
            }

            ZipFile.ExtractToDirectory(instrumentFile, instrumentPath);
            File.Delete(instrumentFile);

            return instrumentPath;
        }

        private static void CreateInstrumentPackage(string instrumentPath, string instrumentFile)
        {
            ZipFile.CreateFromDirectory(instrumentPath, instrumentFile);
            Directory.Delete(instrumentPath, true);
        }

        private static void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string GetFullFilePath(string filePath, string instrumentName, string extension)
        {
            return Path.Combine(filePath, $"{instrumentName}.{extension}");
        }
    }
}
