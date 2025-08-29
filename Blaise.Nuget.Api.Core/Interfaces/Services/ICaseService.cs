using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        bool CaseExists(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord);

        IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string filter);

        IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile, string filter);

        IDataRecord GetDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        IDataRecord GetDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string databaseFile);

        void CreateNewDataRecords(
            ConnectionModel connectionModel,
            IEnumerable<CaseModel> caseModels,
            string questionnaireName,
            string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile);

        void CreateNewDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void CreateNewDataRecord(
            ConnectionModel connectionModel,
            IDataRecord dataRecord,
            string questionnaireName,
            string serverParkName);

        void CreateNewDataRecord(
            ConnectionModel connectionModel,
            string databaseFile,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData);

        void UpdateDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void UpdateDataRecord(
            ConnectionModel connectionModel,
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName);

        void UpdateDataRecord(
            ConnectionModel connectionModel,
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string databaseFile);

        bool FieldExists(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName,
            string fieldName);

        bool FieldExists(IDataRecord dataRecord, string fieldName);

        void RemoveDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);

        void RemoveDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName);

        int GetNumberOfCases(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile);

        Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord);

        void LockDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId);

        void UnLockDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId);

        int GetOutcomeCode(IDataRecord dataRecord);

        DateTime? GetLastUpdated(IDataRecord dataRecord);

        string GetLastUpdatedAsString(IDataRecord dataRecord);

        bool CaseInUseInCati(IDataRecord dataRecord);

        CaseStatusModel GetCaseStatus(IDataRecord dataRecord);

        IEnumerable<CaseStatusModel> GetCaseStatusModelList(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName);

        IEnumerable<CaseStatusModel> GetCaseStatusModelList(ConnectionModel connectionModel, string databaseFile);

        CaseModel GetCaseModel(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName);
    }
}
