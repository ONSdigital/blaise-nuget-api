namespace Blaise.Nuget.Api.Contracts.Models
{
    public class ConnectionModel
    {
        public string ServerName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Binding { get; set; }

        public int Port { get; set; }

        public int RemotePort { get; set; }

        public int ConnectionExpiresInMinutes { get; set; }
    }
}
