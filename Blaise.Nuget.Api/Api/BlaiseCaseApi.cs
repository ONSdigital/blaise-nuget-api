﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

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

        public bool CaseExists(string primaryKeyValue, string questionnaireName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.CaseExists(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetPrimaryKeyValue(dataRecord);
        }

        public IDataSet GetCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataSet(_connectionModel, databaseFile);
        }

        public IDataSet GetCases(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataSet(_connectionModel, questionnaireName, serverParkName);
        }

        public IDataRecord GetCase(string primaryKeyValue, string questionnaireName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);
        }

        public IDataRecord GetCase(string primaryKeyValue, string databaseFile)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValue, databaseFile);
        }

        public void CreateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string questionnaireName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, primaryKeyValue, fieldData, questionnaireName, serverParkName);
        }

        public void CreateCase(IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateCase(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");

            _caseService.CreateNewDataRecord(_connectionModel, databaseFile, primaryKeyValue, fieldData);
        }
        
        public void UpdateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string questionnaireName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, primaryKeyValue, fieldData,
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

        public IDataValue GetFieldValue(string primaryKeyValue, string questionnaireName,
            string serverParkName, FieldNameType fieldNameType)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _caseService.GetDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        public void RemoveCase(string primaryKeyValue, string questionnaireName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);
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

        public void LockDataRecord(string primaryKeyValue, string questionnaireName, string serverParkName, string lockId)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

           _caseService.LockDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName, lockId);
        }

        public void UnLockDataRecord(string primaryKeyValue, string questionnaireName, string serverParkName, string lockId)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.UnLockDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName, lockId);
        }

        // Ugghh :(
        public bool DataRecordIsLocked(string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            /* StatNeth recommended way of checking that a data record is locked :( Not good practice */
            try
            {
                _caseService.GetDataRecord(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);
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

        public CaseModel GetCaseModel(string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseModel(_connectionModel, primaryKeyValue, questionnaireName, serverParkName);
        }
    }
}
