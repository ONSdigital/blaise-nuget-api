using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseConfigurationProvider _configurationProvider;
        private readonly IDataInterfaceProvider _dataInterfaceService;
        private readonly ICaseService _caseService;
        private readonly IAuditTrailService _auditTrailService;
        private readonly ISqlService _sqlService;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider,
            IDataInterfaceProvider dataInterfaceService,
            ICaseService caseService,
            IAuditTrailService auditTrailService,
            ISqlService sqlService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
            _caseService = caseService;
            _auditTrailService = auditTrailService;
            _sqlService = sqlService;
        }

        public void UpdateQuestionnaireFileWithData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, bool addAudit = false)
        {
            var questionnairePath = ExtractQuestionnairePackage(questionnaireFile);

            UpdateQuestionnaireFileWithAllData(connectionModel, questionnairePath, questionnaireName);
            UpdateQuestionnairePackage(connectionModel, questionnaireFile, questionnaireName, serverParkName, questionnairePath, addAudit);
        }

        public void UpdateQuestionnaireFileWithBatchedData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, int batchSize, bool addAudit = false)
        {
            var questionnairePath = ExtractQuestionnairePackage(questionnaireFile);

            UpdateQuestionnaireFileWithBatchedData(connectionModel, questionnairePath, questionnaireName, batchSize);
            UpdateQuestionnairePackage(connectionModel, questionnaireFile, questionnaireName, serverParkName, questionnairePath, addAudit);
        }

        public void UpdateQuestionnairePackageWithSqlConnection(string questionnaireName, string questionnaireFile, bool createDatabaseObjects)
        {
            var questionnairePath = ExtractQuestionnairePackage(questionnaireFile);

            CreateSqlDataInterface(questionnairePath, questionnaireName, createDatabaseObjects);
            CreateQuestionnairePackage(questionnairePath, questionnaireFile);
        }

        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString
                .Replace("Database=blaise", $"Database={applicationType.ToString().ToLower()}");

            _dataInterfaceService.CreateSettingsDataInterface(databaseConnectionString, applicationType, fileName);
        }

        private void UpdateQuestionnaireFileWithAllData(ConnectionModel connectionModel, string questionnairePath, string questionnaireName)
        {
            var inputDataInterfaceFile = CreateSqlDataInterface(questionnairePath, questionnaireName, false, $"{questionnaireName}_sql");
            var outputDataInterfaceFile = CreateLocalDataInterface(questionnairePath, questionnaireName);

            var cases = _caseService.GetDataSet(connectionModel, inputDataInterfaceFile, null);

            while (!cases.EndOfSet)
            {
                _caseService.WriteDataRecord(connectionModel, (IDataRecord2)cases.ActiveRecord, outputDataInterfaceFile);

                cases.MoveNext();
            }
        }

        private void UpdateQuestionnaireFileWithBatchedData(ConnectionModel connectionModel, string questionnairePath, string questionnaireName, int batchSize)
        {
            var inputDataInterfaceFile = CreateSqlDataInterface(questionnairePath, questionnaireName, false, $"{questionnaireName}_sql");
            var outputDataInterfaceFile = CreateLocalDataInterface(questionnairePath, questionnaireName);

            var caseIds = _sqlService.GetCaseIds(_configurationProvider.DatabaseConnectionString, questionnaireName).Distinct().ToList();

            for (var i = 0; i < caseIds.Count; i += batchSize)
            {
                var batchCaseIds = caseIds.Skip(i).Take(batchSize).ToList();
                var filter = $"qiD.Serial_Number in({string.Join(",", batchCaseIds)})";

                var cases = _caseService.GetDataSet(connectionModel, inputDataInterfaceFile, filter);

                while (!cases.EndOfSet)
                {
                    _caseService.WriteDataRecord(connectionModel, (IDataRecord2)cases.ActiveRecord, outputDataInterfaceFile);

                    cases.MoveNext();
                }
            }
        }

        private string CreateLocalDataInterface(string questionnairePath, string questionnaireName)
        {
            var dataSourceFilePath = BuildFilePath(questionnairePath, questionnaireName, DatabaseSourceExt);
            var dataInterfaceFile = BuildFilePathAndCheckItExists(questionnairePath, questionnaireName, DatabaseFileNameExt);
            var dataModelFile = BuildFilePathAndCheckItExists(questionnairePath, questionnaireName, DatabaseModelExt);

            DeleteFileIfExists(dataSourceFilePath);
            _dataInterfaceService.CreateFileDataInterface(dataSourceFilePath, dataInterfaceFile, dataModelFile);

            return dataInterfaceFile;
        }
        private string CreateSqlDataInterface(string questionnairePath, string questionnaireName, bool createDatabaseObjects, string interfaceName = null)
        {
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;

            Console.WriteLine($"CreateSqlDataInterface with interfaceName '{interfaceName}'");

            var dataInterfaceFile = interfaceName is null
                ? BuildFilePathAndCheckItExists(questionnairePath, questionnaireName, DatabaseFileNameExt)
                : BuildFilePath(questionnairePath, interfaceName, DatabaseFileNameExt);

            var dataModelFile = BuildFilePathAndCheckItExists(questionnairePath, questionnaireName, DatabaseModelExt);

            _dataInterfaceService.CreateSqlDataInterface(databaseConnectionString, dataInterfaceFile,
                dataModelFile, createDatabaseObjects);

            return dataInterfaceFile;
        }

        private void CreateAuditTrailCsv(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string questionnairePath)
        {
            var auditTrailCsvContent = _auditTrailService.CreateAuditTrailCsvContent(connectionModel, questionnaireName, serverParkName);

            if (string.IsNullOrWhiteSpace(auditTrailCsvContent))
            {
                return;
            }

            var pathAndFileName = $@"{questionnairePath}/AuditTrailData.csv";
            File.WriteAllText(pathAndFileName, auditTrailCsvContent);
        }

        private static string ExtractQuestionnairePackage(string questionnaireFile)
        {
            var questionnairePath = $"{Path.GetDirectoryName(questionnaireFile)}\\{Guid.NewGuid()}";

            if (Directory.Exists(questionnairePath))
            {
                Directory.Delete(questionnairePath, true);
            }

            ZipFile.ExtractToDirectory(questionnaireFile, questionnairePath);
            File.Delete(questionnaireFile);

            return questionnairePath;
        }

        private void UpdateQuestionnairePackage(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, string questionnairePath, bool addAudit = false)
        {
            if (addAudit)
            {
                CreateAuditTrailCsv(connectionModel, questionnaireName, serverParkName, questionnairePath);
            }

            CreateQuestionnairePackage(questionnairePath, questionnaireFile);
        }

        private static void CreateQuestionnairePackage(string questionnairePath, string questionnaireFile)
        {
            ZipFile.CreateFromDirectory(questionnairePath, questionnaireFile);
            Directory.Delete(questionnairePath, true);
        }

        private static void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string BuildFilePath(string rootPath, string questionnaireName, string extension)
        {
            return Path.Combine(rootPath, $"{questionnaireName}.{extension}");
        }

        private static string BuildFilePathAndCheckItExists(string rootPath, string questionnaireName, string extension)
        {
            var filePath = BuildFilePath(rootPath, questionnaireName, extension);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Could not find file {filePath}");
            }

            return filePath;
        }
    }
}
