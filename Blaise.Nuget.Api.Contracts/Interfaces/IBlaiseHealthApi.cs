namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseHealthApi
    {
        bool ConnectionModelIsHealthy();

        bool ConnectionToBlaiseIsHealthy();

        bool RemoteConnectionToBlaiseIsHealthy();

        bool RemoteConnectionToCatiIsHealthy();
    }
}
