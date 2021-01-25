﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;

        private const string ServerParkName = "LocalDevelopment";
        private const string SurveyName = "OPN2101A";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetInstalledSurveys_The_Correct_Instruments_Are_Returned()
        {
            var result = _sut.GetInstalledSurveys(ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            //Act
            _sut.SetSurveyDay(SurveyName, ServerParkName, DateTime.Today);

            //Assert
            var result = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_Multiple_SurveyDays_Are_Added_The_Survey_Days_Are_Returned()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //Act
            _sut.SetSurveyDays(SurveyName, ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
            Assert.IsTrue(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_A_SurveyDay_When_RemoveSurveyDay_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var surveyDay = DateTime.Today;

            _sut.SetSurveyDay(SurveyName, ServerParkName, surveyDay);
            var surveyDays = _sut.GetSurveyDays(SurveyName, ServerParkName);

            Assert.IsTrue(surveyDays.Contains(DateTime.Today));

            //Act
            _sut.RemoveSurveyDay(SurveyName, ServerParkName, surveyDay);

            //Assert
            var result = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_Has_Multiple_SurveyDays_When_RemoveSurveyDays_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };
            _sut.SetSurveyDays(SurveyName, ServerParkName, daysToAdd);
            var surveyDays = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.IsTrue(surveyDays.Contains(DateTime.Today));
            Assert.IsTrue(surveyDays.Contains(DateTime.Today.AddDays(1)));

            //Act
            _sut.RemoveSurveyDays(SurveyName, ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(SurveyName, ServerParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
            Assert.IsFalse(result.Contains(DateTime.Today.AddDays(1)));
        }
    }
}
