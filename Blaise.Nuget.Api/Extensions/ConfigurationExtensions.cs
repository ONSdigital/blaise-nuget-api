namespace Blaise.Nuget.Api.Extensions
{
    using System;
    using System.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetConfigurationItem(string configurationItem)
        {
            var variable = Environment.GetEnvironmentVariable(configurationItem) ?? GetLocalVariable(configurationItem);
            variable.ThrowExceptionIfNullOrEmpty(configurationItem);
            return variable;
        }

        public static int GetConfigurationItemAsInt(string configurationItem)
        {
            var variable = Environment.GetEnvironmentVariable(configurationItem) ?? GetLocalVariable(configurationItem);
            variable.ThrowExceptionIfNullOrEmpty(configurationItem);
            return GetVariableAsInt(variable, configurationItem);
        }

        public static int GetVariableAsInt(string variable, string variableName)
        {
            variable.ThrowExceptionIfNotInt(variableName);

            return Convert.ToInt32(variable);
        }

        private static string GetLocalVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }
    }
}
