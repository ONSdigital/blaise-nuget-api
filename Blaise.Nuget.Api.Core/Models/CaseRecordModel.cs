using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Models
{
    public class CaseRecordModel
    {
        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public Dictionary<string, string> FieldData { get; set; }
    }
}
