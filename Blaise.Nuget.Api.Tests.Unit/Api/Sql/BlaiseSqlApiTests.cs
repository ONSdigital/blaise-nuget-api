using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Sql
{
    public class BlaiseSqlApiTests
    {
        private Mock<ISqlService> _sqlServiceMock;
        private Mock<IBlaiseConfigurationProvider> _configMock;

        private readonly string _instrumentName;
        private readonly string _connectionString;

        private IBlaiseSqlApi _sut;

        public BlaiseSqlApiTests()
        {
            _instrumentName = "Instrument1";
            _connectionString = "sql;uid;pwd";
        }

        [SetUp]
        public void SetUpTests()
        {
            _sqlServiceMock = new Mock<ISqlService>();
            _configMock = new Mock<IBlaiseConfigurationProvider>();

            _sut = new BlaiseSqlApi(_sqlServiceMock.Object, _configMock.Object);
        }

        [Test]
        public void Given_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseSqlApi());
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseIds_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(d => d.GetCaseIds(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCaseIds(_instrumentName);

            //assert
            _sqlServiceMock.Verify(v => v.GetCaseIds(_connectionString, _instrumentName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseIds_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var caseIds = new List<string>
            {
                "12345678",
                "91011188"
            };

            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(d => d.GetCaseIds(It.IsAny<string>(), It.IsAny<string>())).Returns(caseIds);

            //act
            var result =_sut.GetCaseIds(_instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(caseIds, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetCaseIds_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseIds(string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetCaseIds_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseIds(null));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseIdentifiers_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(d => d.GetCaseIdentifiers(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCaseIds(_instrumentName);

            //assert
            _sqlServiceMock.Verify(v => v.GetCaseIdentifiers(_connectionString, _instrumentName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseIdentifiers_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var caseIdentifiers = new List<CaseIdentifierModel>
            {
                new CaseIdentifierModel("12345678", "NP1234"),
                new CaseIdentifierModel("91011188", "NP1235")
            };

            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(d => d.GetCaseIdentifiers(It.IsAny<string>(), It.IsAny<string>())).Returns(caseIdentifiers);

            //act
            var result = _sut.GetCaseIdentifiers(_instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(caseIdentifiers, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetCaseIdentifiers_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseIdentifiers(string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetCaseIdentifiers_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseIdentifiers(null));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }
    }
}
