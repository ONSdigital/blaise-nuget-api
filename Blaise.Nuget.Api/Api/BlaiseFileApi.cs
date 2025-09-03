namespace Blaise.Nuget.Api.Api
{
    using Blaise.Nuget.Api.Contracts.Interfaces;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Extensions;
    using Blaise.Nuget.Api.Providers;
    using StatNeth.Blaise.API.DataInterface;

    public class BlaiseFileApi : IBlaiseFileApi
    {
        private readonly IFileService _fileService;

        private readonly ConnectionModel _connectionModel;

        public BlaiseFileApi(
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

        /// <inheritdoc/>
        public void UpdateQuestionnaireFileWithData(
            string serverParkName,
            string questionnaireName,
            string questionnaireFile,
            bool auditOption = false)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");

            _fileService.UpdateQuestionnaireFileWithData(
                _connectionModel,
                questionnaireFile,
                questionnaireName,
                serverParkName,
                auditOption);
        }

        /// <inheritdoc/>
        public void UpdateQuestionnaireFileWithBatchedData(
            string serverParkName,
            string questionnaireName,
            string questionnaireFile,
            int batchSize,
            bool auditOption = false)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");

            _fileService.UpdateQuestionnaireFileWithBatchedData(
                _connectionModel,
                questionnaireFile,
                questionnaireName,
                serverParkName,
                batchSize,
                auditOption);
        }

        /// <inheritdoc/>
        public void UpdateQuestionnaireFileWithSqlConnection(
            string questionnaireName,
            string questionnaireFile,
            bool overwriteExistingData = true)
        {
            questionnaireName.ThrowExceptionIfNullOrEmpty("questionnaireName");
            questionnaireFile.ThrowExceptionIfNullOrEmpty("questionnaireFile");

            _fileService.UpdateQuestionnairePackageWithSqlConnection(questionnaireName, questionnaireFile, overwriteExistingData);
        }

        /// <inheritdoc/>
        public void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName)
        {
            fileName.ThrowExceptionIfNullOrEmpty("fileName");

            _fileService.CreateSettingsDataInterfaceFile(applicationType, fileName);
        }
    }
}
