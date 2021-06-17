using System.Linq;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Providers;
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
            var adminApi = new BlaiseAdminApi();


            //act
            sut.GetConnection(connectionModel);
            sut.GetConnection(connectionModel);

            sut.ResetConnections();
            sut.GetConnection(connectionModel);

            //assert
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Connection_Exists_When_I_Call_GetConnections_Then_The_Connection_Count_Is_Returned()
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

            var instrumentName = "LMS2102_BK1";
            var serverParkName = "LocalDevelopment";
            var sut = UnityProvider.Resolve<IRemoteDataLinkProvider>();
            var adminApi = new BlaiseAdminApi();


            //act
            sut.GetDataLink(connectionModel, instrumentName, serverParkName);
            var connections = adminApi.OpenConnections();

            Assert.AreEqual(6, connections.Count());

            sut.ResetConnections();
            connections = adminApi.OpenConnections();

            //assert
            Assert.AreEqual(0, connections);
        }
    }
}
