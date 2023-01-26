using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Exceptions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;

        private const string _serverParkName = "LocalDevelopment";
        private const string _questionnaireName = "DST2106Z";
        private const string _questionnaireNameRemoveCaseData = "LMS2211_EW1";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void
            Given_An_Questionnaire_Is_Installed_When_I_Call_GetInstalledQuestionnaires_The_Correct_Questionnaires_Are_Returned()
        {
            var result = _sut.GetInstalledQuestionnaires(_serverParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            //Act
            _sut.SetSurveyDay(_questionnaireName, _serverParkName, DateTime.Today);

            //Assert
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
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
            _sut.SetSurveyDays(_questionnaireName, _serverParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
            Assert.IsTrue(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void
            Given_An_Questionnaire_Is_Installed_And_A_SurveyDay_When_RemoveSurveyDay_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var surveyDay = DateTime.Today;

            _sut.SetSurveyDay(_questionnaireName, _serverParkName, surveyDay);
            var surveyDays = _sut.GetSurveyDays(_questionnaireName, _serverParkName);

            Assert.IsTrue(surveyDays.Contains(DateTime.Today));

            //Act
            _sut.RemoveSurveyDay(_questionnaireName, _serverParkName, surveyDay);

            //Assert
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void
            Given_An_Questionnaire_Is_Installed_And_Has_Multiple_SurveyDays_When_RemoveSurveyDays_Is_Called_The_SurveyDays_Are_Removed()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };
            _sut.SetSurveyDays(_questionnaireName, _serverParkName, daysToAdd);
            var surveyDays = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
            Assert.IsTrue(surveyDays.Contains(DateTime.Today));
            Assert.IsTrue(surveyDays.Contains(DateTime.Today.AddDays(1)));

            //Act
            _sut.RemoveSurveyDays(_questionnaireName, _serverParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(_questionnaireName, _serverParkName);
            Assert.IsFalse(result.Contains(DateTime.Today));
            Assert.IsFalse(result.Contains(DateTime.Today.AddDays(1)));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Questionnaire_Has_A_SurveyDay_When_I_Call_GetDayBatch_The_DayBatch_Is_Created()
        {
            var result = _sut.CreateDayBatch(_questionnaireName, _serverParkName, DateTime.Today, true);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void
            Given_An_Questionnaire_Has_DayBatch_Entries_When_I_Call_GetDayBatch_The_Correct_Entries_Are_Returned()
        {
            var result = _sut.GetDayBatch(_questionnaireName, _serverParkName);
            Assert.NotNull(result);
        }

        [Test]
        public void
            Given_A_Questionnaire_Is_Installed_And_Has_Case_Information_When_I_Call_ClearCatiDataForQuestionnaire_The_Case_Information_Is_Removed()
        {
            var result = _sut.ClearCatiDataForQuestionnaire(_questionnaireNameRemoveCaseData, _serverParkName);
            Assert.Greater(result, 0);
        }

        [Test]
        public void
            Given_A_Questionnaire_Is_Installed_And_Has_Case_Information_When_I_Call_ClearCatiDataForQuestionnaire_And_The_Questionnaire_Is_Empty_A_DataNotFoundException_Is_Thrown()
        {
            try
            {
                var result = _sut.ClearCatiDataForQuestionnaire("", _serverParkName);
            }
            catch (ArgumentException ex)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void appt_clear()
        {
            var result = _sut.ClearAppointments(_questionnaireNameRemoveCaseData, _serverParkName);
        }
    }
}

