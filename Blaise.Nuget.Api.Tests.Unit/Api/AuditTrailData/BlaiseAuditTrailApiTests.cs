using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
// ReSharper disable All

namespace Blaise.Nuget.Api.Tests.Unit.Api.AuditTrailData
{
    public class BlaiseAuditTrailApiTests
    {
        private readonly ConnectionModel _connectionModel;

        public BlaiseAuditTrailApiTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [Test]
        public void GetAuditTrail_Returns_ValidAuditTrail()
        {
            // Arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = "lms2301_ts6";
            //var instrumentId = "60013c5d-dcb0-4dc4-903a-9a6aaa7fee94"; /*Needs to ultimately use the questionnaire name*/

            var auditTrailApi = new BlaiseAuditTrailApi(_connectionModel);

            // Act
            auditTrailApi.GetAuditTrail(serverPark, questionnaireName);

            // Assert
            Assert.Pass();
        }

        [Test]
        public void GetAuditTrail_With_Empty_Server_Park_Returns_An_Exception()
        {
        }

        [Test]
        public void GetAuditTrail_With_Empty_InstrumentId_Returns_An_Exception()
        {
        }
    }
}