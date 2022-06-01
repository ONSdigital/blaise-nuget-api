using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCaseApi
    {
        bool CaseExists(string primaryKeyValue, string questionnaireName, 
            string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);
        IDataSet GetCases(string databaseFile);
        IDataSet GetCases(string questionnaireName, string serverParkName);

        IDataRecord GetCase(string primaryKeyValue, string questionnaireName, 
            string serverParkName);

        void CreateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string questionnaireName, string serverParkName);

        void CreateCase(IDataRecord dataRecord, string questionnaireName, string serverParkName);

        void CreateCase(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateCase(string primaryKeyValue, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName);

        void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName);

        bool FieldExists(string questionnaireName, string serverParkName, FieldNameType fieldNameType);

        bool FieldExists(string questionnaireName, string serverParkName, string fieldName);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName);

        IDataValue GetFieldValue(string primaryKeyValue, string questionnaireName,
            string serverParkName, FieldNameType fieldNameType);

        void RemoveCase(string primaryKeyValue, string questionnaireName, 
            string serverParkName);

        void RemoveCases(string questionnaireName, string serverParkName);

        int GetNumberOfCases(string questionnaireName, string serverParkName);
        int GetNumberOfCases(string databaseFile);

        Dictionary<string, string> GetRecordDataFields(IDataRecord dataRecord);

        int GetOutcomeCode(IDataRecord dataRecord);

        void LockDataRecord(string primaryKeyValue, string questionnaireName, string serverParkName,
            string lockId);

        void UnLockDataRecord(string primaryKeyValue, string questionnaireName, string serverParkName,
           string lockId);
        
        bool DataRecordIsLocked(string primaryKeyValue, string questionnaireName, string serverParkName);

        DateTime? GetLastUpdated(IDataRecord dataRecord);

        string GetLastUpdatedAsString(IDataRecord dataRecord);

        bool CaseInUseInCati(IDataRecord dataRecord);

        CaseStatusModel GetCaseStatus(IDataRecord dataRecord);

        IEnumerable<CaseStatusModel> GetCaseStatusModelList(string questionnaireName, string serverParkName);

        CaseModel GetCaseModel(string primaryKeyValue, string questionnaireName, string serverParkName);
    }
}