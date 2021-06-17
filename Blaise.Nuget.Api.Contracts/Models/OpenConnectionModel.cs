using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class OpenConnectionModel
    {
        public OpenConnectionModel()
        {
            ExpirationDateTimes = new List<DateTime>();
        }

        public string ConnectionType { get; set; }

        public int Connections { get; set; }

        public IEnumerable<DateTime> ExpirationDateTimes { get; set; }
    }
}
