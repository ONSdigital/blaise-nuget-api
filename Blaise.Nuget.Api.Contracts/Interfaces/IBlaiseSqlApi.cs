using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSqlApi
    {
        IEnumerable<string> GetCaseIds(string instrumentName);

        IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string instrumentName);

        string GetPostCode(string instrumentName, string primaryKey);
    }
}