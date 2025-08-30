namespace Blaise.Nuget.Api.Tests.Behaviour.AuditTrailData
{
    using Blaise.Nuget.Api.Api;
    using Blaise.Nuget.Api.Contracts.Models;
    using NUnit.Framework;
    using System;

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
        public void GetAuditTrail_WithValidParameters_ReturnsAuditTrailData()
        {
            // arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = "lms2301_ts6";

            // act
            var audotTrailDataModels = _auditTrailApi.GetAuditTrail(serverPark, questionnaireName);

            // assert
            Assert.That(audotTrailDataModels, Is.Not.Null);
            Assert.That(audotTrailDataModels, Is.Not.Empty);
        }

        [Ignore("Integration")]
        [Test]
        public void GetAuditTrail_With_Empty_Server_Park_Returns_An_Exception()
        {
            // arrange
            var serverPark = string.Empty;
            var questionnaireName = "lms2301_ts6";

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailApi.GetAuditTrail(serverPark, questionnaireName));
        }

        [Ignore("Integration")]
        [Test]
        public void GetAuditTrail_With_Empty_Questionnaire_Name_Returns_An_Exception()
        {
            // arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = string.Empty;

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailApi.GetAuditTrail(serverPark, questionnaireName));
        }
    }
}
