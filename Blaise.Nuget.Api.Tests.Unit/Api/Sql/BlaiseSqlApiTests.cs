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

        private readonly string _questionnaireName;
        private readonly string _primaryKey;
        private readonly string _connectionString;

        private IBlaiseSqlApi _sut;

        public BlaiseSqlApiTests()
        {
            _questionnaireName = "Questionnaire1";
            _primaryKey = "1234455";
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
            _sqlServiceMock.Setup(s => s.GetCaseIds(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCaseIds(_questionnaireName);

            //assert
            _sqlServiceMock.Verify(v => v.GetCaseIds(_connectionString, _questionnaireName), Times.Once);
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
            _sqlServiceMock.Setup(s => s.GetCaseIds(It.IsAny<string>(), It.IsAny<string>())).Returns(caseIds);

            //act
            var result = _sut.GetCaseIds(_questionnaireName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(caseIds));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCaseIds_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseIds(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCaseIds_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseIds(null));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetEditingCaseIds_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(s => s.GetEditingCaseIds(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetEditingCaseIds(_questionnaireName);

            //assert
            _sqlServiceMock.Verify(v => v.GetEditingCaseIds(_connectionString, _questionnaireName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetEditingCaseIds_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var caseIds = new List<string>
            {
                "12345678",
                "91011188"
            };

            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(s => s.GetEditingCaseIds(It.IsAny<string>(), It.IsAny<string>())).Returns(caseIds);

            //act
            var result = _sut.GetEditingCaseIds(_questionnaireName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(caseIds));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetEditingCaseIds_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetEditingCaseIds(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetEditingCaseIds_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetEditingCaseIds(null));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetCaseIdentifiers_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(s => s.GetCaseIdentifiers(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetCaseIdentifiers(_questionnaireName);

            //assert
            _sqlServiceMock.Verify(v => v.GetCaseIdentifiers(_connectionString, _questionnaireName), Times.Once);
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
            _sqlServiceMock.Setup(s => s.GetCaseIdentifiers(It.IsAny<string>(), It.IsAny<string>())).Returns(caseIdentifiers);

            //act
            var result = _sut.GetCaseIdentifiers(_questionnaireName);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(caseIdentifiers));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetCaseIdentifiers_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetCaseIdentifiers(string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetCaseIdentifiers_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetCaseIdentifiers(null));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPostCode_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(s => s.GetPostCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetPostCode(_questionnaireName, _primaryKey);

            //assert
            _sqlServiceMock.Verify(v => v.GetPostCode(_connectionString, _questionnaireName, _primaryKey), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPostCode_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var postCode = "NP1223";

            _configMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);
            _sqlServiceMock.Setup(s => s.GetPostCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(postCode);

            //act
            var result = _sut.GetPostCode(_questionnaireName, _primaryKey);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(postCode));
        }

        [Test]
        public void Given_An_Empty_QuestionnaireName_When_I_Call_GetPostCode_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetPostCode(string.Empty, _primaryKey));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'questionnaireName' must be supplied"));
        }

        [Test]
        public void Given_A_Null_QuestionnaireName_When_I_Call_GetPostCode_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetPostCode(null, _primaryKey));
            Assert.That(exception.ParamName, Is.EqualTo("questionnaireName"));
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetPostCode_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetPostCode(_questionnaireName, string.Empty));
            Assert.That(exception.Message, Is.EqualTo("A value for the argument 'primaryKey' must be supplied"));
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetPostCode_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetPostCode(_questionnaireName, null));
            Assert.That(exception.ParamName, Is.EqualTo("primaryKey"));
        }
    }
}
