using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCatiApi
    {
        IEnumerable<ISurvey> GetInstalledSurveys(string serverParkName);
        ISurvey GetInstalledSurvey(string instrumentName, string serverParkName);
        DayBatchModel CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate);
        DayBatchModel GetDayBatch(string instrumentName, string serverParkName);
        void AddToDayBatch(string instrumentName, string serverParkName,
            string primaryKeyValue);
        List<DateTime> GetSurveyDays(string instrumentName, string serverParkName);
        void SetSurveyDay(string instrumentName, string serverParkName, DateTime surveyDay);
        void SetSurveyDays(string instrumentName, string serverParkName, List<DateTime> surveyDays);

        void RemoveSurveyDay(string instrumentName, string serverParkName,
            DateTime surveyDay);
        void RemoveSurveyDays(string instrumentName, string serverParkName,
            List<DateTime> surveyDays);

    }
}