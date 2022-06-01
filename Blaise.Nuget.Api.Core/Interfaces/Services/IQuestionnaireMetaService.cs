using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IQuestionnaireMetaService
    {
        IEnumerable<string> GetQuestionnaireModes(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
        IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}