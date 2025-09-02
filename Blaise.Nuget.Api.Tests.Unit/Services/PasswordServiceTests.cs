namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    using System.Security;
    using Blaise.Nuget.Api.Core.Services;
    using NUnit.Framework;

    public class PasswordServiceTests
    {
        [Test]
        public void Given_A_Password_When_I_Call_CreateSecurePassword_A_Correct_SecureString_Is_Returned()
        {
            // arrange
            const string password = "Password123";
            var passwordService = new PasswordService();

            // act
            var result = passwordService.CreateSecurePassword(password);

            // assert
            Assert.That(result, Is.InstanceOf<SecureString>());
            var unsecurePassword = new System.Net.NetworkCredential(string.Empty, result).Password;
            Assert.That(unsecurePassword, Is.EqualTo(password));
        }
    }
}
