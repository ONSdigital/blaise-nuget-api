namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Models;
    using StatNeth.Blaise.API.ServerManager;

    public interface IQuestionnaireService
    {
        IEnumerable<string> GetQuestionnaireNames(ConnectionModel connectionModel, string serverParkName);

        bool QuestionnaireExists(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IEnumerable<ISurvey> GetQuestionnaires(ConnectionModel connectionModel, string serverParkName);

        ISurvey GetQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        DateTime GetInstallDate(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        QuestionnaireStatusType GetQuestionnaireStatus(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName);

        IEnumerable<ISurvey> GetAllQuestionnaires(ConnectionModel connectionModel);

        Guid GetQuestionnaireId(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        void InstallQuestionnaire(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName,
            string questionnaireFile,
            IInstallOptions installOptions);

        void UninstallQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        QuestionnaireConfigurationModel GetQuestionnaireConfigurationModel(
            ConnectionModel connectionModel,
            string questionnaireName,
            string serverParkName);
    }
}
