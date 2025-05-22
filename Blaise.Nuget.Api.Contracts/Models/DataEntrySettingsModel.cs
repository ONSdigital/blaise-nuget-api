namespace Blaise.Nuget.Api.Contracts.Models
{
    public class DataEntrySettingsModel
    {
        public string Type { get; set; }
        public bool SaveSessionOnTimeout { get; set; }
        public bool SaveSessionOnQuit { get; set; }
        public bool DeleteSessionOnTimeout { get; set; }
        public bool DeleteSessionOnQuit { get; set; }
        public int SessionTimeout { get; set; }
        public bool ApplyRecordLocking { get; set; }
    }
}
