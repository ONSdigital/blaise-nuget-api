﻿using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Backup
{
    public class BackupInstrumentTests
    {
        private readonly BlaiseBackupApi _sut;

        public BackupInstrumentTests()
        {
            _sut = new BlaiseBackupApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Valid_Instrument_When_I_Call_BackupToFile_Then_An_Instrument_Is_Backed_Up_To_File()
        {
            //arrange
            var serverParkName = "LocalDevelopment";
            var instrumentName = "OPN2004A";
            var outputPath = @"d:\temp\backup";

            //act
            _sut.BackupSurveyToFile(serverParkName, instrumentName, outputPath);

            //arrange
        }
    }
}