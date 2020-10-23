﻿using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel);

        bool ServerParkExists(ConnectionModel connectionModel, string serverParkName);

        IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName);

        IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel);
    }
}
