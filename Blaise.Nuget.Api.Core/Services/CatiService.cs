#define USE_BLAISE_5_13_3

using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Cati.Runtime;
using StatNeth.Blaise.API.ServerManager;
// ReSharper disable PossibleInvalidOperationException

namespace Blaise.Nuget.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IRemoteCatiManagementServerProvider _remoteCatiManagementServerProvider;
        private readonly IQuestionnaireService _questionnaireService;

        public CatiService(
            IRemoteCatiManagementServerProvider remoteCatiManagementServerProvider,
            IQuestionnaireService questionnaireService)
        {
            _remoteCatiManagementServerProvider = remoteCatiManagementServerProvider;
            _questionnaireService = questionnaireService;
        }

        public IEnumerable<ISurvey> GetInstalledQuestionnaires(ConnectionModel connectionModel, string serverParkName)
        {
            var questionnaires = GetInstalledCatiQuestionnaires(connectionModel, serverParkName);
            var questionnaireNames = questionnaires.Keys;

            return questionnaireNames.Select(questionnaireName =>
                _questionnaireService.GetQuestionnaire(connectionModel, questionnaireName, serverParkName)).ToList();
        }

        public ISurvey GetInstalledQuestionnaire(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName)
        {
            var questionnaires = GetInstalledCatiQuestionnaires(connectionModel, serverParkName);

            if (!questionnaires.ContainsKey(questionnaireName))
            {
                throw new DataNotFoundException(
                    $"No questionnaire called '{questionnaireName}' was found on server park '{serverParkName}'");
            }

            return _questionnaireService.GetQuestionnaire(connectionModel, questionnaireName, serverParkName);
        }

        public DayBatchModel CreateDayBatch(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName,
            DateTime dayBatchDate, bool checkForTreatedCases)
        {
            var catiManagementServer =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiQuestionnaireManager =
                (ICatiInstrumentManager3)catiManagementServer.LoadCatiInstrumentManager(questionnaireId);

            if (catiQuestionnaireManager.Specification.SurveyDays.All(d => d.Date.Date != dayBatchDate.Date))
            {
                throw new DataNotFoundException(
                    $"A survey day does not exist for the required daybatch date '{dayBatchDate.Date}'");
            }

            catiQuestionnaireManager.CreateDaybatch(dayBatchDate, checkForTreatedCases);

            return GetDayBatch(catiManagementServer, questionnaireId);
        }

        public DayBatchModel GetDayBatch(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName)
        {
            var catiManagementServer =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);

            return GetDayBatch(catiManagementServer, questionnaireId);
        }

        public void AddToDayBatch(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            string primaryKeyValue)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);

            catiManagement.AddToDaybatch(questionnaireId, primaryKeyValue);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName)
        {
            var surveyDays = new List<DateTime>();
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(questionnaireId)?.Specification?.SurveyDays;

            if (surveyDateCollection == null || surveyDateCollection.Count == 0)
            {
                return surveyDays;
            }

            surveyDays.AddRange(surveyDateCollection.Select(surveyDay => surveyDay.Date));

            return surveyDays;
        }

        public void SetSurveyDay(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            DateTime surveyDay)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(questionnaireId);

            catiManager.Specification.SurveyDays
                .AddSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void SetSurveyDays(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            List<DateTime> surveyDays)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(questionnaireId);

            catiManager.Specification.SurveyDays
                .AddSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDay(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            DateTime surveyDay)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(questionnaireId);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDays(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            List<DateTime> surveyDays)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(questionnaireId);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        public bool MakeSuperAppointment(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName,
            string primaryKeyValue)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(questionnaireId);

            return catiManager.MakeSuperAppointment(primaryKeyValue);
        }

        private IDictionary<string, Guid> GetInstalledCatiQuestionnaires(ConnectionModel connectionModel,
            string serverParkName)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            return catiManagement.GetInstalledSurveys();
        }

        private static DayBatchModel GetDayBatch(IRemoteCatiManagementServer catiManagementServer, Guid questionnaireId)
        {
            var dayBatchCaseEntries = catiManagementServer.GetKeysInDaybatch(questionnaireId).ToList();

            if (!dayBatchCaseEntries.Any())
            {
                return null;
            }

            var dayBatchDate = catiManagementServer.GetDaybatchDate(questionnaireId);

            return new DayBatchModel(dayBatchDate, dayBatchCaseEntries);
        }

#if USE_BLAISE_5_13_3
        public int ClearCatiDataForQuestionnaire(ConnectionModel connectionModel, string questionnaireName,
            string serverParkName)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManagement5 = catiManagement as IRemoteCatiManagementServer5;

            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            return catiManagement5?.ClearInstrumentData(questionnaireId) ?? 0;
        }

        public int ClearAppointments(ConnectionModel connectionModel, string questionnaireName, string serverParkName,
            List<string> primaryKeys)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManagement6 = catiManagement as IRemoteCatiManagementServer6;

            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            return catiManagement6?.ClearAppointments(questionnaireId, primaryKeys) ?? 0;
        }

        public int CreateAppointment(ConnectionModel connectionModel, string questionnaireName
            , string serverParkName, string primaryKey, DateTime appointmentStartDate
            , string notes = null, int updateMode = 1, string toWhom = null)
        {
            var catiManagement =
                _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManagement6 = catiManagement as IRemoteCatiManagementServer6;

            var questionnaireId =
                _questionnaireService.GetQuestionnaireId(connectionModel, questionnaireName, serverParkName);
            var result = (int)(catiManagement6?.MakeHardAppointment(questionnaireId, primaryKey,
                appointmentStartDate, notes
                , updateMode, toWhom));
            catiManagement6.SynchronizeCatiDatabase(questionnaireId, new List<string> { primaryKey });
            return result;
        }

#else
        public int ClearCatiDataForQuestionnaire(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            throw new NotImplementedException();
        }

        public int ClearAppointments(ConnectionModel connectionModel, string questionnaireName, string serverParkName, List<string> primaryKeys)
        {
            throw new NotImplementedException();
        }

        public int CreateAppointment(ConnectionModel connectionModel, string questionnaireName
            , string serverParkName, string primaryKey, DateTime appointmentStartDate
            , string notes = null, int updateMode = 1, string toWhom = null)
        {
            throw new NotImplementedException();
        }

#endif
    }

}
