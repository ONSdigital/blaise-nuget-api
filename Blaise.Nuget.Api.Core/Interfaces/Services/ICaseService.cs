using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, 
            Dictionary<string, string> fieldData, string instrumentName, string serverParkName);
        
        void CreateNewDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord ,string instrumentName, 
            string serverParkName);

        void CreateNewDataRecord(ConnectionModel connectionModel, string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(ConnectionModel connectionModel, string primaryKeyValue,
            Dictionary<string, string> fieldData, string instrumentName, string serverParkName);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, 
            Dictionary<string, string> fieldData, string instrumentName, string serverParkName);

        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, 
            FieldNameType fieldNameType);

        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            string fieldName);

        void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, 
            string serverParkName);

        void RemoveDataRecords(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile);

        Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord);

        void LockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName,
             string lockId);

        void UnLockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName,
            string lockId);

        int GetOutcomeCode(IDataRecord dataRecord);

        DateTime? GetLastUpdated(IDataRecord dataRecord);

        string GetLastUpdatedAsString(IDataRecord dataRecord);

        bool CaseInUseInCati(IDataRecord dataRecord);

        CaseStatusModel GetCaseStatus(IDataRecord dataRecord);
        IEnumerable<CaseStatusModel> GetCaseStatusList(ConnectionModel connectionModel, string instrumentName,
            string serverParkName);
    }
}
