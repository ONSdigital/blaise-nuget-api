
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class SurveyMetaServiceTests
    {
        private ConnectionModel _connectionModel;
        private string _instrumentName;
        private string _serverParkName;

        private Mock<IDataModelService> _dataModelServiceMock;

        private ISurveyMetaService _sut;

        [SetUp]
        public void SetupTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";

            _dataModelServiceMock = new Mock<IDataModelService>();

            _sut = new SurveyMetaService(_dataModelServiceMock.Object);
        }

        [Test]
        public void Given_An_Instrument_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetSurveyModes_The_Correct_Modes_Are_Returned()
        {
            //arrange
            const string mode1 = "CATI";
            var mode1Mock = new Mock<IMode>();
            mode1Mock.Setup(m => m.Name).Returns(mode1);

            const string mode2 = "CAWI";
            var mode2Mock = new Mock<IMode>();
            mode2Mock.Setup(m => m.Name).Returns(mode2);

            var modelList = new List<IMode> {mode1Mock.Object, mode2Mock.Object};

            var modeCollection = new Mock<IModeCollection>();
            modeCollection.Setup(m => m.GetEnumerator()).Returns(modelList.GetEnumerator());

            var dataModelMock = new Mock<IDatamodel2>();
            dataModelMock.Setup(dm => dm.Modes).Returns(modeCollection.Object);

            _dataModelServiceMock.Setup(dm => dm.GetDataModel(_connectionModel,
                _instrumentName, _serverParkName)).Returns(dataModelMock.Object);

            //act
            var result = _sut.GetSurveyModes(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.Contains("CAWI", result);
            Assert.Contains("CATI", result);
        }

        [Test]
        public void Given_An_Instrument_Has_No_DataModel_When_I_Call_GetSurveyModes_Then_An_Empty_List_Is_Returned()
        {
            //arrange
            _dataModelServiceMock.Setup(dm => dm.GetDataModel(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(null as IDatamodel);

            //act
            var result = _sut.GetSurveyModes(_connectionModel, _instrumentName, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
