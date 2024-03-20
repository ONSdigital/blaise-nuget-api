using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseModel
    {
        public CaseModel(Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData)
        {
            PrimaryKeyValues = primaryKeyValues;
            FieldData = fieldData;
        }

        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public Dictionary<string, string> FieldData { get; set; }
    }
}
