using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Interfaces;
using Blaise.Nuget.Api.Providers;
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
        public void Given_I_Call_ResetConnections_Then_The_Correct_Methods_Are_Called()
        {
            //arrange
            var iocProvider = new Mock<IIocProvider>();
            iocProvider.Setup(i => i.Resolve<It.IsAnyType>());

             var sut = new BlaiseAdminApi(iocProvider.Object);

            //act
            sut.ResetConnections();

            //assert
            iocProvider.Verify(v => v.Resolve<ICatiManagementServerFactory>());
        }
    }
}