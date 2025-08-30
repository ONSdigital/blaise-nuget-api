namespace Blaise.Nuget.Api.Core.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.Meta;
    using System;

    public class DataModelService : IDataModelService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;

        private readonly ILocalDataLinkProvider _localDataLinkProvider;

        public DataModelService(
            IRemoteDataLinkProvider remoteDataLinkProvider,
            ILocalDataLinkProvider localDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
            _localDataLinkProvider = localDataLinkProvider;
        }

        public IDatamodel GetDataModel(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, questionnaireName, serverParkName);

            if (dataLink?.Datamodel == null)
            {
                throw new NullReferenceException($"No datamodel was found for questionnaire '{questionnaireName}' on server park '{serverParkName}'");
            }

            return dataLink.Datamodel;
        }

        public IDatamodel GetDataModel(ConnectionModel connectionModel, string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(connectionModel, databaseFile);

            if (dataLink?.Datamodel == null)
            {
                throw new NullReferenceException($"No datamodel was found for file '{databaseFile}'");
            }

            return dataLink.Datamodel;
        }
    }
}
