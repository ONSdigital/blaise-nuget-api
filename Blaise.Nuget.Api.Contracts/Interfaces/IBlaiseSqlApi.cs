namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    using Blaise.Nuget.Api.Contracts.Models;
    using System.Collections.Generic;

    public interface IBlaiseSqlApi
    {
        IEnumerable<string> GetCaseIds(string questionnaireName);

        IEnumerable<string> GetEditingCaseIds(string questionnaireName);

        IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string questionnaireName);

        string GetPostCode(string questionnaireName, string primaryKey);

        bool DropQuestionnaireTables(string questionnaireName);
    }
}
