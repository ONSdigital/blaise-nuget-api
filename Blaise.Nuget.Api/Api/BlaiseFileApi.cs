using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseFileApi : IBlaiseFileApi
    {
        private readonly IFileService _fileService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseFileApi(
            IFileService fileService,
            ConnectionModel connectionModel)
        {
            _fileService = fileService;
            _connectionModel = connectionModel;
        }

        public BlaiseFileApi(ConnectionModel connectionModel = null)
        {
            _fileService = UnityProvider.Resolve<IFileService>();

            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public void UpdateQuestionnaireFileWithData(string serverParkName, string questionnaireName
                                                                    , string questionnaireFile, bool auditOption)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");


        }

        public void UpdateQuestionnaireFileWithData(string serverParkName, string questionnaireName, string questionnaireFile)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");

            _fileService.UpdateQuestionnaireFileWithData(_connectionModel, questionnaireFile, questionnaireName, serverParkName);
        }

        public void UpdateQuestionnaireFileWithSqlConnection(string questionnaireName, string questionnaireFile)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");

            _fileService.UpdateQuestionnairePackageWithSqlConnection(questionnaireName, questionnaireFile);
        }

        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            fileName.ThrowExceptionIfNullOrEmpty("fileName");

            _fileService.CreateSettingsDataInterfaceFile(applicationType, fileName);
        }
    }
}
