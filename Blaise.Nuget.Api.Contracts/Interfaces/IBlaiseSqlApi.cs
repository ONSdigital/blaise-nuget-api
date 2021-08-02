using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSqlApi
    {
        IEnumerable<string> GetCaseIds(string instrumentName);
    }
}