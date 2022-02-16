using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseModel
    {
        public CaseModel(string primaryKey, Dictionary<string, string> fieldData)
        {
            CaseId = primaryKey;
            FieldData = fieldData;
        }

        public string CaseId { get; set; }

        public Dictionary<string, string> FieldData { get; set; }
    }
}
