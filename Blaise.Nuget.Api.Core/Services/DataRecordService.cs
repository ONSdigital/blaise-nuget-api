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

        public IDataSet GetDataSet(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

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

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string databaseFile, IKey primaryKey)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            return dataLink.ReadRecord(primaryKey);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            return dataLink.ReadRecord(key);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Write(dataRecord);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            dataLink.Write(dataRecord);
        }

        public void DeleteDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Delete(primaryKey);
        }

        public void DeleteDataRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.DeleteAll();
        }

        public int GetNumberOfRecords(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var records = GetDataSet(connectionModel, questionnaireName, serverParkName);

            return GetNumberOfRecords(records);
        }

        public int GetNumberOfRecords(ConnectionModel connectionModel, string databaseFile)
        {
            var records = GetDataSet(connectionModel, databaseFile);

            return GetNumberOfRecords(records);
        }

        public void LockDataRecord(ConnectionModel connectionModel, IKey primaryKey, string questionnaireName, string serverParkName, 
            string lockId)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            dataLink.Lock(primaryKey, lockId);
        }

        public void UnLockDataRecord(ConnectionModel connectionModel,IKey primaryKey, string questionnaireName, string serverParkName, 
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
