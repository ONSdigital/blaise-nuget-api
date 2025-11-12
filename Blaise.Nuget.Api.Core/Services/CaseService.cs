namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.DataLink;
    using StatNeth.Blaise.API.DataRecord;

    public class CaseService : ICaseService
    {
        private readonly IDataModelService _dataModelService;

        private readonly IDataRecordService _dataRecordService;

        private readonly IKeyService _keyService;

        private readonly IFieldService _fieldService;

        private readonly IDataRecordMapper _recordMapper;

        public CaseService(
            IDataModelService dataModelService,
            IDataRecordService dataRecordService,
            IKeyService keyService,
            IFieldService fieldService,
            IDataRecordMapper recordMapper)
        {
            _dataModelService = dataModelService;
            _dataRecordService = dataRecordService;
            _keyService = keyService;
            _fieldService = fieldService;
            _recordMapper = recordMapper;
        }

        public Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKeyValues(dataRecord);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string filter)
        {
            return _dataRecordService.GetDataSet(connectionModel, questionnaireName, serverParkName, filter);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile, string filter = null)
        {
            return _dataRecordService.GetDataSet(connectionModel, databaseFile, filter);
        }

        public IDataRecord GetDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            if (!_keyService.KeyExists(connectionModel, primaryKey, questionnaireName, serverParkName))
            {
                throw new DataNotFoundException($"Case '{primaryKeyValues}' not found for '{questionnaireName}' on server park '{serverParkName}'");
            }

            return _dataRecordService.GetDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, Dictionary<string, string> primaryKeyValues, string databaseFile)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, databaseFile);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValues(primaryKey, primaryKeyValues);

            return _dataRecordService.GetDataRecord(connectionModel, databaseFile, primaryKey);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string fieldName)
        {
            return _fieldService.FieldExists(connectionModel, questionnaireName, serverParkName, fieldName);
        }

        public bool FieldExists(IDataRecord dataRecord, string fieldName)
        {
            return _fieldService.FieldExists(dataRecord, fieldName);
        }

        public void RemoveDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            _dataRecordService.DeleteDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public void RemoveDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            _dataRecordService.DeleteDataRecords(connectionModel, questionnaireName, serverParkName);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName)
        {
            return _fieldService.GetField(dataRecord, fieldName).DataValue;
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, questionnaireName, serverParkName);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, databaseFile);
        }

        public bool CaseExists(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            return _keyService.KeyExists(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecords(
            ConnectionModel connectionModel,
            IEnumerable<CaseModel> caseModels,
            string questionnaireName,
            string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var key = _keyService.GetPrimaryKey(dataModel);

            var dataRecords = caseModels.Select(caseModel =>
                {
                    var dataRecord = _dataRecordService.GetDataRecord(dataModel);
                    return _recordMapper.MapDataRecordFields(dataRecord, key, caseModel.PrimaryKeyValues, caseModel.FieldData);
                }).ToList();

            _dataRecordService.WriteDataRecords(connectionModel, dataRecords, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, primaryKey, primaryKeyValues, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(
            ConnectionModel connectionModel,
            string databaseFile,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, databaseFile);
            var key = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, key, primaryKeyValues, fieldData);

            WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public void UpdateDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            var dataRecord = GetDataRecord(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void UpdateDataRecord(
            ConnectionModel connectionModel,
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string questionnaireName,
            string serverParkName)
        {
            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void UpdateDataRecord(
            ConnectionModel connectionModel,
            IDataRecord dataRecord,
            Dictionary<string, string> fieldData,
            string databaseFile)
        {
            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord)
        {
            return _recordMapper.MapFieldDictionaryFromRecord(dataRecord);
        }

        public void LockDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            _dataRecordService.LockDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName, lockId);
        }

        public void UnLockDataRecord(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName,
            string lockId)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            _dataRecordService.UnLockDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName, lockId);
        }

        public int GetOutcomeCode(IDataRecord dataRecord)
        {
            return (int)GetFieldValue(dataRecord, FieldNameType.HOut.FullName()).IntegerValue;
        }

        public DateTime? GetLastUpdated(IDataRecord dataRecord)
        {
            if (!_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdatedDate.FullName()) ||
                !_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdatedTime.FullName()))
            {
                return null;
            }

            var dateField = _fieldService.GetField(dataRecord, FieldNameType.LastUpdatedDate.FullName());
            var timeField = _fieldService.GetField(dataRecord, FieldNameType.LastUpdatedTime.FullName());

            if (string.IsNullOrWhiteSpace(dateField?.DataValue?.ValueAsText) ||
                string.IsNullOrWhiteSpace(timeField?.DataValue?.ValueAsText))
            {
                return null;
            }

            if (DateTime.TryParseExact(
                $"{dateField.DataValue.ValueAsText} {timeField.DataValue.ValueAsText}",
                "dd-MM-yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTime))
            {
                return dateTime;
            }

            return null;
        }

        public string GetLastUpdatedAsString(IDataRecord dataRecord)
        {
            if (!_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdated.FullName()))
            {
                return null;
            }

            var field = _fieldService.GetField(dataRecord, FieldNameType.LastUpdated.FullName());

            return field?.DataValue?.ValueAsText;
        }

        public bool CaseInUseInCati(IDataRecord dataRecord)
        {
            var lastUpdated = GetLastUpdated(dataRecord);

            if (lastUpdated == null)
            {
                return false;
            }

            return lastUpdated.Value.AddMinutes(31) > DateTime.Now;
        }

        public CaseStatusModel GetCaseStatus(IDataRecord dataRecord)
        {
            return new CaseStatusModel(
                GetPrimaryKeyValues(dataRecord),
                GetOutcomeCode(dataRecord),
                GetLastUpdatedAsString(dataRecord));
        }

        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName)
        {
            var caseStatusList = new List<CaseStatusModel>();
            var cases = GetDataSet(connectionModel, questionnaireName, serverParkName, null);

            while (!cases.EndOfSet)
            {
                var record = cases.ActiveRecord;

                caseStatusList.Add(GetCaseStatus(record));

                cases.MoveNext();
            }

            return caseStatusList;
        }

        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(ConnectionModel connectionModel, string databaseFile)
        {
            var caseStatusList = new List<CaseStatusModel>();
            var cases = GetDataSet(connectionModel, databaseFile);

            while (!cases.EndOfSet)
            {
                var record = cases.ActiveRecord;

                caseStatusList.Add(GetCaseStatus(record));

                cases.MoveNext();
            }

            return caseStatusList;
        }

        public CaseModel GetCaseModel(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            var dataRecord = GetDataRecord(connectionModel, primaryKeyValues, questionnaireName, serverParkName);

            return new CaseModel(primaryKeyValues, GetFieldDataFromRecord(dataRecord));
        }

        private IKey GetPrimaryKey(
            ConnectionModel connectionModel,
            Dictionary<string, string> primaryKeyValues,
            string questionnaireName,
            string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValues(primaryKey, primaryKeyValues);

            return primaryKey;
        }
    }
}
