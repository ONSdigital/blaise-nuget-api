using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System.Collections.Generic;
using System.Linq;

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

        public Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord)
        {
            var primaryKeyValues = new Dictionary<string, string>();
            var primaryKey = dataRecord.Keys.First(k => k.Name == PrimaryKeyName);
            foreach (var item in primaryKey.Fields)
            {
                primaryKeyValues.Add(item.FullName, item.DataValue.ValueAsText.Trim());
            }

            return primaryKeyValues;
        }

        public void AssignPrimaryKeyValues(IKey key, Dictionary<string, string> primaryKeyValues)
        {
            foreach (var item in primaryKeyValues)
            {
                var keyField = key.Fields.GetItem(item.Key);
                keyField.DataValue.Assign(item.Value);
            }
        }
    }
}
