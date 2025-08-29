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

        public BlaiseSqlApi(
            ISqlService mySqlService,
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

        /// <inheritdoc/>
        public IEnumerable<string> GetCaseIds(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetCaseIds(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetEditingCaseIds(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetEditingCaseIds(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        /// <inheritdoc/>
        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.GetCaseIdentifiers(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }

        /// <inheritdoc/>
        public string GetPostCode(string questionnaireName, string primaryKey)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            primaryKey.ThrowExceptionIfNullOrEmpty("primaryKey");

            return _mySqlService.GetPostCode(_configurationProvider.DatabaseConnectionString, questionnaireName, primaryKey);
        }

        /// <inheritdoc/>
        public bool DropQuestionnaireTables(string questionnaireName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _mySqlService.DropQuestionnaireTables(_configurationProvider.DatabaseConnectionString, questionnaireName);
        }
    }
}
