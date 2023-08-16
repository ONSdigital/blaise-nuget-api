using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string databaseFile);

        void CreateNewDataRecords(ConnectionModel connectionModel, IEnumerable<CaseModel> caseModels,
            string questionnaireName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, 
            Dictionary<string, string> fieldData, string questionnaireName, string serverParkName);
        
        void CreateNewDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord ,string questionnaireName, 
            string serverParkName);

        void CreateNewDataRecord(ConnectionModel connectionModel, string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(ConnectionModel connectionModel, string primaryKeyValue,
            Dictionary<string, string> fieldData, string questionnaireName, string serverParkName);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, 
            Dictionary<string, string> fieldData, string questionnaireName, string serverParkName);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord,
            Dictionary<string, string> fieldData, string databaseFile);

        bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, 
            FieldNameType fieldNameType);

        bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string fieldName);

        void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, 
            string serverParkName);

        void RemoveDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        int GetNumberOfCases(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile);

        Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord);

        void LockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName,
             string lockId);

        void UnLockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName,
            string lockId);

        int GetOutcomeCode(IDataRecord dataRecord);

        DateTime? GetLastUpdated(IDataRecord dataRecord);

        string GetLastUpdatedAsString(IDataRecord dataRecord);

        bool CaseInUseInCati(IDataRecord dataRecord);

        CaseStatusModel GetCaseStatus(IDataRecord dataRecord);
        IEnumerable<CaseStatusModel> GetCaseStatusModelList(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName);
        
        IEnumerable<CaseStatusModel> GetCaseStatusModelList(ConnectionModel connectionModel, string databaseFile);

        CaseModel GetCaseModel(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName,
            string serverParkName);
    }
}
