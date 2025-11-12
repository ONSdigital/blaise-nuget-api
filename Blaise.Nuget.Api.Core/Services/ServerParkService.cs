namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Exceptions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;

    public class ServerParkService : IServerParkService
    {
        private readonly IConnectedServerFactory _connectionFactory;

        public ServerParkService(IConnectedServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            var serverParks = GetServerParks(connectionModel);

            return serverParks.Select(sp => sp.Name);
        }

        public bool ServerParkExists(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParkNames = GetServerParkNames(connectionModel);

            return serverParkNames.Any(sp => sp.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParks = GetServerParks(connectionModel);
            var serverPark = serverParks.FirstOrDefault(sp => sp.Name.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));

            if (serverPark == null)
            {
                throw new DataNotFoundException($"Server park '{serverParkName}' not found");
            }

            return serverPark;
        }

        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);

            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks;
        }
    }
}
