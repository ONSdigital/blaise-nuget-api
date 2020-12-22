﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Cati.Runtime;
using StatNeth.Blaise.API.Cati.Specification;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DayBatchServiceTests
    {
        private Mock<ICatiManagementServerFactory> _catiFactoryMock;
        private Mock<IRemoteCatiManagementServer> _catiManagementServerMock;
        private Mock<ISurveyDayCollection> _surveyDayCollection;
        
        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private DayBatchService _sut;

        public DayBatchServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _surveyDayCollection = new Mock<ISurveyDayCollection>();

            _catiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementServerMock.Setup(c => c.SelectServerPark(It.IsAny<string>()));
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(It.IsAny<string>()).CreateDaybatch(It.IsAny<DateTime>()));
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(It.IsAny<string>()).Specification.SurveyDays).Returns(_surveyDayCollection.Object);

            _catiFactoryMock = new Mock<ICatiManagementServerFactory>();
            _catiFactoryMock.Setup(r => r.GetConnection(_connectionModel))
                .Returns(_catiManagementServerMock.Object);

            //setup service under test
            _sut = new DayBatchService(_catiFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_CreateDayBatch_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act
            _sut.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName)
                .CreateDaybatch(dayBatchDate), Times.Once);
        }

        [Test]
        public void Given_I_Call_SetSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.SetSurveyDay(_connectionModel, _instrumentName, _serverParkName, DateTime.Today);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).Specification.SurveyDays.AddSurveyDay(DateTime.Today), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).SaveSpecification(), Times.Once);
        }

        [Test]
        public void Given_I_Call_SetSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };
            //act
            _sut.SetSurveyDays(_connectionModel, _instrumentName, _serverParkName, surveyDays);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).Specification.SurveyDays.AddSurveyDays(surveyDays), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).SaveSpecification(), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetSurveyDays(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).Specification.SurveyDays, Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveSurveyDay(_connectionModel, _instrumentName, _serverParkName, DateTime.Today);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).Specification.SurveyDays.RemoveSurveyDay(DateTime.Today), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).SaveSpecification(), Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };
            //act
            _sut.RemoveSurveyDays(_connectionModel, _instrumentName, _serverParkName, surveyDays);

            //assert
            _catiFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).Specification.SurveyDays.RemoveSurveyDays(surveyDays), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName).SaveSpecification(), Times.Once);
        }
    }
}
