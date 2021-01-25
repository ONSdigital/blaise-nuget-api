﻿using System;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCatiApi
    {
        IEnumerable<ISurvey> GetInstalledSurveys(string serverParkName);
        ISurvey GetInstalledSurvey(string instrumentName, string serverParkName);
        void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate);
        List<DateTime> GetSurveyDays(string instrumentName, string serverParkName);
        void SetSurveyDay(string instrumentName, string serverParkName, DateTime surveyDay);
        void SetSurveyDays(string instrumentName, string serverParkName, List<DateTime> surveyDays);

        void RemoveSurveyDay(string instrumentName, string serverParkName,
            DateTime surveyDay);
        void RemoveSurveyDays(string instrumentName, string serverParkName,
            List<DateTime> surveyDays);

    }
}