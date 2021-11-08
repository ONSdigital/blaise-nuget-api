using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Cati.Runtime;
using StatNeth.Blaise.API.Cati.Specification;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class CatiServiceTests
    {
        private Mock<IRemoteCatiManagementServerProvider> _catiProviderMock;
        private Mock<ISurveyService> _surveyServiceMock;

        private Mock<IRemoteCatiManagementServer> _catiManagementServerMock;
        private Mock<ISurveyDayCollection> _surveyDayCollection;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;
        private readonly DateTime _surveyDay;

        private CatiService _sut;

        public CatiServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();

            _surveyDay = DateTime.Today;
        }

        [SetUp]
        public void SetUpTests()
        {
            var surveyDayMock = new Mock<ISurveyDay>();
            surveyDayMock.Setup(d => d.Date).Returns(_surveyDay);
            var surveyDays = new List<ISurveyDay> { surveyDayMock.Object };

            _surveyDayCollection = new Mock<ISurveyDayCollection>();
            _surveyDayCollection.Setup(s => s.GetEnumerator()).Returns(surveyDays.GetEnumerator());

            _catiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(
                It.IsAny<Guid>()).CreateDaybatch(It.IsAny<DateTime>()));
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(
                It.IsAny<Guid>()).Specification.SurveyDays).Returns(_surveyDayCollection.Object);

            _catiProviderMock = new Mock<IRemoteCatiManagementServerProvider>();
            _catiProviderMock.Setup(r => r.GetCatiManagementForServerPark(_connectionModel, _serverParkName))
                .Returns(_catiManagementServerMock.Object);

            _surveyServiceMock = new Mock<ISurveyService>();
            _surveyServiceMock.Setup(ss => ss.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName))
                .Returns(_instrumentId);

            //setup service under test
            _sut = new CatiService(_catiProviderMock.Object, _surveyServiceMock.Object);
        }

        [Test]
        public void Given_Surveys_Are_Installed_When_I_Call_GetInstalledSurveys_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string instrument1 = "OPN2004a";
            const string instrument2 = "OPN2010a";
            var installedSurveys = new Dictionary<string, Guid>
            {
                {instrument1, Guid.NewGuid()},
                {instrument2, Guid.NewGuid()}
            };

            var surveyMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);
            _surveyServiceMock.Setup(s => s.GetSurvey(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(surveyMock.Object);

            //act
            _sut.GetInstalledSurveys(_connectionModel, _serverParkName);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _surveyServiceMock.Verify(v => v.GetSurvey(_connectionModel, instrument1, _serverParkName), Times.Once);
            _surveyServiceMock.Verify(v => v.GetSurvey(_connectionModel, instrument2, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Surveys_Are_Installed_When_I_Call_GetInstalledSurveys_Then_An_Correct_List_Is_Returned()
        {
            //arrange
            const string instrument1 = "OPN2004a";
            const string instrument2 = "OPN2010a";
            var installedSurveys = new Dictionary<string, Guid>
            {
                {instrument1, Guid.NewGuid()},
                {instrument2, Guid.NewGuid()}
            };

            var surveyMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);
            _surveyServiceMock.Setup(s => s.GetSurvey(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(surveyMock.Object);

            //act
            var result = _sut.GetInstalledSurveys(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<ISurvey>>(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Given_No_Surveys_Are_Installed_When_I_Call_GetInstalledSurveys_Then_An_Empty_List_Is_Returned()
        {
            //arrange
            var installedSurveys = new Dictionary<string, Guid>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);

            //act
            var result = _sut.GetInstalledSurveys(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<ISurvey>>(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Given_A_Survey_Is_Installed_When_I_Call_GetInstalledSurvey_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            const string instrument1 = "OPN2004a";
            const string instrument2 = "OPN2010a";
            var installedSurveys = new Dictionary<string, Guid>
            {
                {instrument1, Guid.NewGuid()},
                {instrument2, Guid.NewGuid()},
                {_instrumentName, Guid.NewGuid()}
            };

            var surveyMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);
            _surveyServiceMock.Setup(s => s.GetSurvey(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(surveyMock.Object);

            //act
            _sut.GetInstalledSurvey(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _surveyServiceMock.Verify(v => v.GetSurvey(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _surveyServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_A_Survey_Is_Installed_When_I_Call_GetInstalledSurvey_Then_The_Correct_Survey_Is_Returned()
        {
            //arrange
            const string instrument1 = "OPN2004a";
            const string instrument2 = "OPN2010a";
            var installedSurveys = new Dictionary<string, Guid>
            {
                {instrument1, Guid.NewGuid()},
                {instrument2, Guid.NewGuid()},
                {_instrumentName, Guid.NewGuid()}
            };

            var surveyMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);
            _surveyServiceMock.Setup(s => s.GetSurvey(_connectionModel, _instrumentName, _serverParkName))
                .Returns(surveyMock.Object);

            //act
            var result = _sut.GetInstalledSurvey(_connectionModel, _instrumentName, _serverParkName);

            //assert
            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);
            Assert.AreSame(surveyMock.Object, result);
        }

        [Test]
        public void Given_A_Survey_Is_Not_Installed_When_I_Call_GetInstalledSurvey_Then_A_DataNotFound_Exception_Is_Thrown()
        {
            //arrange
            const string instrument1 = "OPN2004a";
            const string instrument2 = "OPN2010a";
            var installedSurveys = new Dictionary<string, Guid>
            {
                {instrument1, Guid.NewGuid()},
                {instrument2, Guid.NewGuid()}
            };

            var surveyMock = new Mock<ISurvey>();

            _catiManagementServerMock.Setup(c => c.GetInstalledSurveys()).Returns(installedSurveys);
            _surveyServiceMock.Setup(s => s.GetSurvey(_connectionModel, It.IsAny<string>(), _serverParkName))
                .Returns(surveyMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetInstalledSurvey(_connectionModel, _instrumentName, _serverParkName));
            Assert.AreEqual($"No survey called '{_instrumentName}' was found on server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_A_Survey_Day_Exists_For_Day_Batch_Date_When_I_Call_CreateDayBatch_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var dayBatchDate = _surveyDay;
            var caseIds = new List<string> { "90001", "90002" };

            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_instrumentId, null))
                .Returns(caseIds);
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_instrumentId))
                .Returns(dayBatchDate);

            //act
            _sut.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId)
                .CreateDaybatch(dayBatchDate), Times.Once);
        }

        [Test]
        public void Given_A_Survey_Day_Does_Not_Exist_For_Day_Batch_Date_When_I_Call_CreateDayBatch_Then_A_DataNotFoundException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = _surveyDay.AddDays(1);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate));
            Assert.AreEqual($"A survey day does not exist for the required daybatch date '{dayBatchDate.Date}'", exception.Message);
        }

        [Test]
        public void Given_A_Day_Batch_Exists_When_I_Call_GetDayBatch_Then_The_Correct_DayBatchModel_Is_Returned()
        {
            //arrange
            var dayBatchDate = _surveyDay;
            var caseIds = new List<string> { "90001", "90002" };

            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_instrumentId, ""))
                .Returns(caseIds);
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_instrumentId))
                .Returns(dayBatchDate);

            //act
            var result = _sut.GetDayBatch(_connectionModel, _instrumentName, _serverParkName);

            //assert
           Assert.IsNotNull(result);
           Assert.IsInstanceOf<DayBatchModel>(result);
           Assert.AreEqual(dayBatchDate, result.DayBatchDate);
           Assert.AreEqual(caseIds, result.CaseIds);
        }

        [Test]
        public void Given_A_Day_Batch_Does_Not_Exist_When_I_Call_GetDayBatch_Then_Null_Is_Returned()
        {
            //arrange
            _catiManagementServerMock.Setup(cm => cm.GetKeysInDaybatch(_instrumentId, ""))
                .Returns(new List<string>());
            _catiManagementServerMock.Setup(cm => cm.GetDaybatchDate(_instrumentId))
                .Returns(null);

            //act
            var result = _sut.GetDayBatch(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void Given_I_Call_SetSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.SetSurveyDay(_connectionModel, _instrumentName, _serverParkName, DateTime.Today);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).Specification.SurveyDays.AddSurveyDay(DateTime.Today), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).SaveSpecification(), Times.Once);
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
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).Specification.SurveyDays.AddSurveyDays(surveyDays), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).SaveSpecification(), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetSurveyDays_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetSurveyDays(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);

            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).Specification.SurveyDays, Times.Once);
        }

        [Test]
        public void Given_I_Call_RemoveSurveyDay_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveSurveyDay(_connectionModel, _instrumentName, _serverParkName, DateTime.Today);

            //assert
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).Specification.SurveyDays.RemoveSurveyDay(DateTime.Today), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).SaveSpecification(), Times.Once);
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
            _catiProviderMock.Verify(v => v.GetCatiManagementForServerPark(_connectionModel, _serverParkName),
                Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).Specification.SurveyDays.RemoveSurveyDays(surveyDays), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentId).SaveSpecification(), Times.Once);
        }
    }
}
