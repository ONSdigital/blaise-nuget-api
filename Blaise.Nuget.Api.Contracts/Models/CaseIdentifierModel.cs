namespace Blaise.Nuget.Api.Contracts.Models
{
    public class CaseIdentifierModel
    {
        public CaseIdentifierModel(string primaryKey, string postCode)
        {
            PrimaryKey = primaryKey;
            PostCode = postCode;
        }

        public string PrimaryKey { get; set; }

        public string PostCode { get; set; }
    }
}
