using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCatiApi
    {
        IEnumerable<ISurvey> GetInstalledQuestionnaires(string serverParkName);
        ISurvey GetInstalledQuestionnaire(string questionnaireName, string serverParkName);
        DayBatchModel CreateDayBatch(string questionnaireName, string serverParkName, 
            DateTime dayBatchDate, bool checkForTreatedCases);
        DayBatchModel GetDayBatch(string questionnaireName, string serverParkName);
        void AddToDayBatch(string questionnaireName, string serverParkName,
            string primaryKeyValue);
        List<DateTime> GetSurveyDays(string questionnaireName, string serverParkName);
        void SetSurveyDay(string questionnaireName, string serverParkName, DateTime surveyDay);
        void SetSurveyDays(string questionnaireName, string serverParkName, List<DateTime> surveyDays);
        void RemoveSurveyDay(string questionnaireName, string serverParkName, DateTime surveyDay);
        void RemoveSurveyDays(string questionnaireName, string serverParkName, List<DateTime> surveyDays);
        bool MakeSuperAppointment(string questionnaireName, string serverParkName, string primaryKeyValue);
    }
}