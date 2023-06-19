﻿using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Nuget.Api.Tests.Unit.AuditTrailData
{
    public class BlaiseAuditTrailApiTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly BlaiseAuditTrailApi _auditTrailApi;

        public BlaiseAuditTrailApiTests()
        {
            _connectionModel = new ConnectionModel();
            _auditTrailApi = new BlaiseAuditTrailApi(_connectionModel);
        }

        [Test]
        public void Test_One()
        {
            _auditTrailApi.GetAuditTrail();
        }
    }
}