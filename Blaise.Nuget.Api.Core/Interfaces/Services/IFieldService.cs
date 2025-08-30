namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.DataRecord;

    public interface IFieldService
    {
        bool FieldExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string fieldName);

        bool FieldExists(IDataRecord dataRecord, string fieldName);

        IField GetField(IDataRecord dataRecord, string fieldName);
    }
}
