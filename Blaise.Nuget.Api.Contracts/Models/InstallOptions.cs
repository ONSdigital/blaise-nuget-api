using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class InstallOptions : IInstallOptions
    {
        /// <inheritdoc/>
        public string LayoutSetGroupName { get; set; }

        /// <inheritdoc/>
        public string DataEntrySettingsName { get; set; }

        /// <inheritdoc/>
        public DataOverwriteMode OverwriteMode { get; set; }

        /// <inheritdoc/>
        public HarmlessDataModificationMode HarmlessDataModificationMode { get; set; }

        /// <inheritdoc/>
        public bool GeneratePages { get; set; }

        /// <inheritdoc/>
        public bool RemoveSessions { get; set; }

        /// <inheritdoc/>
        public string InitialAppCariSetting { get; set; }

        /// <inheritdoc/>
        public Orientation Orientation { get; set; }

        /// <inheritdoc/>
        public string InitialAppDataEntrySettingsName { get; set; }

        /// <inheritdoc/>
        public string InitialModeName { get; set; }

        /// <inheritdoc/>
        public bool EnableClose { get; set; }

        /// <inheritdoc/>
        public bool EncryptDataFiles { get; set; }

        /// <inheritdoc/>
        public bool DownloadSessionData { get; set; }

        /// <inheritdoc/>
        public bool UploadSessionData { get; set; }

        /// <inheritdoc/>
        public bool AllowDownloadOverMeteredConnection { get; set; }

        /// <inheritdoc/>
        public string InitialAppLayoutSetGroupName { get; set; }

        /// <inheritdoc/>
        public DataConversionMode DataConversionMode { get; set; }
    }
}
