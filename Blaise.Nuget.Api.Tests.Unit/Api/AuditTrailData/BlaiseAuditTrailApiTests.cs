using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
using System;
// ReSharper disable All

namespace Blaise.Nuget.Api.Tests.Unit.Api.AuditTrailData
{
    public class BlaiseAuditTrailApiTests
    {
        private readonly ConnectionModel _connectionModel;
        private BlaiseAuditTrailApi _auditTrailService;

        public BlaiseAuditTrailApiTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void Setup()
        {
            // Initialize the AuditTrailService or mock its dependencies
            _auditTrailService = new BlaiseAuditTrailApi(_connectionModel);
        }

        [Test]
        public void GetAuditTrail_WithValidParameters_ReturnsAuditTrailData()
        {
            // Arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = "lms2301_ts6";

            // Act
            var csvAsBytes = _auditTrailService.GetAuditTrail(serverPark, questionnaireName);

            // Assert
            Assert.IsNotNull(csvAsBytes);
        }

        [Test]
        public void GetAuditTrail_With_Empty_Server_Park_Returns_An_Exception()
        {
            // Arrange
            var serverPark = "";
            var questionnaireName = "lms2301_ts6";

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailService.GetAuditTrail(serverPark, questionnaireName));
        }

        [Test]
        public void GetAuditTrail_With_Empty_Questionnaire_Name_Returns_An_Exception()
        {
            // Arrange
            var serverPark = "LocalDevelopment";
            var questionnaireName = "";

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => _auditTrailService.GetAuditTrail(serverPark, questionnaireName));
        }
    }
}