using System;
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
           var unityProvider = new UnityProvider();
           unityProvider.RegisterDependencies();

           _caseService = unityProvider.Resolve<ICaseService>();

           var configurationProvider = unityProvider.Resolve<IBlaiseConfigurationProvider>();
           _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool CaseExists(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.CaseExists(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
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

        public IDataSet GetCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataSet(_connectionModel, instrumentName, serverParkName);
        }

        public IDataRecord GetCase(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void CreateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, primaryKeyValue, fieldData, instrumentName, serverParkName);
        }

        public void CreateCase(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void CreateCase(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");

            _caseService.CreateNewDataRecord(_connectionModel, databaseFile, primaryKeyValue, fieldData);
        }
        
        public void UpdateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, primaryKeyValue, fieldData,
                instrumentName, serverParkName);
        }

        public void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, instrumentName, serverParkName);
        }

        public bool FieldExists(string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.FieldExists(_connectionModel, instrumentName, serverParkName, fieldNameType);
        }

        public bool FieldExists(string instrumentName, string serverParkName, string fieldName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            fieldName.ThrowExceptionIfNullOrEmpty("fieldName");

            return _caseService.FieldExists(_connectionModel, instrumentName, serverParkName, fieldName);
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

        public IDataValue GetFieldValue(string primaryKeyValue, string instrumentName,
            string serverParkName, FieldNameType fieldNameType)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _caseService.GetDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        public void RemoveCase(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void RemoveCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecords(_connectionModel, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetNumberOfCases(_connectionModel, instrumentName, serverParkName);
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

        public void LockDataRecord(string primaryKeyValue, string instrumentName, string serverParkName, string lockId)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

           _caseService.LockDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName, lockId);
        }

        public void UnLockDataRecord(string primaryKeyValue, string instrumentName, string serverParkName, string lockId)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            lockId.ThrowExceptionIfNullOrEmpty("lockId");

            _caseService.UnLockDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName, lockId);
        }

        //Ugghh :(
        public bool DataRecordIsLocked(string primaryKeyValue, string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            try
            {
                _caseService.GetDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

        public DateTime? GetLiveDate(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetLiveDate(dataRecord);
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

        public IEnumerable<CaseStatusModel> GetCaseStatusList(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetCaseStatusList(_connectionModel, instrumentName, serverParkName);
        }
    }
}
