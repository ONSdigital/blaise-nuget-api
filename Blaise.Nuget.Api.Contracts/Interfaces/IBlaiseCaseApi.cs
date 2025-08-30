namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;
    using System;
    using System.Collections.Generic;

    public interface IBlaiseCaseApi
    {
        bool CaseExists(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord);

        IDataSet GetCases(string databaseFile);

        IDataSet GetCases(string questionnaireName, string serverParkName);

        IDataSet GetFilteredCases(string questionnaireName, string serverParkName, string filter);

        IDataRecord GetCase(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        IDataRecord GetCase(Dictionary<string, string> primaryKeyValues, string databaseFile);

        void CreateCases(List<CaseModel> cases, string questionnaireName, string serverParkName);

        void CreateCase(
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void CreateCase(IDataRecord dataRecord, string questionnaireName, string serverParkName);

        void CreateCase(
            string databaseFile,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData);

        void UpdateCase(
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void UpdateCase(
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void UpdateCase(
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string databaseFile);

        bool FieldExists(string questionnaireName, string serverParkName, FieldNameType fieldNameType);

        bool FieldExists(string questionnaireName, string serverParkName, string fieldName);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        bool FieldExists(IDataRecord dataRecord, string fieldName);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName);

        IDataValue GetFieldValue(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            FieldNameType fieldNameType);

        void RemoveCase(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        void RemoveCases(string questionnaireName, string serverParkName);

        int GetNumberOfCases(string questionnaireName, string serverParkName);

        int GetNumberOfCases(string databaseFile);

        Dictionary<string, string> GetRecordDataFields(IDataRecord dataRecord);

        int GetOutcomeCode(IDataRecord dataRecord);

        void LockDataRecord(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId);

        void UnLockDataRecord(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId);

        bool DataRecordIsLocked(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        DateTime? GetLastUpdated(IDataRecord dataRecord);

        string GetLastUpdatedAsString(IDataRecord dataRecord);

        bool CaseInUseInCati(IDataRecord dataRecord);

        CaseStatusModel GetCaseStatus(IDataRecord dataRecord);

        IEnumerable<CaseStatusModel> GetCaseStatusModelList(string questionnaireName, string serverParkName);

        IEnumerable<CaseStatusModel> GetCaseStatusModelList(string databaseFile);

        CaseModel GetCaseModel(
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);
    }
}
