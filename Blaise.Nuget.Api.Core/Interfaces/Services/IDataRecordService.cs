﻿using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataRecordService
    {
        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(string databaseFile);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string databaseFile);

        void DeleteDataRecord(ConnectionModel connectionModel, IKey primaryKey, string instrumentName, string serverParkName);
        int GetNumberOfRecords(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        int GetNumberOfRecords(string databaseFile);
    }
}