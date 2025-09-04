namespace Blaise.Nuget.Api.Core.Models
{
    using System.Collections.Generic;

    public class CaseRecordModel
    {
        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public Dictionary<string, string> FieldData { get; set; }
    }
}
