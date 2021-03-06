﻿using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class ConfigurationModelTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly ConnectionModel _connectionModel;

        private IBlaiseApi _sut;

        public ConfigurationModelTests()
        {
            _connectionModel = new ConnectionModel();
        }
        [SetUp]
        public void SetUpTests()
        {
            _caseServiceMock = new Mock<ICaseService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();

            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.GetConnectionModel())
                .Returns(_connectionModel);

            _sut = new BlaiseApi(
                _caseServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDefaultConnectionModel_The_Correct_Model_Is_Returned()
        {
            //act
            var result = _sut.GetDefaultConnectionModel();

            //assert
            Assert.AreSame(_connectionModel, result);
        }

        [Test]
        public void Given_I_Call_GetDefaultConnectionModel_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDefaultConnectionModel();

            //assert
            _configurationProviderMock.Verify(v => v.GetConnectionModel(), Times.Once);
        }
    }
}
