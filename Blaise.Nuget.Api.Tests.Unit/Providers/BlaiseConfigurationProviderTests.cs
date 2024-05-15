using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class BlaiseConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        private IBlaiseConfigurationProvider _sut;

        [SetUp]
        public void SetUpTests()
        {
            _sut = new BlaiseConfigurationProvider();
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_GetConnectionModel_I_Get_A_ConnectionModel_Back()
        {
            //act
            var result = _sut.GetConnectionModel();

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ConnectionModel>(result);
            Assert.AreEqual("BlaiseServerHostNameTest", result.ServerName);
            Assert.AreEqual("BlaiseServerUserNameTest", result.UserName);
            Assert.AreEqual("BlaiseServerPasswordTest", result.Password);
            Assert.AreEqual("BlaiseServerBindingTest", result.Binding);
            Assert.AreEqual(10, result.Port);
            Assert.AreEqual(20, result.RemotePort);
        }

        [Test]
        public void Given_ConnectionExpiresInMinutes_Value_Is_Set_When_I_Call_ConnectionExpiresInMinutes_I_Get_The_Correct_Value_Back()
        {
            //act
            var result = _sut.ConnectionExpiresInMinutes;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(60, result);
        }

        [Test]
        public void Given_ConnectionString_Value_Is_Set_When_I_Call_DatabaseConnectionString_I_Get_The_Correct_Value_Back_Including_Timeout()
        {
            //act
            var result = _sut.DatabaseConnectionString;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("user id=blaise;server=0.0.0.0;database=blaise;password=password123;defaultcommandtimeout=500;connectiontimeout=500", result);
        }
    }
}
