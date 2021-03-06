﻿using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataRecordService : IDataRecordService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;
        private readonly ILocalDataLinkProvider _localDataLinkProvider;

        public DataRecordService(
            IRemoteDataLinkProvider remoteDataLinkProvider, 
            ILocalDataLinkProvider localDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
            _localDataLinkProvider = localDataLinkProvider;
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            return dataLink.Read(null);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            return dataLink.Read(null);
        }

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            return DataRecordManager.GetDataRecord(dataModel);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            return dataLink.ReadRecord(key);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Write(dataRecord);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            dataLink.Write(dataRecord);
        }

        public void DeleteDataRecord(ConnectionModel connectionModel, IKey primaryKey, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Delete(primaryKey);
        }

        public void DeleteDataRecords(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.DeleteAll();
        }

        public int GetNumberOfRecords(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var records = GetDataSet(connectionModel, instrumentName, serverParkName);

            return GetNumberOfRecords(records);
        }

        public int GetNumberOfRecords(ConnectionModel connectionModel, string databaseFile)
        {
            var records = GetDataSet(connectionModel, databaseFile);

            return GetNumberOfRecords(records);
        }

        public void LockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string instrumentName, string serverParkName, 
            string lockId)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Lock(primaryKey, lockId);
        }

        public void UnLockDataRecord(ConnectionModel connectionModel,IKey primaryKey, string instrumentName, string serverParkName, 
            string lockId)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            dataLink.Unlock(primaryKey, lockId);
        }

        private static int GetNumberOfRecords(IDataSet records)
        {
            var numberOfRecords = 0;

            while (!records.EndOfSet)
            {
                numberOfRecords++;
                records.MoveNext();
            }

            return numberOfRecords;
        }
    }
}
