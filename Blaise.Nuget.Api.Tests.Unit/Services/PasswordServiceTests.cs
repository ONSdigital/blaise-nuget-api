using System.Security;
using Blaise.Nuget.Api.Core.Services;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class PasswordServiceTests
    {
        [Test]
        public void Given_I_Call_CreateSecurePassword_A_SecureString_Is_Returned()
        {
            //arrange
            const string password = "Password123";

            var passwordService = new PasswordService();

            //act
            var result = passwordService.CreateSecurePassword(password);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<SecureString>(result);
        }
    }
}
