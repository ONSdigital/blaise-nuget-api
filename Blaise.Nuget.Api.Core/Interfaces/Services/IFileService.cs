namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        string GetDatabaseFileName(string filePath, string instrumentName);
        bool DatabaseFileExists(string filePath, string instrumentName);
        string CreateDatabaseFile(string metaFileName, string filePath, string instrumentName);

        void BackupDatabaseFile(string dataFileName, string metaFileName, string destinationPath);
        void CopyFileToDirectory(string dataFileName, string destinationFilePath);
        void CopyFileToGCloudBucket(string dataFileName, string destinationBucket);
    }
}