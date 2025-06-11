using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class InstallOptions : IInstallOptions
    {
        public string LayoutSetGroupName { get; set; }

        public string DataEntrySettingsName { get; set; }

        public DataOverwriteMode OverwriteMode { get; set; }

        public HarmlessDataModificationMode HarmlessDataModificationMode { get; set; }

        public bool GeneratePages { get; set; }

        public bool RemoveSessions { get; set; }

        public string InitialAppCariSetting { get; set; }

        public Orientation Orientation { get; set; }

        public string InitialAppDataEntrySettingsName { get; set; }

        public string InitialModeName { get; set; }

        public bool EnableClose { get; set; }

        public bool EncryptDataFiles { get; set; }

        public bool DownloadSessionData { get; set; }

        public bool UploadSessionData { get; set; }

        public bool AllowDownloadOverMeteredConnection { get; set; }

        public string InitialAppLayoutSetGroupName { get; set; }
    }
}
