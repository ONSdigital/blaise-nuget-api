namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.DataRecord;
    using StatNeth.Blaise.API.Meta;
    using System.Collections.Generic;

    public interface IKeyService
    {
        bool KeyExists(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName);

        IKey GetKey(IDatamodel datamodel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        Dictionary<string, string> GetPrimaryKeyValues(IDataRecord dataRecord);

        void AssignPrimaryKeyValues(IKey key, Dictionary<string, string> primaryKeyValues);
    }
}
