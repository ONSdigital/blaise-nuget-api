using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISqlService
    {
        IEnumerable<string> GetCaseIds(string connectionString, string questionnaireName);

        IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string connectionString, string questionnaireName);

        string GetPostCode(string connectionString, string questionnaireName, string primaryKey);
    }
}