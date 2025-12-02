namespace Blaise.Nuget.Api.Tests.Behaviour.AuditTrailData
{
    using System;
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;

    public class BlaiseAuditTrailApiTests
    {
        private readonly ConnectionModel _connectionModel;

        private BlaiseAuditTrailApi _auditTrailApi;

        public BlaiseAuditTrailApiTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void Setup()
        {
            _auditTrailApi = new BlaiseAuditTrailApi(_connectionModel);
        }

        [Ignore("Integration")]
        [Test]
        public void When_I_Call_GetAuditTrail_Then_AuditTrailData_Is_Returned()
        {
            // arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = "lms2301_ts6";

            // act
            var auditTrailDataModels = _auditTrailApi.GetAuditTrail(serverPark, questionnaireName);

            // assert
            Assert.That(auditTrailDataModels, Is.Not.Null);
            Assert.That(auditTrailDataModels, Is.Not.Empty);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Empty_Server_Park_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var serverPark = string.Empty;
            var questionnaireName = "lms2301_ts6";

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailApi.GetAuditTrail(serverPark, questionnaireName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Empty_Questionnaire_Name_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            // arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = string.Empty;

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailApi.GetAuditTrail(serverPark, questionnaireName));
        }
    }
}
