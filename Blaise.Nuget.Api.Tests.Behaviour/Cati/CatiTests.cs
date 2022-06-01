using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;

        private const string ServerParkName = "LocalDevelopment";
        private const string questionnaireName = "DST2106Z";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_When_I_Call_GetInstalledQuestionnaires_The_Correct_Questionnaires_Are_Returned()
        {
            var result = _sut.GetInstalledQuestionnaires(ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            //Act
            _sut.SetSurveyDay(questionnaireName, ServerParkName, DateTime.Today);

            //Assert
            var result = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Multiple_SurveyDays_Are_Added_The_Survey_Days_Are_Returned()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //Act
            _sut.SetSurveyDays(questionnaireName, ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
            Assert.IsTrue(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_When_RemoveSurveyDay_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var surveyDay = DateTime.Today;

            _sut.SetSurveyDay(questionnaireName, ServerParkName, surveyDay);
            var surveyDays = _sut.GetSurveyDays(questionnaireName, ServerParkName);

            Assert.IsTrue(surveyDays.Contains(DateTime.Today));

            //Act
            _sut.RemoveSurveyDay(questionnaireName, ServerParkName, surveyDay);

            //Assert
            var result = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_Multiple_SurveyDays_When_RemoveSurveyDays_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };
            _sut.SetSurveyDays(questionnaireName, ServerParkName, daysToAdd);
            var surveyDays = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.IsTrue(surveyDays.Contains(DateTime.Today));
            Assert.IsTrue(surveyDays.Contains(DateTime.Today.AddDays(1)));

            //Act
            _sut.RemoveSurveyDays(questionnaireName, ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(questionnaireName, ServerParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
            Assert.IsFalse(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_A_SurveyDay_When_I_Call_GetDayBatch_The_DayBatch_Is_Created()
        {
            var result = _sut.CreateDayBatch(questionnaireName, ServerParkName, DateTime.Today, true);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_DayBatch_Entries_When_I_Call_GetDayBatch_The_Correct_Entries_Are_Returned()
        {
            var result = _sut.GetDayBatch(questionnaireName, ServerParkName);
            Assert.NotNull(result);
        }
    }
}
