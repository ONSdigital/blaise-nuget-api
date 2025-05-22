using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSqlApi
    {
        IEnumerable<string> GetCaseIds(string questionnaireName);

        IEnumerable<string> GetEditingCaseIds(string questionnaireName);

        IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string questionnaireName);

        string GetPostCode(string questionnaireName, string primaryKey);

        bool DropQuestionnaireTables(string questionnaireName);
    }
}
