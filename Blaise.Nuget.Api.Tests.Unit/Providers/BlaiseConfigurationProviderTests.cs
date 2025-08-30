using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class BlaiseConfigurationProviderTests
    {
        /// <summary>
        /// Ensure the App.config in the test project has values that relate to the tests
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ConnectionModel>());
            Assert.That(result.ServerName, Is.EqualTo("BlaiseServerHostNameTest"));
            Assert.That(result.UserName, Is.EqualTo("BlaiseServerUserNameTest"));
            Assert.That(result.Password, Is.EqualTo("BlaiseServerPasswordTest"));
            Assert.That(result.Binding, Is.EqualTo("BlaiseServerBindingTest"));
            Assert.That(result.Port, Is.EqualTo(10));
            Assert.That(result.RemotePort, Is.EqualTo(20));
        }

        [Test]
        public void Given_ConnectionExpiresInMinutes_Value_Is_Set_When_I_Call_ConnectionExpiresInMinutes_I_Get_The_Correct_Value_Back()
        {
            //act
            var result = _sut.ConnectionExpiresInMinutes;

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(60));
        }

        [Test]
        public void Given_ConnectionString_Value_Is_Set_When_I_Call_DatabaseConnectionString_I_Get_The_Correct_Value_Back_Including_Timeout()
        {
            //act
            var result = _sut.DatabaseConnectionString;

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("user id=blaise;server=0.0.0.0;database=blaise;password=password123;defaultcommandtimeout=500;connectiontimeout=500"));
        }
    }
}
