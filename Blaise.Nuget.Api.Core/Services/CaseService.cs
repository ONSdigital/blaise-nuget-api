using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Blaise.Nuget.Api.Core.Services
{
    using System.Linq;

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

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKeyValue(dataRecord);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            return _dataRecordService.GetDataSet(connectionModel, questionnaireName, serverParkName);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile)
        {
            return _dataRecordService.GetDataSet(connectionModel, databaseFile);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            if (!_keyService.KeyExists(connectionModel, primaryKey, questionnaireName, serverParkName))
            {
                throw new DataNotFoundException($"Case '{primaryKeyValue}' not found for '{questionnaireName}' on server park '{serverParkName}'");
            }

            return _dataRecordService.GetDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string databaseFile)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, databaseFile);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return _dataRecordService.GetDataRecord(connectionModel, databaseFile, primaryKey);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(connectionModel, questionnaireName, serverParkName, fieldNameType);
        }

        public bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string fieldName)
        {
            return _fieldService.FieldExists(connectionModel, questionnaireName, serverParkName, fieldName);
        }

        public void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            _dataRecordService.DeleteDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public void RemoveDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            _dataRecordService.DeleteDataRecords(connectionModel, questionnaireName, serverParkName);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.GetField(dataRecord, fieldNameType).DataValue;
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, string fieldName)
        {
            return _fieldService.GetField(dataRecord, fieldName).DataValue;
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(dataRecord, fieldNameType);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, questionnaireName, serverParkName);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, databaseFile);
        }

        public bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            return _keyService.KeyExists(connectionModel, primaryKey, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecords(ConnectionModel connectionModel, IEnumerable<CaseModel> caseModels, string questionnaireName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var key = _keyService.GetPrimaryKey(dataModel);

            var dataRecords = caseModels.Select(caseModel =>
                {
                    var dataRecord = _dataRecordService.GetDataRecord(dataModel);
                    return _recordMapper.MapDataRecordFields(dataRecord, key, caseModel.CaseId, caseModel.FieldData);
                }).ToList();

            _dataRecordService.WriteDataRecords(connectionModel, dataRecords, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string questionnaireName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, primaryKey, primaryKeyValue, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, databaseFile);
            var key = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, key, primaryKeyValue, fieldData);

            WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName)
        {
            var dataRecord = GetDataRecord(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string questionnaireName, string serverParkName)
        {
            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, questionnaireName, serverParkName);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string databaseFile)
        {
            dataRecord = _recordMapper.MapDataRecordFields(dataRecord, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord)
        {
            return _recordMapper.MapFieldDictionaryFromRecord(dataRecord);
        }

        public void LockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName,
            string lockId)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            _dataRecordService.LockDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName, lockId);
        }

        public void UnLockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName,
            string serverParkName, string lockId)
        {
            var primaryKey = GetPrimaryKey(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            _dataRecordService.UnLockDataRecord(connectionModel, primaryKey, questionnaireName, serverParkName, lockId);
        }

        public int GetOutcomeCode(IDataRecord dataRecord)
        {
            return (int)GetFieldValue(dataRecord, FieldNameType.HOut).IntegerValue;
        }

        public DateTime? GetLastUpdated(IDataRecord dataRecord)
        {
            if (!_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdatedDate) ||
                !_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdatedTime))
            {
                return null;
            }

            var dateField = _fieldService.GetField(dataRecord, FieldNameType.LastUpdatedDate);
            var timeField = _fieldService.GetField(dataRecord, FieldNameType.LastUpdatedTime);

            if (string.IsNullOrWhiteSpace(dateField?.DataValue?.ValueAsText) ||
                string.IsNullOrWhiteSpace(timeField?.DataValue?.ValueAsText))
            {
                return null;
            }

            if (DateTime.TryParseExact($"{dateField.DataValue.ValueAsText} {timeField.DataValue.ValueAsText}",
                "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }

            return null;
        }

        public string GetLastUpdatedAsString(IDataRecord dataRecord)
        {
            if (!_fieldService.FieldExists(dataRecord, FieldNameType.LastUpdated))
            {
                return null;
            }

            var field = _fieldService.GetField(dataRecord, FieldNameType.LastUpdated);

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
                GetPrimaryKeyValue(dataRecord),
                GetOutcomeCode(dataRecord),
                GetLastUpdatedAsString(dataRecord));
        }

        public IEnumerable<CaseStatusModel> GetCaseStatusModelList(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var caseStatusList = new List<CaseStatusModel>();
            var cases = GetDataSet(connectionModel, questionnaireName, serverParkName);

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

        public CaseModel GetCaseModel(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName,
            string serverParkName)
        {
            var dataRecord = GetDataRecord(connectionModel, primaryKeyValue, questionnaireName, serverParkName);

            return new CaseModel(primaryKeyValue, GetFieldDataFromRecord(dataRecord));
        }

        private IKey GetPrimaryKey(ConnectionModel connectionModel, string primaryKeyValue, string questionnaireName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return primaryKey;
        }
    }
}
