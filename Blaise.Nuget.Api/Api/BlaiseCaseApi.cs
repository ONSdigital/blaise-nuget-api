namespace Blaise.Nuget.Api.Api
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Extensions;
    using Blaise.Nuget.Api.Providers;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;

    public class BlaiseCaseApi : IBlaiseCaseApi
    {
        private readonly ICaseService _caseService;

        private readonly ConnectionModel _connectionModel;

        public BlaiseCaseApi(
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

        /// <inheritdoc/>
        public bool CaseExists(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.CaseExists(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetPrimaryKeyValues(dataRecord);
        }

        /// <inheritdoc/>
        public IDataSet GetCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataSet(_connectionModel, databaseFile, null);
        }

        /// <inheritdoc/>
        public IDataSet GetCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataSet(_connectionModel, questionnaireName, serverParkName, null);
        }

        /// <inheritdoc/>
        public IDataSet GetFilteredCases(string questionnaireName, string serverParkName, string filter)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            filter.ThrowExceptionIfNullOrEmpty("filter");

            return _caseService.GetDataSet(_connectionModel, questionnaireName, serverParkName, filter);
        }

        /// <inheritdoc/>
        public IDataRecord GetCase(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public IDataRecord GetCase(Dictionary<string, string> primaryKeyValues, string databaseFile)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValues, databaseFile);
        }

        /// <inheritdoc/>
        public void CreateCases(List<CaseModel> cases, string questionnaireName, string serverParkName)
        {
            cases.ThrowExceptionIfNullOrEmpty("cases");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecords(_connectionModel, cases, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void CreateCase(
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, primaryKeyValues, fieldData, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void CreateCase(IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void CreateCase(string databaseFile, Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");

            _caseService.CreateNewDataRecord(_connectionModel, databaseFile, primaryKeyValues, fieldData);
        }

        /// <inheritdoc/>
        public void UpdateCase(
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(
                _connectionModel,
                primaryKeyValues,
                fieldData,
                questionnaireName,
                serverParkName);
        }

        /// <inheritdoc/>
        public void UpdateCase(
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void UpdateCase(
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string databaseFile)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, databaseFile);
        }

        /// <inheritdoc/>
        public bool FieldExists(string questionnaireName, string serverParkName, FieldNameType fieldNameType)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.FieldExists(_connectionModel, questionnaireName, serverParkName, fieldNameType.FullName());
        }

        /// <inheritdoc/>
        public bool FieldExists(string questionnaireName, string serverParkName, string fieldName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            fieldName.ThrowExceptionIfNullOrEmpty("fieldName");

            return _caseService.FieldExists(_connectionModel, questionnaireName, serverParkName, fieldName);
        }

        /// <inheritdoc/>
        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.FieldExists(dataRecord, fieldNameType.FullName());
        }

        /// <inheritdoc/>
        public bool FieldExists(IDataRecord dataRecord, string fieldName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.FieldExists(dataRecord, fieldName);
        }

        /// <inheritdoc/>
        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetFieldValue(dataRecord, fieldNameType.FullName());
        }

        /// <inheritdoc/>
        public IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldName.ThrowExceptionIfNullOrEmpty("fieldName");

            return _caseService.GetFieldValue(dataRecord, fieldName);
        }

        /// <inheritdoc/>
        public IDataValue GetFieldValue(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            FieldNameType fieldNameType)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _caseService.GetDataRecord(
                _connectionModel,
                primaryKeyValues,
                questionnaireName,
                serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        /// <inheritdoc/>
        public void RemoveCase(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void RemoveCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecords(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public int GetNumberOfCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetNumberOfCases(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public int GetNumberOfCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetNumberOfCases(_connectionModel, databaseFile);
        }

        /// <inheritdoc/>
        public Dictionary<string, string> GetRecordDataFields(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetFieldDataFromRecord(dataRecord);
        }

        /// <inheritdoc/>
        public void LockDataRecord(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.LockDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName, lockId);
        }

        /// <inheritdoc/>
        public void UnLockDataRecord(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.UnLockDataRecord(_connectionModel, primaryKeyValues, questionnaireName, serverParkName, lockId);
        }

        /// <inheritdoc/>
        public bool DataRecordIsLocked(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            // StatNeth recommended way of checking that a data record is locked
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

        /// <inheritdoc/>
        public int GetOutcomeCode(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetOutcomeCode(dataRecord);
        }

        /// <inheritdoc/>
        public DateTime? GetLastUpdated(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetLastUpdated(dataRecord);
        }

        /// <inheritdoc/>
        public string GetLastUpdatedAsString(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetLastUpdatedAsString(dataRecord);
        }

        /// <inheritdoc/>
        public bool CaseInUseInCati(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.CaseInUseInCati(dataRecord);
        }

        /// <inheritdoc/>
        public CaseStatusModel GetCaseStatus(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetCaseStatus(dataRecord);
        }

        /// <inheritdoc/>
        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseStatusModelList(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetCaseStatusModelList(_connectionModel, databaseFile);
        }

        /// <inheritdoc/>
        public CaseModel GetCaseModel(Dictionary<string, string> primaryKeyValues, string questionnaireName, string serverParkName)
        {
            primaryKeyValues.ThrowExceptionIfNull("primaryKeyValues");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseModel(_connectionModel, primaryKeyValues, questionnaireName, serverParkName);
        }
    }
}
