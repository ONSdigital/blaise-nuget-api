namespace Blaise.Nuget.Api.Tests.Unit.Mappers
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Mappers;
    using Moq;
    using NUnit.Framework;
    using StatNeth.Blaise.API.AuditTrail;

    public class AuditTrailDataMapperTests
    {
        private IAuditTrailDataMapper _sut;

        [SetUp]
        public void SetupTests()
        {
            _sut = new AuditTrailDataMapper();
        }

        [Test]
        public void Given_An_EventInfo_KeyValue_And_SessionId_When_I_Call_MapAuditTrailDataModel_Then_An_Expected_Audit_Trail_Data_Model_Is_Returned()
        {
            // arrange
            const string KeyValue = "KeyValue";
            var sessionId = Guid.NewGuid();
            var eventTimeStamp = DateTime.Now;
            const string EventInfoContent = "blah";

            var eventInfoMock = new Mock<IEventInfo>();
            eventInfoMock.Setup(ei => ei.TimeStamp).Returns(eventTimeStamp);
            eventInfoMock.Setup(ei => ei.ToString()).Returns(EventInfoContent);

            // act
            var result = _sut.MapAuditTrailDataModel(KeyValue, sessionId, eventInfoMock.Object);

            // assert
            Assert.That(result, Is.InstanceOf<AuditTrailDataModel>());
            Assert.That(result.KeyValue, Is.EqualTo(KeyValue));
            Assert.That(result.SessionId, Is.EqualTo(sessionId));
            Assert.That(result.TimeStamp, Is.EqualTo(eventTimeStamp));
            Assert.That(result.Content, Is.EqualTo(EventInfoContent));
        }

        [Test]
        public void Given_A_List_of_AuditTrailDataModels_When_I_Call_MapAuditTrailCsvContent_Then_An_Expected_String_Is_Returned()
        {
            // arrange
            var auditTrailDataModels = new List<AuditTrailDataModel>
            {
                new AuditTrailDataModel
                {
                    KeyValue = "KeyValue1",
                    SessionId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now.AddDays(-1),
                    Content = "blah",
                },
                new AuditTrailDataModel
                {
                    KeyValue = "KeyValue2",
                    SessionId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    Content = "meh",
                },
            };

            var expectedCsv =
                $"KeyValue,SessionId,Timestamp,Content\r\nKeyValue1, {auditTrailDataModels[0].SessionId}, {auditTrailDataModels[0].TimeStamp:dd/MM/yyyy HH:mm:ss}, blah\r\nKeyValue2, {auditTrailDataModels[1].SessionId}, {auditTrailDataModels[1].TimeStamp:dd/MM/yyyy HH:mm:ss}, meh\r\n";

            // act
            var result = _sut.MapAuditTrailCsvContent(auditTrailDataModels);

            // assert
            Assert.That(result, Is.InstanceOf<string>());
            Assert.That(result, Is.EqualTo(expectedCsv));
        }
    }
}
