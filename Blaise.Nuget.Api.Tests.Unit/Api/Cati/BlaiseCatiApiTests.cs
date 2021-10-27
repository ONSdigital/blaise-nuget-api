using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Cati
{
    public class BlaiseCatiApiTests
    {
        private Mock<ICatiService> _catiServiceMock;

        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseCatiApi _sut;

        public BlaiseCatiApiTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _catiServiceMock = new Mock<ICatiService>();

            _sut = new BlaiseCatiApi(_catiServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstalledSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetInstalledSurveys(_serverParkName);

            //assert
            _catiServiceMock.Verify(v => v.GetInstalledSurveys(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstalledSurveys_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledSurveys(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstalledSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledSurveys(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstalledSurvey_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetInstalledSurvey(_instrumentName, _serverParkName);

            //assert
            _catiServiceMock.Verify(v => v.GetInstalledSurvey(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetInstalledSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledSurvey(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetInstalledSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledSurvey(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstalledSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstalledSurvey(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstalledSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstalledSurvey(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateDayBatch_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act
            _sut.CreateDayBatch(_instrumentName, _serverParkName, dayBatchDate);

            //assert
            _catiServiceMock.Verify(v => v.CreateDayBatch(_connectionModel, _instrumentName, 
                _serverParkName, dayBatchDate), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(string.Empty, _serverParkName, dayBatchDate));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CreateDayBatchForSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(null, _serverParkName, dayBatchDate));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(_instrumentName, string.Empty, dayBatchDate));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(_instrumentName, null, dayBatchDate));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDayBatch_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetDayBatch(_instrumentName, _serverParkName);

            //assert
            _catiServiceMock.Verify(v => v.GetDayBatch(_connectionModel, _instrumentName,
                _serverParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDayBatch(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDayBatch(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDayBatch(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDayBatch(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_SetSurveyDay_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act
            _sut.SetSurveyDay(_instrumentName, _serverParkName, surveyDay);

            //assert
            _catiServiceMock.Verify(v => v.SetSurveyDay(_connectionModel, _instrumentName, _serverParkName, surveyDay), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_SetSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDay(string.Empty, _serverParkName, surveyDay));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_SetSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDay(null, _serverParkName, surveyDay));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_SetSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDay(_instrumentName, string.Empty, surveyDay));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_SetSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDay(_instrumentName, null, surveyDay));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        ///
        ///
        [Test]
        public void Given_Valid_Arguments_When_I_Call_SetSurveyDays_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act
            _sut.SetSurveyDays(_instrumentName, _serverParkName, surveyDays);

            //assert
            _catiServiceMock.Verify(v => v.SetSurveyDays(_connectionModel, _instrumentName, _serverParkName, surveyDays), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_SetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDays(string.Empty, _serverParkName, surveyDays));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_SetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDays(null, _serverParkName, surveyDays));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_SetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SetSurveyDays(_instrumentName, string.Empty, surveyDays));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_SetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SetSurveyDays(_instrumentName, null, surveyDays));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyDays_Then_The_Expected_Result_Is_Returned()
        {
            var surveyDays = new List<DateTime>
            {
                new DateTime(2020, 12, 10),
                new DateTime(2020, 12, 11)
            };
            //arrange
            _catiServiceMock.Setup(p => p.GetSurveyDays(_connectionModel, _instrumentName, _serverParkName)).Returns(surveyDays);

            //act            
            var result = _sut.GetSurveyDays(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DateTime>>(result);
            Assert.AreEqual(surveyDays, result);
        }

        [Test]
        public void Given_There_Are_No_SurveyDays_In_The_Instrument_When_I_Call_GetSurveyDays_Then_A_Blank_List_Is_Returned()
        {
            var surveyDays = new List<DateTime>();
            //arrange
            _catiServiceMock.Setup(p => p.GetSurveyDays(_connectionModel, _instrumentName, _serverParkName)).Returns(surveyDays);

            //act            
            var result = _sut.GetSurveyDays(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<DateTime>>(result);
            Assert.AreEqual(surveyDays, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyDays(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyDays(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyDays(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyDays(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveSurveyDay_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act
            _sut.RemoveSurveyDay(_instrumentName, _serverParkName, surveyDay);

            //assert
            _catiServiceMock.Verify(v => v.RemoveSurveyDay(_connectionModel, _instrumentName, _serverParkName, surveyDay), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDay(string.Empty, _serverParkName, surveyDay));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDay(null, _serverParkName, surveyDay));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDay(_instrumentName, string.Empty, surveyDay));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveSurveyDay_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDay = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDay(_instrumentName, null, surveyDay));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveSurveyDays_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act
            _sut.RemoveSurveyDays(_instrumentName, _serverParkName, surveyDays);

            //assert
            _catiServiceMock.Verify(v => v.RemoveSurveyDays(_connectionModel, _instrumentName, _serverParkName, surveyDays), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDays(string.Empty, _serverParkName, surveyDays));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDays(null, _serverParkName, surveyDays));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveSurveyDays(_instrumentName, string.Empty, surveyDays));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveSurveyDays_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var surveyDays = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveSurveyDays(_instrumentName, null, surveyDays));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
