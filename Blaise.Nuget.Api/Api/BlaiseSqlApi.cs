using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseSqlApi 
    {
        private readonly IMySqlService _mySqlService;

        private readonly IBlaiseConfigurationProvider _configurationProvider;

        internal BlaiseSqlApi(IMySqlService mySqlService,
            IBlaiseConfigurationProvider configurationProvider)
        {
            _mySqlService = mySqlService;
            _configurationProvider = configurationProvider;
        }

        public BlaiseSqlApi()
        {
            _mySqlService = UnityProvider.Resolve<IMySqlService>();
            _configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
        }


        public IEnumerable<string> GetCaseIds(string instrumentName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            return _mySqlService.GetCaseIds(_configurationProvider.DatabaseConnectionString, instrumentName);
        }

    }
}
