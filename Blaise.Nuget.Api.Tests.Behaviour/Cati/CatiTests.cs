using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;

        private const string _ServerParkName = "LocalDevelopment";
        private const string _QuestionnaireName = "DST2106Z";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_When_I_Call_GetInstalledQuestionnaires_The_Correct_Questionnaires_Are_Returned()
        {
            var result = _sut.GetInstalledQuestionnaires(_ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            //Act
            _sut.SetSurveyDay(_QuestionnaireName, _ServerParkName, DateTime.Today);

            //Assert
            var result = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
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
            _sut.SetSurveyDays(_QuestionnaireName, _ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
            Assert.IsTrue(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_When_RemoveSurveyDay_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var surveyDay = DateTime.Today;

            _sut.SetSurveyDay(_QuestionnaireName, _ServerParkName, surveyDay);
            var surveyDays = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);

            Assert.IsTrue(surveyDays.Contains(DateTime.Today));

            //Act
            _sut.RemoveSurveyDay(_QuestionnaireName, _ServerParkName, surveyDay);

            //Assert
            var result = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
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
            _sut.SetSurveyDays(_QuestionnaireName, _ServerParkName, daysToAdd);
            var surveyDays = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
            Assert.IsTrue(surveyDays.Contains(DateTime.Today));
            Assert.IsTrue(surveyDays.Contains(DateTime.Today.AddDays(1)));

            //Act
            _sut.RemoveSurveyDays(_QuestionnaireName, _ServerParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(_QuestionnaireName, _ServerParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
            Assert.IsFalse(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_A_SurveyDay_When_I_Call_GetDayBatch_The_DayBatch_Is_Created()
        {
            var result = _sut.CreateDayBatch(_QuestionnaireName, _ServerParkName, DateTime.Today, true);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_DayBatch_Entries_When_I_Call_GetDayBatch_The_Correct_Entries_Are_Returned()
        {
            var result = _sut.GetDayBatch(_QuestionnaireName, _ServerParkName);
            Assert.NotNull(result);
        }
    }
}
