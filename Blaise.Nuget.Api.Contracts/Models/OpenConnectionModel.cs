using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class OpenConnectionModel
    {
        public OpenConnectionModel()
        {
            Connections = new Dictionary<string, DateTime>();
        }

        public string ConnectionType { get; set; }

        public int NumberOfConnections { get; set; }

        public Dictionary<string, DateTime> Connections { get; set; }
    }
}
