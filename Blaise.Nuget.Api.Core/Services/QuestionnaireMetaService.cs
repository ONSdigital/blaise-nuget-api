namespace Blaise.Nuget.Api.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.Meta;

    public class QuestionnaireMetaService : IQuestionnaireMetaService
    {
        private readonly IDataModelService _dataModelService;

        public QuestionnaireMetaService(IDataModelService dataModelService)
        {
            _dataModelService = dataModelService;
        }

        public IEnumerable<string> GetQuestionnaireModes(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName) as IDatamodel2;

            return dataModel == null ? new List<string>() : dataModel.Modes.Select(dm => dm.Name);
        }

        public IEnumerable<DataEntrySettingsModel> GetQuestionnaireDataEntrySettings(ConnectionModel connectionModel, string questionnaireName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, questionnaireName, serverParkName);
            var dataEntrySettings = dataModel.DataEntrySettings;

            var questionnaireEntrySettingsModelList = new List<DataEntrySettingsModel>();

            foreach (var dataEntrySetting in dataEntrySettings)
            {
                questionnaireEntrySettingsModelList.Add(new DataEntrySettingsModel
                {
                    Type = dataEntrySetting.Name,
                    SessionTimeout = ((IDataEntrySettings4)dataEntrySetting).SessionTimeout,
                    SaveSessionOnTimeout = ((IDataEntrySettings6)dataEntrySetting).SaveOnTimeout,
                    SaveSessionOnQuit = ((IDataEntrySettings6)dataEntrySetting).SaveOnQuit,
                    DeleteSessionOnTimeout = ((IDataEntrySettings6)dataEntrySetting).DeleteSessionOnTimeout,
                    DeleteSessionOnQuit = ((IDataEntrySettings6)dataEntrySetting).DeleteSessionOnQuit,
                    ApplyRecordLocking = ((IDataEntrySettings4)dataEntrySetting).ApplyRecordLocking,
                });
            }

            return questionnaireEntrySettingsModelList;
        }
    }
}
