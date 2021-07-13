using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Providers;
using NUnit.Framework;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Behaviour.Survey
{
    public class SurveyModeTests
    {
        private readonly BlaiseSurveyApi _sut;

        private const string ServerParkName = "LocalDevelopment";
        private const string InstrumentName = "OPN2102R";
        private readonly ConnectionModel _connectionModel;

        public SurveyModeTests()
        {
            _connectionModel = new ConnectionModel
            {
                ServerName = "localhost",
                UserName = "Root",
                Password = "Root",
                Binding = "http",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 90
            };

            _sut = new BlaiseSurveyApi();
        }

        //[Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_In_Cati_And_Cawi_Mode_When_I_Call_GetSurveyModes_The_Correct_Modes_Are_Returned()
        {
            //arrange
            var dataModelService = UnityProvider.Resolve<IDataModelService>();

            //act
            var result = (IDatamodel2)dataModelService.GetDataModel(_connectionModel, InstrumentName, ServerParkName);

            //assert
            Assert.IsNotNull(result.Modes);
            Assert.AreEqual(2, result.Modes);
        }
    }
}
