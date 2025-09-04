namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.Meta;

    public interface IDataModelService
    {
        IDatamodel GetDataModel(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IDatamodel GetDataModel(ConnectionModel connectionModel, string databaseFile);
    }
}
