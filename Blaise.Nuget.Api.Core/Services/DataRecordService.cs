using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
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

        /// <inheritdoc/>
        public IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string filter)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            return dataLink.Read(filter, null);
        }

        /// <inheritdoc/>
        public IDataSet GetDataSet(ConnectionModel connectionModel, string databaseFile, string filter)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            return dataLink.Read(filter, null);
        }

        /// <inheritdoc/>
        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            return DataRecordManager.GetDataRecord(dataModel);
        }

        /// <inheritdoc/>
        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string databaseFile, IKey primaryKey)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            return dataLink.ReadRecord(primaryKey);
        }

        /// <inheritdoc/>
        public IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            return dataLink.ReadRecord(key);
        }

        /// <inheritdoc/>
        public void WriteDataRecords(ConnectionModel connectionModel, IEnumerable<IDataRecord> dataRecords, string questionnaireName, string serverParkName)
        {
            var dataSet = DataLinkManager.GetDataSet(dataRecords);
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Write(dataSet);
        }

        /// <inheritdoc/>
        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Write(dataRecord);
        }

        /// <inheritdoc/>
        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            dataLink.Write(dataRecord);
        }

        /// <inheritdoc/>
        public void DeleteDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Delete(primaryKey);
        }

        /// <inheritdoc/>
        public void DeleteDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.DeleteAll();
        }

        /// <inheritdoc/>
        public int GetNumberOfRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var records = GetDataSet(connectionModel, questionnaireName, serverParkName, null);

            return GetNumberOfRecords(records);
        }

        /// <inheritdoc/>
        public int GetNumberOfRecords(ConnectionModel connectionModel, string databaseFile)
        {
            var records = GetDataSet(connectionModel, databaseFile, null);

            return GetNumberOfRecords(records);
        }

        /// <inheritdoc/>
        public void LockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName,
            string lockId)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Lock(primaryKey, lockId);
        }

        /// <inheritdoc/>
        public void UnLockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName,
            string lockId)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

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
