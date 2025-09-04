namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class CatiTests
    {
        private const string ServerParkName = "LocalDevelopment";
        private const string QuestionnaireName = "DST2106Z";

        private readonly BlaiseCatiApi _sut;

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_When_I_Call_GetInstalledQuestionnaires_The_Correct_Questionnaires_Are_Returned()
        {
            var result = _sut.GetInstalledQuestionnaires(ServerParkName);
            Assert.That(result, Is.Not.Null);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(result, Is.Not.Null);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            // act
            _sut.SetSurveyDay(QuestionnaireName, ServerParkName, DateTime.Today);

            // assert
            var result = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(result.Contains(DateTime.Today), Is.True);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Multiple_SurveyDays_Are_Added_The_Survey_Days_Are_Returned()
        {
            // arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1),
            };

            // act
            _sut.SetSurveyDays(QuestionnaireName, ServerParkName, daysToAdd);

            // assert
            var result = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(result.Contains(DateTime.Today), Is.True);
            Assert.That(result.Contains(DateTime.Today.AddDays(1)), Is.True);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_When_RemoveSurveyDay_Is_Called_The_SurveyDays_Are_Removed()
        {
            // arrange
            var surveyDay = DateTime.Today;

            _sut.SetSurveyDay(QuestionnaireName, ServerParkName, surveyDay);
            var surveyDays = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);

            Assert.That(surveyDays.Contains(DateTime.Today), Is.True);

            // act
            _sut.RemoveSurveyDay(QuestionnaireName, ServerParkName, surveyDay);

            // assert
            var result = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(result.Contains(DateTime.Today), Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_Multiple_SurveyDays_When_RemoveSurveyDays_Is_Called_The_SurveyDays_Are_Removed()
        {
            // arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1),
            };
            _sut.SetSurveyDays(QuestionnaireName, ServerParkName, daysToAdd);
            var surveyDays = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(surveyDays.Contains(DateTime.Today), Is.True);
            Assert.That(surveyDays.Contains(DateTime.Today.AddDays(1)), Is.True);

            // act
            _sut.RemoveSurveyDays(QuestionnaireName, ServerParkName, daysToAdd);

            // assert
            var result = _sut.GetSurveyDays(QuestionnaireName, ServerParkName);
            Assert.That(result.Contains(DateTime.Today), Is.False);
            Assert.That(result.Contains(DateTime.Today.AddDays(1)), Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_A_SurveyDay_When_I_Call_GetDayBatch_The_DayBatch_Is_Created()
        {
            var result = _sut.CreateDayBatch(QuestionnaireName, ServerParkName, DateTime.Today, true);
            Assert.That(result, Is.Not.Null);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_DayBatch_Entries_When_I_Call_GetDayBatch_The_Correct_Entries_Are_Returned()
        {
            var result = _sut.GetDayBatch(QuestionnaireName, ServerParkName);
            Assert.That(result, Is.Not.Null);
        }
    }
}
