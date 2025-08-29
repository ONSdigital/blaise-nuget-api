using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseCatiApi : IBlaiseCatiApi
    {
        private readonly ICatiService _catiService;
        private readonly ICaseService _caseService;
        private readonly ConnectionModel _connectionModel;

        public BlaiseCatiApi(
            ICatiService catiService,
            ICaseService caseService,
            ConnectionModel connectionModel)
        {
            _catiService = catiService;
            _caseService = caseService;
            _connectionModel = connectionModel;
        }

        public BlaiseCatiApi(ConnectionModel connectionModel = null)
        {
            _catiService = UnityProvider.Resolve<ICatiService>();
            _caseService = UnityProvider.Resolve<ICaseService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        /// <inheritdoc/>
        public IEnumerable<ISurvey> GetInstalledQuestionnaires(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetInstalledQuestionnaires(_connectionModel, serverParkName);
        }

        /// <inheritdoc/>
        public ISurvey GetInstalledQuestionnaire(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetInstalledQuestionnaire(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public DayBatchModel CreateDayBatch(string questionnaireName, string serverParkName,
            DateTime dayBatchDate, bool checkForTreatedCases)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            if (_caseService.GetNumberOfCases(_connectionModel, questionnaireName, serverParkName) == 0)
            {
                throw new DataNotFoundException($"There are no cases available in '{questionnaireName}' to create a daybatch");
            }

            return _catiService.CreateDayBatch(_connectionModel, questionnaireName, serverParkName, dayBatchDate, checkForTreatedCases);
        }

        /// <inheritdoc/>
        public DayBatchModel GetDayBatch(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetDayBatch(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void AddToDayBatch(string questionnaireName, string serverParkName, string primaryKeyValue)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");

            _catiService.AddToDayBatch(_connectionModel, questionnaireName, serverParkName, primaryKeyValue);
        }

        /// <inheritdoc/>
        public List<DateTime> GetSurveyDays(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetSurveyDays(_connectionModel, questionnaireName, serverParkName);
        }

        /// <inheritdoc/>
        public void SetSurveyDay(string questionnaireName, string serverParkName, DateTime surveyDay)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _catiService.SetSurveyDay(_connectionModel, questionnaireName, serverParkName, surveyDay);
        }

        /// <inheritdoc/>
        public void SetSurveyDays(string questionnaireName, string serverParkName, List<DateTime> surveyDays)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            surveyDays.ThrowExceptionIfNullOrEmpty("surveyDays");

            _catiService.SetSurveyDays(_connectionModel, questionnaireName, serverParkName, surveyDays);
        }

        /// <inheritdoc/>
        public void RemoveSurveyDay(string questionnaireName, string serverParkName, DateTime surveyDay)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _catiService.RemoveSurveyDay(_connectionModel, questionnaireName, serverParkName, surveyDay);
        }

        /// <inheritdoc/>
        public void RemoveSurveyDays(string questionnaireName, string serverParkName, List<DateTime> surveyDays)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            surveyDays.ThrowExceptionIfNullOrEmpty("surveyDays");

            _catiService.RemoveSurveyDays(_connectionModel, questionnaireName, serverParkName, surveyDays);
        }

        /// <inheritdoc/>
        public bool MakeSuperAppointment(string questionnaireName, string serverParkName, string primaryKeyValue)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");

            return _catiService.MakeSuperAppointment(_connectionModel, questionnaireName, serverParkName, primaryKeyValue);
        }
    }
}
