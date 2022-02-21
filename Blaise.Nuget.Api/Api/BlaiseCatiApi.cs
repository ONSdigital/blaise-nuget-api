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

        internal BlaiseCatiApi(
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
        
        public IEnumerable<ISurvey> GetInstalledSurveys(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetInstalledSurveys(_connectionModel, serverParkName);
        }

        public ISurvey GetInstalledSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetInstalledSurvey(_connectionModel, instrumentName, serverParkName);
        }

        public DayBatchModel CreateDayBatch(string instrumentName, string serverParkName, 
            DateTime dayBatchDate, bool checkForTreatedCases)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            if (_caseService.GetNumberOfCases(_connectionModel, instrumentName, serverParkName) == 0)
            {
                throw new DataNotFoundException($"There are no cases available in '{instrumentName}' to create a daybatch");
            }

            return _catiService.CreateDayBatch(_connectionModel, instrumentName, serverParkName, dayBatchDate, checkForTreatedCases);
        }

        public DayBatchModel GetDayBatch(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetDayBatch(_connectionModel, instrumentName, serverParkName);
        }

        public void AddToDayBatch(string instrumentName, string serverParkName, string primaryKeyValue)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");

            _catiService.AddToDayBatch(_connectionModel, instrumentName, serverParkName, primaryKeyValue);
        }

        public List<DateTime> GetSurveyDays(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _catiService.GetSurveyDays(_connectionModel, instrumentName, serverParkName);
        }

        public void SetSurveyDay(string instrumentName, string serverParkName, DateTime surveyDay)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _catiService.SetSurveyDay(_connectionModel, instrumentName, serverParkName, surveyDay);
        }

        public void SetSurveyDays(string instrumentName, string serverParkName, List<DateTime> surveyDays)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            surveyDays.ThrowExceptionIfNullOrEmpty("surveyDays");

            _catiService.SetSurveyDays(_connectionModel, instrumentName, serverParkName, surveyDays);
        }

        public void RemoveSurveyDay(string instrumentName, string serverParkName, DateTime surveyDay)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _catiService.RemoveSurveyDay(_connectionModel, instrumentName, serverParkName, surveyDay);
        }

        public void RemoveSurveyDays(string instrumentName, string serverParkName, List<DateTime> surveyDays)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            surveyDays.ThrowExceptionIfNullOrEmpty("surveyDays");

            _catiService.RemoveSurveyDays(_connectionModel, instrumentName, serverParkName, surveyDays);
        }

        public bool MakeSuperAppointment(string instrumentName, string serverParkName, string primaryKeyValue)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");

            return _catiService.MakeSuperAppointment(_connectionModel, instrumentName, serverParkName, primaryKeyValue);
        }
    }
}
