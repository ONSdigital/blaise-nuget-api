using System.Collections;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IMySqlService
    {
        IEnumerable<string> GetCaseIds(string connectionString, string instrumentName);
    }
}