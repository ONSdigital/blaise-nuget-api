using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IServerParkService _parkService;

        public QuestionnaireService(IServerParkService parkService)
        {
            _parkService = parkService;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetQuestionnaireNames(ConnectionModel connectionModel, string serverParkName)
        {
            var questionnaires = GetQuestionnaires(connectionModel, serverParkName);

            return questionnaires.Select(sp => sp.Name);
        }

        /// <inheritdoc/>
        public bool QuestionnaireExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaireNames = GetQuestionnaireNames(connectionModel, serverParkName);

            return questionnaireNames.Any(sp => sp.Equals(questionnaireName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <inheritdoc/>
        public IEnumerable<ISurvey> GetQuestionnaires(ConnectionModel connectionModel, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            return serverPark.Surveys;
        }

        /// <inheritdoc/>
        public ISurvey GetQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaires = GetQuestionnaires(connectionModel, serverParkName);

            return GetQuestionnaire(questionnaires, questionnaireName);
        }

        /// <inheritdoc/>
        public DateTime GetInstallDate(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);

            return questionnaire.InstallDate;
        }

        /// <inheritdoc/>
        public QuestionnaireStatusType GetQuestionnaireStatus(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);

            return questionnaire.Status.ToEnum<QuestionnaireStatusType>();
        }

        /// <inheritdoc/>
        public QuestionnaireConfigurationModel GetQuestionnaireConfigurationModel(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaireConfiguration = GetQuestionnaireConfiguration(connectionModel, questionnaireName, serverParkName);

            return new QuestionnaireConfigurationModel
            {
                QuestionnaireDataEntryType = questionnaireConfiguration.InitialDataEntrySettingsName.ToEnum<QuestionnaireDataEntryType>(),
                QuestionnaireInterviewType = questionnaireConfiguration.InitialLayoutSetGroupName.ToEnum<QuestionnaireInterviewType>()
            };
        }

        /// <inheritdoc/>
        public IEnumerable<ISurvey> GetAllQuestionnaires(ConnectionModel connectionModel)
        {
            var questionnaireList = new List<ISurvey>();
            var serverParkNames = _parkService.GetServerParkNames(connectionModel);

            foreach (var serverParkName in serverParkNames)
            {
                var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);
                questionnaireList.AddRange(serverPark.Surveys);
            }

            return questionnaireList;
        }

        /// <inheritdoc/>
        public Guid GetQuestionnaireId(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            return GetQuestionnaireId(questionnaireName, serverPark);
        }

        public string GetMetaFileName(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var configuration = GetQuestionnaireConfiguration(connectionModel, questionnaireName, serverParkName);

            return configuration.MetaFileName;
        }

        /// <inheritdoc/>
        public void InstallQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string questionnaireFile, IInstallOptions installOptions)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName) as IServerPark6;

            if (serverPark is null)
            {
                throw new Exception("Could not cast to IServerPark6");
            }

            serverPark.InstallSurvey(questionnaireFile, installOptions);
        }

        /// <inheritdoc/>
        public void UninstallQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);
            var questionnaireId = GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);

            serverPark.RemoveSurvey(questionnaireId);
        }

        private static Guid GetQuestionnaireId(string questionnaireName, IServerPark serverPark)
        {
            var questionnaire = serverPark.Surveys.FirstOrDefault(s => string.Equals(s.Name, questionnaireName, StringComparison.OrdinalIgnoreCase));

            if (questionnaire == null)
            {
                throw new DataNotFoundException($"Questionnaire '{questionnaireName}' not found on server park '{serverPark.Name}'");
            }

            return questionnaire.InstrumentID;
        }

        private static ISurvey GetQuestionnaire(IEnumerable<ISurvey> questionnaires, string questionnaireName)
        {
            var questionnaire = questionnaires.FirstOrDefault(s => string.Equals(s.Name, questionnaireName, StringComparison.OrdinalIgnoreCase));

            if (questionnaire == null)
            {
                throw new DataNotFoundException($"No questionnaire found for questionnaire name '{questionnaireName}'");
            }

            return questionnaire;
        }

        private IConfiguration GetQuestionnaireConfiguration(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);

            return GetQuestionnaireConfiguration(questionnaire);
        }

        private static IConfiguration GetQuestionnaireConfiguration(ISurvey questionnaire)
        {
            var configuration = questionnaire.Configuration.Configurations.FirstOrDefault();

            if (configuration == null)
            {
                throw new NullReferenceException($"There was no configuration found for questionnaire '{questionnaire.Name}'");
            }

            return configuration;
        }
    }
}
