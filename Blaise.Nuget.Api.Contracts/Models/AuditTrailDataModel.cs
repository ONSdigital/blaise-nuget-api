using System;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class AuditTrailDataModel
    {
        public string KeyValue { get; set; }

        public Guid SessionId { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
