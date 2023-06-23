namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseAuditTrailApi
    {
        byte[] GetAuditTrail(string serverPark, string questionnaireName);
    }
}
