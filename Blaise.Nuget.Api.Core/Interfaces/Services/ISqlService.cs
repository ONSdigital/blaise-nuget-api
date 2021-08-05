using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISqlService
    {
        IEnumerable<string> GetCaseIds(string connectionString, string instrumentName);

        IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string connectionString, string instrumentName);

        string GetPostCode(string connectionString, string instrumentName, string primaryKey);
    }
}