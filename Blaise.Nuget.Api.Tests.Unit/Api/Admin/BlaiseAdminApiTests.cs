using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Core.Interfaces.Admin;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Admin
{
    public class BlaiseAdminApiTests
    {
        [Test]
        public void Given_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseAdminApi());
        }

        [Test]
        public void Given_I_Call_ResetConnections_Then_The_ResetConnections_Method_Is_Called_On_All_Expected_Services()
        {
            //arrange
            var iocProvider = new Mock<IIocProvider>();

            var catiMock = new Mock<ICatiManagementServerFactory>();
            var connectedMock = new Mock<IConnectedServerFactory>();
            var remoteServerMock = new Mock<IRemoteDataServerFactory>();
            var securityMock = new Mock<ISecurityManagerFactory>();
            var remoteDataMock = new Mock<IRemoteDataLinkProvider>();
            var localDataMock = new Mock<ILocalDataLinkProvider>();
            
            iocProvider.Setup(i => i.Resolve<ICatiManagementServerFactory>()).Returns(catiMock.Object);
            iocProvider.Setup(i => i.Resolve<IConnectedServerFactory>()).Returns(connectedMock.Object);
            iocProvider.Setup(i => i.Resolve<IRemoteDataServerFactory>()).Returns(remoteServerMock.Object);
            iocProvider.Setup(i => i.Resolve<ISecurityManagerFactory>()).Returns(securityMock.Object);
            iocProvider.Setup(i => i.Resolve<IRemoteDataLinkProvider>()).Returns(remoteDataMock.Object);
            iocProvider.Setup(i => i.Resolve<ILocalDataLinkProvider>()).Returns(localDataMock.Object);

            var sut = new BlaiseAdminApi(iocProvider.Object);

            //act
            sut.ResetConnections();

            //assert
            iocProvider.Verify(v => v.Resolve<ICatiManagementServerFactory>());
            iocProvider.Verify(v => v.Resolve<IConnectedServerFactory>());
            iocProvider.Verify(v => v.Resolve<IRemoteDataServerFactory>());
            iocProvider.Verify(v => v.Resolve<ISecurityManagerFactory>());
            iocProvider.Verify(v => v.Resolve<IRemoteDataLinkProvider>());
            iocProvider.Verify(v => v.Resolve<ILocalDataLinkProvider>());

            catiMock.Verify(v => v.ResetConnections());
            connectedMock.Verify(v => v.ResetConnections());
            remoteServerMock.Verify(v => v.ResetConnections());
            securityMock.Verify(v => v.ResetConnections());
            remoteDataMock.Verify(v => v.ResetConnections());
            localDataMock.Verify(v => v.ResetConnections());
        }
    }
}