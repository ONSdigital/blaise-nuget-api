using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseSqlApi : IBlaiseSqlApi
    {
        private readonly ISqlService _mySqlService;

        private readonly IBlaiseConfigurationProvider _configurationProvider;

        internal BlaiseSqlApi(ISqlService mySqlService,
            IBlaiseConfigurationProvider configurationProvider)
        {
            _mySqlService = mySqlService;
            _configurationProvider = configurationProvider;
        }

        public BlaiseSqlApi()
        {
            _mySqlService = UnityProvider.Resolve<ISqlService>();
            _configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
        }
        
        public IEnumerable<string> GetCaseIds(string instrumentName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            return _mySqlService.GetCaseIds(_configurationProvider.DatabaseConnectionString, instrumentName);
        }

        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string instrumentName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            return _mySqlService.GetCaseIdentifiers(_configurationProvider.DatabaseConnectionString, instrumentName);
        }

        public string GetPostCode(string instrumentName, string primaryKey)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            primaryKey.ThrowExceptionIfNullOrEmpty("primaryKey");

            return _mySqlService.GetPostCode(_configurationProvider.DatabaseConnectionString, instrumentName, primaryKey);
        }
    }
}
