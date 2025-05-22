using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Extensions;

namespace Blaise.Nuget.Api.Providers
{
    public class BlaiseConfigurationProvider : IBlaiseConfigurationProvider
    {
        public ConnectionModel GetConnectionModel()
        {
            var connectionModel = new ConnectionModel
            {
                ServerName = ConfigurationExtensions.GetConfigurationItem("ENV_BLAISE_SERVER_HOST_NAME"),
                UserName = ConfigurationExtensions.GetConfigurationItem("ENV_BLAISE_ADMIN_USER"),
                Password = ConfigurationExtensions.GetConfigurationItem("ENV_BLAISE_ADMIN_PASSWORD"),
                Binding = ConfigurationExtensions.GetConfigurationItem("ENV_BLAISE_SERVER_BINDING"),
                Port = ConfigurationExtensions.GetConfigurationItemAsInt("ENV_BLAISE_CONNECTION_PORT"),
                RemotePort = ConfigurationExtensions.GetConfigurationItemAsInt("ENV_BLAISE_REMOTE_CONNECTION_PORT"),
                ConnectionExpiresInMinutes = ConnectionExpiresInMinutes
            };
            return connectionModel;
        }

        public int ConnectionExpiresInMinutes => ConfigurationExtensions.GetVariableAsInt(
            ConfigurationExtensions.GetConfigurationItem("ENV_CONNECTION_EXPIRES_IN_MINUTES") ?? "30",
            "ENV_CONNECTION_EXPIRES_IN_MINUTES");

        public string CommandTimeout => ConfigurationExtensions.GetConfigurationItem("COMMAND_TIMEOUT") ?? "300";

        public string DatabaseConnectionString => $"{ConfigurationExtensions.GetConfigurationItem("ENV_DB_CONNECTIONSTRING")};defaultcommandtimeout={CommandTimeout};connectiontimeout={CommandTimeout}";
    }
}
