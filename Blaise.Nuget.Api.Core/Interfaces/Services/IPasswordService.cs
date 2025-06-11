using System.Security;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IPasswordService
    {
        SecureString CreateSecurePassword(string password);
    }
}
