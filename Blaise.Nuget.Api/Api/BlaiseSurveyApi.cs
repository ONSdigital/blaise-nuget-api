using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseSurveyApi : IBlaiseSurveyApi
    {
        private readonly ISurveyService _surveyService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseSurveyApi(
            ISurveyService surveyService,
            ConnectionModel connectionModel)
        {
            _surveyService = surveyService;
            _connectionModel = connectionModel;
        }

        public BlaiseSurveyApi(ConnectionModel connectionModel = null)
        {
            _surveyService = UnityProvider.Resolve<ISurveyService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool SurveyExists(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.SurveyExists(_connectionModel, instrumentName, serverParkName);
        }

        public IEnumerable<ISurvey> GetSurveysAcrossServerParks()
        {
            return _surveyService.GetAllSurveys(_connectionModel);
        }

        public IEnumerable<ISurvey> GetSurveys(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveys(_connectionModel, serverParkName);
        }

        public ISurvey GetSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurvey(_connectionModel, instrumentName, serverParkName);
        }

        public SurveyStatusType GetSurveyStatus(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveyStatus(_connectionModel, instrumentName, serverParkName);
        }

        public SurveyInterviewType GetSurveyInterviewType(string instrumentName, string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            return _surveyService.GetSurveyInterviewType(_connectionModel, instrumentName, serverParkName);
        }

        public IEnumerable<string> GetNamesOfSurveys(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveyNames(_connectionModel, serverParkName);
        }

        public Guid GetIdOfSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetInstrumentId(_connectionModel, instrumentName, serverParkName);
        }

        public void InstallSurvey(string instrumentName, string serverParkName,
            string instrumentFile, SurveyInterviewType surveyInterviewType)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _surveyService.InstallInstrument(_connectionModel, instrumentName, serverParkName,
                instrumentFile, surveyInterviewType);
        }

        public void UninstallSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _surveyService.UninstallInstrument(_connectionModel, instrumentName, serverParkName);
        }

        public void ActivateSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var survey = GetSurvey(instrumentName, serverParkName);

            survey.Activate();
        }

        public void DeactivateSurvey(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var survey = GetSurvey(instrumentName, serverParkName);

            survey.Deactivate();
        }
    }
}
