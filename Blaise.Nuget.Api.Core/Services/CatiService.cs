using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Cati.Runtime;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IRemoteCatiManagementServerProvider _remoteCatiManagementServerProvider;
        private readonly ISurveyService _surveyService;

        public CatiService(
            IRemoteCatiManagementServerProvider remoteCatiManagementServerProvider,
            ISurveyService surveyService)
        {
            _remoteCatiManagementServerProvider = remoteCatiManagementServerProvider;
            _surveyService = surveyService;
        }

        public IEnumerable<ISurvey> GetInstalledSurveys(ConnectionModel connectionModel, string serverParkName)
        {
            var instruments = GetInstalledCatiSurveys(connectionModel, serverParkName);
            var instrumentNames = instruments.Keys;

            return instrumentNames.Select(instrumentName => _surveyService.GetSurvey(connectionModel, instrumentName, serverParkName)).ToList();
        }

        public ISurvey GetInstalledSurvey(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var instruments = GetInstalledCatiSurveys(connectionModel, serverParkName);

            if (!instruments.ContainsKey(instrumentName))
            {
                throw new DataNotFoundException($"No survey called '{instrumentName}' was found on server park '{serverParkName}'");
            }

            return _surveyService.GetSurvey(connectionModel, instrumentName, serverParkName);
        }

        public DayBatchModel CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            var catiManagementServer = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            
            if (catiManagementServer.LoadCatiInstrumentManager(instrumentId)
                .Specification
                .SurveyDays
                .All(d => d.Date.Date != dayBatchDate.Date))
            {
                throw new DataNotFoundException($"A survey day does not exist for the required daybatch date '{dayBatchDate.Date}'");
            }

            catiManagementServer
                .LoadCatiInstrumentManager(instrumentId)
                .CreateDaybatch(dayBatchDate);

            return GetDayBatch(catiManagementServer, instrumentId);
        }

        public DayBatchModel GetDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var catiManagementServer = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);

            return GetDayBatch(catiManagementServer, instrumentId);
        }

        public void AddToDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, string primaryKeyValue)
        { 
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);

            catiManagement.AddToDaybatch(instrumentId, primaryKeyValue);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyDays = new List<DateTime>();
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(instrumentId)?.Specification?.SurveyDays;

            if (surveyDateCollection == null || surveyDateCollection.Count == 0)
            {
                return surveyDays;
            }

            surveyDays.AddRange(surveyDateCollection.Select(surveyDay => surveyDay.Date));

            return surveyDays;
        }

        public void SetSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime surveyDay)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentId);

            catiManager.Specification.SurveyDays
                .AddSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void SetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName, List<DateTime> surveyDays)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentId);

            catiManager.Specification.SurveyDays
                .AddSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentId);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            List<DateTime> surveyDays)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentId);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        private IDictionary<string, Guid> GetInstalledCatiSurveys(ConnectionModel connectionModel, string serverParkName)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            return catiManagement.GetInstalledSurveys();
        }

        private static DayBatchModel GetDayBatch(IRemoteCatiManagementServer catiManagementServer, Guid instrumentId)
        {
            var dayBatchCaseEntries = catiManagementServer.GetKeysInDaybatch(instrumentId).ToList();

            if (!dayBatchCaseEntries.Any())
            {
                return null;
            }

            var dayBatchDate = catiManagementServer.GetDaybatchDate(instrumentId);

            return new DayBatchModel(dayBatchDate, dayBatchCaseEntries);
        }
    }
}
