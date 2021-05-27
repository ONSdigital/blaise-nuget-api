using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Services;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Admin
{
    public class ConnectionTests
    {
        [Ignore("Integration")]
        [Test]
        public void Given_A_Connection_Exists_When_I_Call_ResetConnections_Then_The_Cached_Connections_Are_Cleared()
        {
            //arrange
            var connectionModel = new ConnectionModel
            {
                ServerName = "",
                UserName = "",
                Password = "",
                Binding = "",
                Port = 0,
                RemotePort = 0,
                ConnectionExpiresInMinutes = 0
            };

            var sut = new ConnectedServerFactory(new PasswordService());

            //act
            sut.GetConnection(connectionModel);
            sut.GetConnection(connectionModel);
            sut.ResetConnections();
            sut.GetConnection(connectionModel);

            //assert
        }
    }
}
