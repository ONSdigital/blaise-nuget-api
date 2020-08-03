
using System;
using System.IO;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IConfigurationProvider _configurationProvider;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";
        private const string LibraryFileExt = "blix";

        public FileService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public string GetDatabaseFileName(string filePath, string instrumentName)
        {
            return GetFullFilePath(filePath, instrumentName, DatabaseFileNameExt);
        }

        public bool DatabaseFileExists(string filePath, string instrumentName)
        {
            return File.Exists(Path.Combine(filePath, $"{instrumentName}.{DatabaseSourceExt}"));
        }

        public void BackupDatabaseFile(string dataFileName, string metaFileName, string destinationPath)
        {
            //copy source file
            var sourceFilePath = Path.GetDirectoryName(dataFileName);

            if (string.IsNullOrWhiteSpace(sourceFilePath))
            {
                throw new NullReferenceException($"Could not find path for the dataFile '{dataFileName}'");
            }

            var sourceFileName = Path.GetFileNameWithoutExtension(dataFileName);
            var fullSourceFilePath = Path.Combine(sourceFilePath, $"{sourceFileName}.{DatabaseSourceExt}");

            if (destinationPath.StartsWith("gs://"))
            {
                CopyFileToGCloudBucket(dataFileName, Path.Combine(destinationPath, Path.GetFileName(dataFileName)));
                CopyFileToGCloudBucket(metaFileName, Path.Combine(destinationPath, Path.GetFileName(metaFileName)));
                CopyFileToGCloudBucket(fullSourceFilePath, Path.Combine(destinationPath, Path.GetFileName(fullSourceFilePath)));
            }
            else
            {
                CopyFileToDirectory(dataFileName, destinationPath);
                CopyFileToDirectory(metaFileName, destinationPath);
                CopyFileToDirectory(fullSourceFilePath, destinationPath);
            }
        }

        public string CreateDatabaseFile(string metaFileName, string filePath, string instrumentName)
        {
            CopyLibraryFiles(_configurationProvider.LibraryDirectory, filePath);

            Directory.CreateDirectory(filePath);
            var fileNameToCopy = GetFullFilePath(filePath, instrumentName, DatabaseModelExt);
            File.Copy(metaFileName, fileNameToCopy, true);

            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.Blaise;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.BlaiseDataProviderForDotNET;
            dataInterface.DataPartitionType = DataPartitionType.Stream;
            var connectionBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionBuilder.DataSource = GetFullFilePath(filePath, instrumentName, DatabaseSourceExt);
            dataInterface.ConnectionInfo.SetConnectionString(connectionBuilder.ConnectionString);
            dataInterface.DatamodelFileName = GetFullFilePath(filePath, instrumentName, DatabaseModelExt);
            dataInterface.FileName = GetFullFilePath(filePath, instrumentName, DatabaseFileNameExt);
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(null, true);
            dataInterface.SaveToFile(true);

            return dataInterface.FileName;
        }

        public void CopyFileToDirectory(string sourceFile, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);
            var destinationFile = Path.Combine(destinationPath, Path.GetFileName(sourceFile));
            File.Copy(sourceFile, destinationFile, true);
        }

        public void CopyFileToGCloudBucket(string sourceFile, string destinationPath)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();

            psi.FileName = "gsutil";
            psi.Arguments = String.Join(" ", new string[]{ "cp", sourceFile, destinationPath});
            System.Diagnostics.Process.Start(psi);
        }

        private static string GetFullFilePath(string filePath, string instrumentName, string extension)
        {
            return Path.Combine(filePath, $"{instrumentName}.{extension}");
        }

        private static void CopyLibraryFiles(string libraryPath, string destinationPath)
        {
            if (!Directory.Exists(libraryPath))
            {
                throw new ArgumentException($"The library path '{libraryPath}' provided in the config does not exist");
            }

            var libraryFiles = Directory.GetFiles(libraryPath, $"*.{LibraryFileExt}", SearchOption.AllDirectories);

            if (!libraryFiles.Any())
            {
                throw new DataNotFoundException($"No Library files with extension '{LibraryFileExt}' found in any directory under '{libraryPath}'");
            }

            Directory.CreateDirectory(destinationPath);

            foreach (var libraryFile in libraryFiles)
            {
                if (FileIsLocked(libraryFile))
                {
                   throw new AccessViolationException($"Could not copy file {libraryFile} as it is locked");
                }

                var destinationFile = Path.Combine(destinationPath, Path.GetFileName(libraryFile));

                if (File.Exists(destinationFile) && FileIsLocked(destinationFile))
                {
                    throw new AccessViolationException($"Could not write to file {destinationFile} as it is locked");
                }

                File.Copy(libraryFile, destinationFile, true);
            }
        }

        private static bool FileIsLocked(string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);

                using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
