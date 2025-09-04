namespace Blaise.Nuget.Api.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;

    public class KeyService : IKeyService
    {
        private const string PrimaryKeyName = "PRIMARY";

        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;

        public KeyService(IRemoteDataLinkProvider remoteDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
        }

        /// <inheritdoc/>
        public bool KeyExists(
            ConnectionModel connectionModel,
            IKey key,
            string questionnaireName,
            string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            return dataLink.KeyExists(key);
        }

        /// <inheritdoc/>
        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return DataRecordManager.GetKey(dataModel, keyName);
        }

        /// <inheritdoc/>
        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            return DataRecordManager.GetKey(dataModel, PrimaryKeyName);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
