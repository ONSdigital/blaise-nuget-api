using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSurveyApi
    {
        bool SurveyExists(string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetSurveysAcrossServerParks();

        IEnumerable<ISurvey> GetSurveys(string serverParkName);

        ISurvey GetSurvey(string instrumentName, string serverParkName);

        SurveyStatusType GetSurveyStatus(string instrumentName, string serverParkName);

        IEnumerable<string> GetNamesOfSurveys(string serverParkName);

        SurveyInterviewType GetSurveyInterviewType(string instrumentName, string serverParkName);

        Guid GetIdOfSurvey(string instrumentName, string serverParkName);

        void InstallSurvey(string instrumentName, string serverParkName, string instrumentFile, SurveyInterviewType surveyInterviewType);

        void UninstallSurvey(string instrumentName, string serverParkName);

        void ActivateSurvey(string instrumentName, string serverParkName);

        void DeactivateSurvey(string instrumentName, string serverParkName);

        IEnumerable<string> GetSurveyModes(string instrumentName, string serverParkName);
    }
}