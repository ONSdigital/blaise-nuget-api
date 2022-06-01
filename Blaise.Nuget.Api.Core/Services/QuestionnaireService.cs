using System;
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
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IServerParkService _parkService;

        public QuestionnaireService(IServerParkService parkService)
        {
            _parkService = parkService;
        }

        public IEnumerable<string> GetQuestionnaireNames(ConnectionModel connectionModel, string serverParkName)
        {
            var questionnaires = GetQuestionnaires(connectionModel, serverParkName);

            return questionnaires.Select(sp => sp.Name);
        }

        public bool QuestionnaireExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaireNames = GetQuestionnaireNames(connectionModel, serverParkName);

            return questionnaireNames.Any(sp => sp.Equals(questionnaireName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<ISurvey> GetQuestionnaires(ConnectionModel connectionModel, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            return serverPark.Surveys;
        }

        public ISurvey GetQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaires = GetQuestionnaires(connectionModel, serverParkName);

            return GetQuestionnaire(questionnaires, questionnaireName);
        }

        public DateTime GetInstallDate(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);

            return questionnaire.InstallDate;
        }

        public QuestionnaireStatusType GetQuestionnaireStatus(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);

            return questionnaire.Status.ToEnum<QuestionnaireStatusType>();
        }

        public QuestionnaireInterviewType GetQuestionnaireInterviewType(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var questionnaire = GetQuestionnaire(connectionModel, questionnaireName, serverParkName);
            var configuration = questionnaire.Configuration.Configurations.FirstOrDefault(c =>
                    string.Equals(c.InstrumentName, questionnaireName, StringComparison.CurrentCultureIgnoreCase));

            if (configuration == null)
            {
                throw new QuestionnaireConfigurationException($"No configuration found for the questionnaire name '{questionnaireName}'");
            }

            return configuration.InitialLayoutSetGroupName.ToEnum<QuestionnaireInterviewType>();
        }

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

        public void InstallQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string questionnaireFile, QuestionnaireInterviewType questionnaireInterviewType)
        {
            var serverPark = _parkService.GetServerPark(connectionModel, serverParkName);

            serverPark.InstallSurvey(questionnaireFile,
                questionnaireInterviewType.FullName(),
                QuestionnaireDataEntryType.StrictInterviewing.ToString(),
                DataOverwriteMode.Always);
        }

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
