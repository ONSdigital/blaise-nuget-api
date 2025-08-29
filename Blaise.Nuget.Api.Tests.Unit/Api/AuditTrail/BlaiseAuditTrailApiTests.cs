using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;

namespace Blaise.Nuget.Api.Tests.Unit.Api.AuditTrail
{
    public class BlaiseAuditTrailApiTests
    {
        private IBlaiseAuditTrailApi _sut;
        private readonly ConnectionModel _connectionModel;
        private Mock<IAuditTrailService> _auditTrailServiceMock;
        private readonly string _questionnaireName;
        private readonly string _serverParkName;

        public BlaiseAuditTrailApiTests()
        {
            _questionnaireName = "LMS2201A_BP1";
            _serverParkName = "gusty";
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _auditTrailServiceMock = new Mock<IAuditTrailService>();

            _sut = new BlaiseAuditTrailApi(_auditTrailServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseAuditTrailApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.That(() => new BlaiseAuditTrailApi(), Throws.Nothing);
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseAuditTrailApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.That(() => new BlaiseAuditTrailApi(new ConnectionModel()), Throws.Nothing);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetAuditTrail_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetAuditTrail(_questionnaireName, _serverParkName);

            //assert
            _auditTrailServiceMock.Verify(v => v.GetAuditTrailData(_connectionModel,
                _questionnaireName, _serverParkName), Times.Once());
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetAuditTrail_Then_A_List_Of_AuditTrailDataModels_Is_Returned()
        {
            //arrange
            var auditTrailDataList = new List<AuditTrailDataModel>();
            _auditTrailServiceMock.Setup(at => at.GetAuditTrailData(
                    It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(auditTrailDataList);

            //act
            var result = _sut.GetAuditTrail(_questionnaireName, _serverParkName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<AuditTrailDataModel>>());
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetAuditTrail_Then_The_Expected_Byte_Array_Is_Returned()
        {
            //arrange
            var auditTrailDataList = new List<AuditTrailDataModel>();
            _auditTrailServiceMock.Setup(at => at.GetAuditTrailData(
                It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(auditTrailDataList);

            //act
            var result = _sut.GetAuditTrail(_questionnaireName, _serverParkName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(auditTrailDataList));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetAuditTrail(null, _serverParkName));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetAuditTrail_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetAuditTrail(string.Empty, _serverParkName));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetAuditTrail(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("serverParkName"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetAuditTrail_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetAuditTrail(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'serverParkName' must be supplied"));
        }
    }
}