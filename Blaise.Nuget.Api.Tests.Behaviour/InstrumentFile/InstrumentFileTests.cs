using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Tests.Behaviour.InstrumentFile
{
    public class InstrumentFileTests
    {
        private readonly BlaiseFileApi _sut;
        public InstrumentFileTests()
        {
            _sut = new BlaiseFileApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_UpdateInstrumentFileWithData_Then_The_Instrument_Is_Updated()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "opn2101a";
            const string instrumentFile = @"D:\Opn\Temp\OPN2101A.bpkg";

            CreateCases(100, instrumentName, serverParkName);

            //act && assert
            Assert.DoesNotThrow(() => _sut.UpdateInstrumentFileWithData(serverParkName, instrumentName, instrumentFile));

            //cleanup
            DeleteCasesInDatabase(instrumentName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_UpdateInstrumentFileWithSqlConnection_Then_The_Instrument_Is_Updated()
        {
            //arrange
            const string instrumentName = "opn2101a";
            const string instrumentFile = @"D:\Opn\Temp\OPN2101A.bpkg";

            //act && assert
            Assert.DoesNotThrow(() => _sut.UpdateInstrumentFileWithSqlConnection(instrumentName,
                instrumentFile));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_CreateSettingsDataInterfaceFile_Then_The_Interface_Is_Created()
        {
            //arrange
            const ApplicationType applicationType = ApplicationType.Cati;
            const string fileName = @"D:\OPN2101A.bcdi";

            //act && assert
            Assert.DoesNotThrow(() => _sut.CreateSettingsDataInterfaceFile(applicationType, fileName));
        }

        private static void CreateCases(int numberOfCases, string instrumentName, string serverParkName)
        {
            var blaiseCaseApi = new BlaiseCaseApi();
            var primaryKey = 90000;

            for (var count = 0; count < numberOfCases; count++)
            {
                var dictionary = new Dictionary<string, string> { { "serial_number", primaryKey.ToString() } };

                blaiseCaseApi.CreateCase(primaryKey.ToString(), dictionary, instrumentName, serverParkName);
                primaryKey++;
            }
        }

        private static void DeleteCasesInDatabase(string instrumentName, string serverParkName)
        {
            var blaiseCaseApi = new BlaiseCaseApi();

            var cases = blaiseCaseApi.GetCases(instrumentName, serverParkName);

            while (!cases.EndOfSet)
            {
                var primaryKey = blaiseCaseApi.GetPrimaryKeyValue(cases.ActiveRecord);

                blaiseCaseApi.RemoveCase(primaryKey, instrumentName, serverParkName);

                cases.MoveNext();
            }
        }
    }
}
