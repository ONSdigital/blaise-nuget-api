using System;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Mappers;
using Blaise.Nuget.Api.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using StatNeth.Blaise.API.AuditTrail;

namespace Blaise.Nuget.Api.Tests.Unit.Mappers
{
    public class AuditTrailDataMapperTests
    {
        private IAuditTrailDataMapper _sut;


        [SetUp]
        public void SetupTests()
        {
            _sut = new AuditTrailDataMapper();
        }

        [Test]
        public void Given_An_EventInfo_KeyValue_And_SessionId_When_I_Call_MapAuditTrailDataModel_I_Get_An_Expected_AuditTrailDataModel_Back()
        {
            //arrange
            const string keyValue = "keyValue";
            var sessionId = Guid.NewGuid();
            var eventTimeStamp = DateTime.Now;
            var eventInfoContent = "blah";

            var eventInfoMock = new Mock<IEventInfo>();
            eventInfoMock.Setup(ei => ei.TimeStamp).Returns(eventTimeStamp);
            eventInfoMock.Setup(ei => ei.ToString()).Returns(eventInfoContent);

            //act
            var result = _sut.MapAuditTrailDataModel(keyValue, sessionId, eventInfoMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<AuditTrailDataModel>(result);
            Assert.AreEqual(keyValue, result.KeyValue);
            Assert.AreEqual(sessionId, result.SessionId);
            Assert.AreEqual(eventTimeStamp, result.TimeStamp);
            Assert.AreEqual(eventInfoContent, result.Content);
        }

        [Test]
        public void Given_A_list_of_AuditTrailDataModels_When_I_Call_MapAuditTrailCsvContent_I_Get_An_Expected_string_back()
        {
            //arrange
            var auditTrailDataModels = new List<AuditTrailDataModel>
            {
                new AuditTrailDataModel
                {
                    KeyValue = "keyValue1",
                    SessionId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now.AddDays(-1),
                    Content = "blah"
                },
                new AuditTrailDataModel
                {
                    KeyValue = "keyValue2",
                    SessionId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    Content = "meh"
                }
            };

            var expectedCsv =
                $"KeyValue,SessionId,Timestamp,Content\r\nkeyValue1, {auditTrailDataModels[0].SessionId}, {auditTrailDataModels[0].TimeStamp:dd/MM/yyyy HH:mm:ss}, blah\r\nkeyValue2, {auditTrailDataModels[1].SessionId}, {auditTrailDataModels[1].TimeStamp:dd/MM/yyyy HH:mm:ss}, meh\r\n";

            //act
            var result = _sut.MapAuditTrailCsvContent(auditTrailDataModels);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual(expectedCsv, result);

        }
    }
}
