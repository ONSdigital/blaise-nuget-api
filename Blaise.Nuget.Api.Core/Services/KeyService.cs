using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class KeyService : IKeyService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;
        private const string PrimaryKeyName = "PRIMARY";

        public KeyService(IRemoteDataLinkProvider remoteDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
        }

        public bool KeyExists(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            return dataLink.KeyExists(key);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return DataRecordManager.GetKey(dataModel, keyName);
        }

        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            return DataRecordManager.GetKey(dataModel, PrimaryKeyName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return dataRecord.Keys[0].KeyValue.Trim();
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKeyValue)
        {
            key.Fields[0].DataValue.Assign(primaryKeyValue);
        }
    }
}
