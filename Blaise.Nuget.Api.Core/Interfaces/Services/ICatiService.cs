using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICatiService
    {
        IEnumerable<ISurvey> GetInstalledSurveys(ConnectionModel connectionModel, string serverParkName);
        
        ISurvey GetInstalledSurvey(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        DayBatchModel CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate);

        DayBatchModel GetDayBatch(ConnectionModel connectionModel, string instrumentName,
            string serverParkName);

        void AddToDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            string primaryKeyValue);

        List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        void SetSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay);
        void SetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName, List<DateTime> surveyDays);

        void RemoveSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay);

        void RemoveSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            List<DateTime> surveyDays);
    }
}