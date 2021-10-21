using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISurveyMetaService
    {
        IEnumerable<string> GetSurveyModes(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        IEnumerable<SurveyEntrySettingsModel> GetSurveyDataEntrySettings(ConnectionModel connectionModel, string instrumentName, string serverParkName);
    }
}