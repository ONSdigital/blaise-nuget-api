using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseStatusModel
    {
        public CaseStatusModel()
        {
            PrimaryKeyValues = new Dictionary<string, string>();
        }

        public CaseStatusModel(Dictionary<string, string> primaryKeyValues, int outcome, string lastUpdated)
        {
            PrimaryKeyValues = primaryKeyValues;
            Outcome = outcome;
            LastUpdated = lastUpdated;
        }


        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public int Outcome { get; set; }

        public string LastUpdated { get; set; }

        public string GetPrimaryKeyValue(string primaryKeyName)
        {
            if (PrimaryKeyValues == null || PrimaryKeyValues.Count == 0)
            {
                throw new ArgumentOutOfRangeException("primaryKeyName","There are no primary keys defined");
            }

            return PrimaryKeyValues[primaryKeyName];
        }
    }
}
