using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICatiService
    {
        IEnumerable<ISurvey> GetInstalledQuestionnaires(ConnectionModel connectionModel, string serverParkName);
        
        ISurvey GetInstalledQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        DayBatchModel CreateDayBatch(ConnectionModel connectionModel, string questionnaireName, string serverParkName, 
            DateTime dayBatchDate, bool checkForTreatedCases);

        DayBatchModel GetDayBatch(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName);

        void AddToDayBatch(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string primaryKeyValue);

        List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        void SetSurveyDay(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            DateTime surveyDay);
        void SetSurveyDays(ConnectionModel connectionModel, string questionnaireName, string serverParkName, List<DateTime> surveyDays);

        void RemoveSurveyDay(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            DateTime surveyDay);

        void RemoveSurveyDays(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            List<DateTime> surveyDays);

        bool MakeSuperAppointment(ConnectionModel connectionModel, string questionnaireName, string serverParkName, string primaryKeyValue);
    }
}