namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;

    public interface IQuestionnaireMetaService
    {
        IEnumerable<string> GetQuestionnaireModes(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}
