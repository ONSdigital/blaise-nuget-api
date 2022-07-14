﻿using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataRecordService
    {
        IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string databaseFile, IKey primaryKey);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string questionnaireName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile);

        void DeleteDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName);

        void DeleteDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        int GetNumberOfRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        int GetNumberOfRecords(ConnectionModel connectionModel, string databaseFile);

        void LockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName,
            string lockId);

        void UnLockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName,
             string lockId);
    }
}