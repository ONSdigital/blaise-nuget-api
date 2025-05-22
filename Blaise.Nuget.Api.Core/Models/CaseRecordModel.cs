using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Models
{
    public class CaseRecordModel
    {
        public Dictionary<string, string> primaryKeyValues { get; set; }
        public Dictionary<string, string> fieldData { get; set; }
    }
}
