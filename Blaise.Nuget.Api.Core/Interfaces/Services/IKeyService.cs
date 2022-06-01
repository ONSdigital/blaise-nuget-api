using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IKeyService
    {
        bool KeyExists(ConnectionModel connectionModel, IKey key, string questionnaireName, string serverParkName);

        IKey GetKey(IDatamodel datamodel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);
    }
}