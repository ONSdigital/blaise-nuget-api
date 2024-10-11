using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseCaseApi : IBlaiseCaseApi
    {
        private readonly ICaseService _caseService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseCaseApi(
            ICaseService caseService,
            ConnectionModel connectionModel)
        {
            _caseService = caseService;
            _connectionModel = connectionModel;
        }

        public BlaiseCaseApi(ConnectionModel connectionModel = null)
        {

            _caseService = UnityProvider.Resolve<ICaseService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool CaseExists(Dictionary<string, string> primaryKeyValues, string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.CaseExists(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        public Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetPrimaryKeyValues(dataRecord);
        }

        public IDataSet GetCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataSet(_connectionModel, databaseFile, null);
        }

        public IDataSet GetCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataSet(_connectionModel, questionnaireName, serverParkName, null);
        }

        public IDataSet GetFilteredCases(string questionnaireName, string serverParkName, string filter)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            filter.ThrowExceptionIfNullOrEmpty("filter");

            return _caseService.GetDataSet(_connectionModel, questionnaireName, serverParkName, filter);
        }

        public IDataRecord GetCase(Dictionary<string, string> primaryKeyValues, string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        public IDataRecord GetCase(Dictionary<string, string> primaryKeyValues, string databaseFile)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValues, databaseFile);
        }

        public void CreateCases(List<CaseModel> cases, string questionnaireName, string serverParkName)
        {
            cases.ThrowExceptionIfNullOrEmpty("cases");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecords(_connectionModel, cases, questionnaireName, serverParkName);
        }

        public void CreateCase(Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, primaryKeyValues, fieldData, questionnaireName, serverParkName);
        }

        public void CreateCase(IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateCase(string databaseFile, Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");

            _caseService.CreateNewDataRecord(_connectionModel, databaseFile, primaryKeyValues, fieldData);
        }

        public void UpdateCase(Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, primaryKeyValues, fieldData,
                questionnaireName, serverParkName);
        }

        public void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, questionnaireName, serverParkName);
        }

        public void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string databaseFile)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, databaseFile);
        }

        public bool FieldExists(string questionnaireName, string serverParkName, FieldNameType fieldNameType)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.FieldExists(_connectionModel, questionnaireName, serverParkName, fieldNameType);
        }

        public bool FieldExists(string questionnaireName, string serverParkName, string fieldName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            fieldName.ThrowExceptionIfNullOrEmpty("fieldName");

            return _caseService.FieldExists(_connectionModel, questionnaireName, serverParkName, fieldName);
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.FieldExists(dataRecord, fieldNameType);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetFieldValue(dataRecord, fieldNameType);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldName.ThrowExceptionIfNullOrEmpty("fieldName");

            return _caseService.GetFieldValue(dataRecord, fieldName);
        }

        public IDataValue GetFieldValue(Dictionary<string, string> primaryKeyValues, string questionnaireName,
            string serverParkName, FieldNameType fieldNameType)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _caseService.GetDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        public void RemoveCase(Dictionary<string, string> primaryKeyValues, string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        public void RemoveCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecords(_connectionModel, questionnaireName, serverParkName);
        }

        public int GetNumberOfCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetNumberOfCases(_connectionModel, questionnaireName, serverParkName);
        }

        public int GetNumberOfCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetNumberOfCases(_connectionModel, databaseFile);
        }

        public Dictionary<string, string> GetRecordDataFields(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetFieldDataFromRecord(dataRecord);
        }

        public void LockDataRecord(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName, string lockId)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.LockDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName, lockId);
        }

        public void UnLockDataRecord(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName, string lockId)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.UnLockDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName, lockId);
        }

        // Ugghh :(
        public bool DataRecordIsLocked(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            /* StatNeth recommended way of checking that a data record is locked :( Not good practice */
            try
            {
                _caseService.GetDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
                return false;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }

            return true;
        }

        public int GetOutcomeCode(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetOutcomeCode(dataRecord);
        }

        public DateTime? GetLastUpdated(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetLastUpdated(dataRecord);
        }

        public string GetLastUpdatedAsString(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetLastUpdatedAsString(dataRecord);
        }

        public bool CaseInUseInCati(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.CaseInUseInCati(dataRecord);
        }

        public CaseStatusModel GetCaseStatus(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetCaseStatus(dataRecord);
        }

        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseStatusModelList(_connectionModel, questionnaireName, serverParkName);
        }

        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetCaseStatusModelList(_connectionModel, databaseFile);
        }

        public CaseModel GetCaseModel(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseModel(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }
    }
}
