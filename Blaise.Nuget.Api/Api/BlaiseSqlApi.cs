using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using System.Collections.Generic;

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

        public IEnumerable<string> GetCaseIds(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetCaseIds(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        public IEnumerable<string> GetEditingCaseIds(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetEditingCaseIds(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetCaseIdentifiers(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        public string GetPostCode(string questionnaireName, string primaryKey)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            primaryKey.ThrowExceptionIfNullOrEmpty("primaryKey");

            return _mySqlService.GetPostCode(_configurationProvider.DatabaseConnectionString, questionnaireName, primaryKey);
        }

        public bool DropQuestionnaireTables(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.DropQuestionnaireTables(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }
    }
}
