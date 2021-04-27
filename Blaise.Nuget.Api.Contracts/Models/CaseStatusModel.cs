namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseStatusModel
    {
        public CaseStatusModel(string primaryKey, int outcome, string lastUpdated)
        {
            PrimaryKey = primaryKey;
            Outcome = outcome;
            LastUpdated = lastUpdated;
        }

        public string PrimaryKey { get; set; }

        public int Outcome { get; set; }

        public string LastUpdated { get; set; }
    }
}
