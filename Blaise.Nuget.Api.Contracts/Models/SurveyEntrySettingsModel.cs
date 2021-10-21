namespace Blaise.Nuget.Api.Contracts.Models
{
    public class SurveyEntrySettingsModel
    {
        public string Type { get; set; }
        public bool DeleteSessionOnTimeout { get; set; }
        public bool DeleteSessionOnQuit { get; set; }
        public int SessionTimeout { get; set; }
    }
}