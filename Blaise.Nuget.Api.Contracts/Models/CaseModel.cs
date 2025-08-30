namespace Blaise.Nuget.Api.Contracts.Models
{
    using System;
    using System.Collections.Generic;

    public class CaseModel
    {
        public CaseModel()
        {
            PrimaryKeyValues = new Dictionary<string, string>();
        }

        public CaseModel(Dictionary<string, string> primaryKeyValues, Dictionary<string, string> fieldData)
        {
            PrimaryKeyValues = primaryKeyValues;
            FieldData = fieldData;
        }

        public Dictionary<string, string> PrimaryKeyValues { get; set; }

        public Dictionary<string, string> FieldData { get; set; }

        public string PrimaryKey => GetPrimaryKeyValue("QID.Serial_Number");// specifically to support minimal changes for Nisra ingest

        public string GetPrimaryKeyValue(string primaryKeyName)
        {
            if (PrimaryKeyValues == null || PrimaryKeyValues.Count == 0)
            {
                throw new ArgumentOutOfRangeException("primaryKeyName", "There are no primary keys defined");
            }

            if (!PrimaryKeyValues.ContainsKey(primaryKeyName))
            {
                throw new ArgumentException(
                    $"The primary key name '{primaryKeyName}' does not exist in the primaryKey object");
            }

            return PrimaryKeyValues[primaryKeyName];
        }
    }
}
