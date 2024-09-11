using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseFileApi
    {
        void UpdateQuestionnaireFileWithData(string serverParkName, string questionnaireName,
            string questionnaireFile, bool auditOption = false);

        void UpdateQuestionnaireFileWithBatchedData(string questionnaireFile, string questionnaireName, 
            string serverParkName, int batchSize, bool addAudit = false);

        void UpdateQuestionnaireFileWithSqlConnection(string questionnaireName, string questionnaireFile, bool overwriteExistingData = true);

        void CreateSettingsDataInterfaceFile(ApplicationType applicationType, string fileName);
    }
}