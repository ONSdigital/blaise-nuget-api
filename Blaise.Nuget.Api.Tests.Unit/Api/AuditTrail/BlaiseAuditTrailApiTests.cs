
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Blaise.Nuget.Api.Tests.Unit.Api.AuditTrail
{
    public class BlaiseAuditTrailApiTests
    {
        private BlaiseAuditTrailApi _sut;
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

            Assert.DoesNotThrow(() => new BlaiseAuditTrailApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseAuditTrailApi_No_Exceptions_Are_Thrown()
        {
            Assert.DoesNotThrow(() => new BlaiseAuditTrailApi(new ConnectionModel()));
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
        public void Given_Valid_Arguments_When_I_Call_GetAuditTrail_Then_A_Byte_Array_Is_Returned()
        {
            //act
            var result = _sut.GetAuditTrail(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<byte[]>(result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetAuditTrail_Then_The_Expected_Byte_Array_Is_Returned()
        {
            //arrange
            var byteArray = Array.Empty<byte>();
            _auditTrailServiceMock.Setup(at => at.GetAuditTrailData(
                It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(byteArray);

            //act
            var result = _sut.GetAuditTrail(_questionnaireName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(byteArray, result);
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetAuditTrail(null, _serverParkName));
            Assert.AreEqual("questionnaireName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetAuditTrail_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetAuditTrail(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'questionnaireName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetAuditTrail_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetAuditTrail(_questionnaireName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetAuditTrail_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetAuditTrail(_questionnaireName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }
    }
}
