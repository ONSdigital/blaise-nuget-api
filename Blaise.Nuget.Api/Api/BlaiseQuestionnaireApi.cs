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
    public class BlaiseQuestionnaireApi : IBlaiseQuestionnaireApi
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IQuestionnaireMetaService _questionnaireMetaService;
        private readonly ICaseService _caseService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseQuestionnaireApi(
            IQuestionnaireService questionnaireService,
            IQuestionnaireMetaService questionnaireMetaService,
            ICaseService caseService,
            ConnectionModel connectionModel)
        {
            _questionnaireService = questionnaireService;
            _questionnaireMetaService = questionnaireMetaService;
            _caseService = caseService;
            _connectionModel = connectionModel;
        }

        public BlaiseQuestionnaireApi(ConnectionModel connectionModel = null)
        {
            _questionnaireService = UnityProvider.Resolve<IQuestionnaireService>();
            _questionnaireMetaService = UnityProvider.Resolve<IQuestionnaireMetaService>();
            _caseService = UnityProvider.Resolve<ICaseService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool QuestionnaireExists(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.QuestionnaireExists(_connectionModel, questionnaireName, serverParkName);
        }

        public IEnumerable<ISurvey> GetQuestionnairesAcrossServerParks()
        {
            return _questionnaireService.GetAllQuestionnaires(_connectionModel);
        }

        public IEnumerable<ISurvey> GetQuestionnaires(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.GetQuestionnaires(_connectionModel, serverParkName);
        }

        public ISurvey GetQuestionnaire(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.GetQuestionnaire(_connectionModel, questionnaireName, serverParkName);
        }

        public QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.GetQuestionnaireStatus(_connectionModel, questionnaireName, serverParkName);
        }

        public QuestionnaireInterviewType GetQuestionnaireInterviewType(string questionnaireName, string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");

            return _questionnaireService.GetQuestionnaireInterviewType(_connectionModel, questionnaireName, serverParkName);
        }

        public IEnumerable<string> GetNamesOfQuestionnaires(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.GetQuestionnaireNames(_connectionModel, serverParkName);
        }

        public Guid GetIdOfQuestionnaire(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireService.GetQuestionnaireId(_connectionModel, questionnaireName, serverParkName);
        }

        public void InstallQuestionnaire(string questionnaireName, string serverParkName,
            string questionnaireFile, IInstallOptions installOptions)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");
            installOptions.ThrowExceptionIfNull("installOptions");

            _questionnaireService.InstallQuestionnaire(_connectionModel, questionnaireName, serverParkName,
                questionnaireFile, installOptions);
        }

        public void UninstallQuestionnaire(string questionnaireName, string serverParkName, bool deleteCases = false)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            if (deleteCases)
            {
                _caseService.RemoveDataRecords(_connectionModel, questionnaireName, serverParkName);
            }

            _questionnaireService.UninstallQuestionnaire(_connectionModel, questionnaireName, serverParkName);
        }

        public void ActivateQuestionnaire(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var questionnaire = GetQuestionnaire(questionnaireName, serverParkName);

            questionnaire.Activate();
        }

        public void DeactivateQuestionnaire(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var questionnaire = GetQuestionnaire(questionnaireName, serverParkName);

            questionnaire.Deactivate();
        }

        public IEnumerable<string> GetQuestionnaireModes(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireMetaService.GetQuestionnaireModes(_connectionModel, questionnaireName, serverParkName);
        }

        public IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(string questionnaireName, string serverParkName)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _questionnaireMetaService.GetQuestionnaireDataEntrySettings(_connectionModel, questionnaireName, serverParkName);
        }
    }
}
