using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseQuestionnaireApi
    {
        bool QuestionnaireExists(string questionnaireName, string serverParkName);

        IEnumerable<ISurvey> GetQuestionnairesAcrossServerParks();

        IEnumerable<ISurvey> GetQuestionnaires(string serverParkName);

        ISurvey GetQuestionnaire(string questionnaireName, string serverParkName);

        QuestionnaireStatusType GetQuestionnaireStatus(string questionnaireName, string serverParkName);

        IEnumerable<string> GetNamesOfQuestionnaires(string serverParkName);

        QuestionnaireInterviewType GetQuestionnaireInterviewType(string questionnaireName, string serverParkName);

        Guid GetIdOfQuestionnaire(string questionnaireName, string serverParkName);

        void InstallQuestionnaire(string questionnaireName, string serverParkName, string questionnaireFile, IInstallOptions installOptions);

        void UninstallQuestionnaire(
            string questionnaireName,
            string serverParkName,
            bool deleteCases = false,
            bool clearCati = false,
            bool dropTables = false
           );

        void ActivateQuestionnaire(string questionnaireName, string serverParkName);

        void DeactivateQuestionnaire(string questionnaireName, string serverParkName);

        IEnumerable<string> GetQuestionnaireModes(string questionnaireName, string serverParkName);

        IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(string questionnaireName, string serverParkName);
    }
}