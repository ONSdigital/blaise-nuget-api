﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IParkService _parkService;

        public SurveyService(
            IParkService parkService 
            )
        {
            _parkService = parkService;
        }

        public IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName)
        {
            var surveys = GetSurveys(connectionModel, serverParkName);

            return surveys.Select(sp => sp.Name);
        }

        public bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyNames = GetSurveyNames(connectionModel, serverParkName);

            return surveyNames.Any(sp => sp.Equals(instrumentName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            if (!serverPark.Surveys.Any())
            {
                throw new DataNotFoundException($"No surveys found for server park '{serverParkName}'");
            }

            return serverPark.Surveys;
        }

        public ISurvey GetSurvey(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveys = GetSurveys(connectionModel, serverParkName);
            var survey = surveys.FirstOrDefault(s => s.Name == instrumentName);

            if (survey == null)
            {
                throw new DataNotFoundException($"No survey found for instrument name '{instrumentName}'");
            }

            return survey;
        }

        public SurveyStatusType GetSurveyStatus(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var survey = GetSurvey(connectionModel, instrumentName, serverParkName);

            return survey.Status.ToEnum<SurveyStatusType>();
        }

        public SurveyInterviewType GetSurveyInterviewType(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var survey = GetSurvey(connectionModel, instrumentName, serverParkName);
            var configuration = survey.Configuration.Configurations.FirstOrDefault(c => c.InstrumentName == instrumentName);

            if (configuration == null)
            {
                throw new SurveyConfigurationException($"No configuration found for the instrument name '{instrumentName}'");
            }

            return configuration.InitialLayoutSetGroupName.ToEnum<SurveyInterviewType>();
        }

        public IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel)
        {
            var surveyList = new List<ISurvey>();
            var serverParkNames = _parkService.GetServerParkNames(connectionModel);

            foreach (var serverParkName in serverParkNames)
            {
                var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);
                surveyList.AddRange(serverPark.Surveys);
            }

            return surveyList;
        }

        public Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);
            var survey = serverPark.Surveys.FirstOrDefault(s => string.Equals(s.Name, instrumentName, StringComparison.OrdinalIgnoreCase));

            if (survey == null)
            {
                throw new DataNotFoundException($"Instrument '{instrumentName}' not found on server park '{serverParkName}'");
            }

            return survey.InstrumentID;
        }

        public string GetMetaFileName(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var survey = GetSurvey(connectionModel, instrumentName, serverParkName);
            var configuration = GetSurveyConfiguration(survey);

            return configuration.MetaFileName;
        }

        public string GetDataFileName(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var survey = GetSurvey(connectionModel, instrumentName, serverParkName);
            var configuration = GetSurveyConfiguration(survey);

            return configuration.DataFileName;
        }

        public void InstallInstrument(ConnectionModel connectionModel, string instrumentFile,
            SurveyInterviewType surveyInterviewType, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            serverPark.InstallSurvey(instrumentFile, surveyInterviewType.FullName(), 
                                        SurveyDataEntryType.StrictInterviewing.ToString(), DataOverwriteMode.Always);
        }

        public void UninstallInstrument(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);
            var instrumentId = GetInstrumentId(connectionModel, instrumentName, serverParkName);

            serverPark.RemoveSurvey(instrumentId);
        }

        private static IConfiguration GetSurveyConfiguration(ISurvey survey)
        {
            var configuration = survey.Configuration.Configurations.FirstOrDefault();

            if (configuration == null)
            {
                throw new NullReferenceException($"There was no configuration found for survey '{survey.Name}'");
            }

            return configuration;
        }
    }
}
