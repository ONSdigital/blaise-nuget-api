using System.Security;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class PasswordService : IPasswordService
    {
        /// <inheritdoc/>
        public SecureString CreateSecurePassword(string password)
        {
            var securePassword = new SecureString();
            foreach (var c in password)
            {
                securePassword.AppendChar(c);
            }

            return securePassword;
        }
    }
}
