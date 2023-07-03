﻿using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.IO;
using System.IO.Compression;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseConfigurationProvider _configurationProvider;
        private readonly IDataInterfaceProvider _dataInterfaceService;
        private readonly ICaseService _caseService;
        private readonly IAuditTrailService _auditTrailService;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider,
            IDataInterfaceProvider dataInterfaceService,
            ICaseService caseService,
            IAuditTrailService auditTrailService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
            _caseService = caseService;
            _auditTrailService = auditTrailService;
        }

        public void UpdateQuestionnaireFileWithData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, bool addAudit = false)
        {
            var questionnairePath = ExtractQuestionnairePackage(questionnaireFile);
            var dataSourceFilePath = GetFullFilePath(questionnairePath, questionnaireName, DatabaseSourceExt);
            var dataInterfaceFilePath = GetFullFilePath(questionnairePath, questionnaireName, DatabaseFileNameExt);
            var dataModelFilePath = GetFullFilePath(questionnairePath, questionnaireName, DatabaseModelExt);

            DeleteFileIfExists(dataSourceFilePath);

            _dataInterfaceService.CreateFileDataInterface(dataSourceFilePath,
                dataInterfaceFilePath, dataModelFilePath);

            if (addAudit)
            {
                CreateAuditTrailCsv(connectionModel, questionnaireName, serverParkName, questionnairePath);
            }

            var cases = _caseService.GetDataSet(connectionModel, questionnaireName, serverParkName);

            while (!cases.EndOfSet)
            {
                _caseService.WriteDataRecord(connectionModel, (IDataRecord2)cases.ActiveRecord, dataInterfaceFilePath);

                cases.MoveNext();
            }
            CreateQuestionnairePackage(questionnairePath, questionnaireFile);
        }

        public void UpdateQuestionnairePackageWithSqlConnection(string questionnaireName, string questionnaireFile)
        {
            var questionnairePath = ExtractQuestionnairePackage(questionnaireFile);
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;
            var fileName = GetFullFilePath(questionnairePath, questionnaireName, DatabaseFileNameExt);
            var dataModelFileName = GetFullFilePath(questionnairePath, questionnaireName, DatabaseModelExt);

            _dataInterfaceService.CreateSqlDataInterface(databaseConnectionString, fileName,
                dataModelFileName);

            CreateQuestionnairePackage(questionnairePath, questionnaireFile);
        }

        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString
                .Replace("Database=blaise", $"Database={applicationType.ToString().ToLower()}");

            _dataInterfaceService.CreateSettingsDataInterface(databaseConnectionString, applicationType, fileName);
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

        private static string GetFullFilePath(string filePath, string questionnaireName, string extension)
        {
            return Path.Combine(filePath, $"{questionnaireName}.{extension}");
        }
    }
}
