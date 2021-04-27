using System;
using System.Collections.Generic;
using System.Globalization;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Services
{
    public class CaseService : ICaseService
    {
        private readonly IDataModelService _dataModelService;
        private readonly IDataRecordService _dataRecordService;
        private readonly IKeyService _keyService;
        private readonly IFieldService _fieldService;
        private readonly IDataMapperService _mapperService;

        public CaseService(
            IDataModelService dataModelService, 
            IDataRecordService dataRecordService, 
            IKeyService keyService, 
            IFieldService fieldService, 
            IDataMapperService mapperService)
        {
            _dataModelService = dataModelService;
            _dataRecordService = dataRecordService;
            _keyService = keyService;
            _fieldService = fieldService;
            _mapperService = mapperService;
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKeyValue(dataRecord);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetDataSet(connectionModel, instrumentName, serverParkName);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile)
        {
            return _dataRecordService.GetDataSet(connectionModel, databaseFile);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return _dataRecordService.GetDataRecord(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(connectionModel, instrumentName, serverParkName, fieldNameType);
        }

        public void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            _dataRecordService.DeleteDataRecord(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public void RemoveDataRecords(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            _dataRecordService.DeleteDataRecords(connectionModel, instrumentName, serverParkName);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.GetField(dataRecord, fieldNameType).DataValue;
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(dataRecord, fieldNameType);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string databaseFile)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, databaseFile);
        }

        public bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return _keyService.KeyExists(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var key = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, key, primaryKeyValue, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord ,string instrumentName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, databaseFile);
            var key = _keyService.GetPrimaryKey(dataModel);
            var dataRecord = _dataRecordService.GetDataRecord(dataModel);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, key, primaryKeyValue, fieldData);

            WriteDataRecord(connectionModel, dataRecord, databaseFile);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            var dataRecord = GetDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, fieldData);
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            dataRecord = _mapperService.MapDataRecordFields(dataRecord, fieldData);

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public Dictionary<string, string> GetFieldDataFromRecord(IDataRecord dataRecord)
        {
            return _mapperService.MapFieldDictionaryFromRecord(dataRecord);
        }

        public void LockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName,
            string lockId)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);
            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            _dataRecordService.LockDataRecord(connectionModel, primaryKey, instrumentName, serverParkName, lockId);
        }

        public void UnLockDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, 
            string serverParkName, string lockId)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);
            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            _dataRecordService.UnLockDataRecord(connectionModel, primaryKey, instrumentName, serverParkName, lockId);
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

            if(DateTime.TryParseExact($"{dateField.DataValue.ValueAsText} {timeField.DataValue.ValueAsText}",
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
    }
}
