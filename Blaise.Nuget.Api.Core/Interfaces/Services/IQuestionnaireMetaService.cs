namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using Blaise.Nuget.Api.Contracts.Models;
    using System.Collections.Generic;

    public interface IQuestionnaireMetaService
    {
        IEnumerable<string> GetQuestionnaireModes(ConnectionModel connectionModel, string questionnaireName, string serverParkName);

        IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(ConnectionModel connectionModel, string questionnaireName, string serverParkName);
    }
}
