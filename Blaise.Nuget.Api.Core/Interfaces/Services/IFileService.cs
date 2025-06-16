using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        void UpdateQuestionnaireFileWithData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, bool addAudit = false);

        void UpdateQuestionnaireFileWithBatchedData(ConnectionModel connectionModel, string questionnaireFile,
            string questionnaireName, string serverParkName, int batchSize, bool addAudit = false);

        void UpdateQuestionnairePackageWithSqlConnection(
            string questionnaireName,
            string questionnaireFile,
            bool createDatabaseObjects);

        void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName);
    }
}
