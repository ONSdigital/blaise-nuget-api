using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseStatusModel
    {
        public CaseStatusModel(Dictionary<string, string> primaryKeyValues, int outcome, string lastUpdated)
        {
            PrimaryKeyValues = primaryKeyValues;
            Outcome = outcome;
            LastUpdated = lastUpdated;
        }


        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public int Outcome { get; set; }

        public string LastUpdated { get; set; }
    }
}
