namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    using System.Security;

    public interface IPasswordService
    {
        SecureString CreateSecurePassword(string password);
    }
}
