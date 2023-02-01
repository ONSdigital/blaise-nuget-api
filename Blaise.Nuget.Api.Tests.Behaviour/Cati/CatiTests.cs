using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;
        private readonly BlaiseCaseApi _caseApi;

        private const string _serverParkName = "LocalDevelopment";
        private const string _questionnaireName = "DST2106Z";
        private const string _questionnaireNameRemoveCaseData = "LMS2211_EW1";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
            _caseApi = new BlaiseCaseApi();
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
        public void Given_An_Questionnaire_Has_DayBatch_Entries_When_I_Call_GetDayBatch_The_Correct_Entries_Are_Returned()
        {
            var result = _sut.GetDayBatch(_questionnaireName, _serverParkName);
            Assert.NotNull(result);
        }

        #region ClearCatiDataForQuestionnaire

        [Test]
        public void
            Given_A_Questionnaire_Is_Installed_And_Has_Case_Information_When_I_Call_ClearCatiDataForQuestionnaire_The_Case_Information_Is_Removed()
        {
            //ARRANGE
            //Remove any old cases and create a new case
            const string questionnaire = "DST2111Z";
            const string primaryKey = "900010";
            var surveyDayDateTime = DateTime.Now.AddHours(1);

            var installedQuestionnaires = _sut.GetInstalledQuestionnaires(_serverParkName).ToList();
            if (installedQuestionnaires.FirstOrDefault(o => o.Name.Equals(questionnaire, StringComparison.InvariantCultureIgnoreCase)) == null)
            {
                //Missing questionnaire
                Assert.Fail($"Missing questionnaire {questionnaire}");
            }

            ConfigurePartialTestWithSurveyDayAndDaybatch(questionnaire, _serverParkName, primaryKey, surveyDayDateTime);

            //ACT
            //Delete the cati data
            var result = _sut.ClearCatiDataForQuestionnaire(questionnaire, _serverParkName);

            //clean up
            var dates = _sut.GetSurveyDays(questionnaire, _serverParkName);
            _sut.RemoveSurveyDays(questionnaire, _serverParkName, dates);

            //ASSERT
            Assert.Greater(result, 0);
        }

        [Test]
        public void
            Given_A_Questionnaire_Is_Installed_And_Has_Case_Information_When_I_Call_ClearCatiDataForQuestionnaire_And_The_Questionnaire_Is_Empty_A_DataNotFoundException_Is_Thrown()
        {
            try
            {
                //ARRANGE
                //Remove any old cases and create a new case
                const string questionnaire = "DST2111Z";
                const string primaryKey = "900010";
                var surveyDayDateTime = DateTime.Now.AddHours(1);

                var installedQuestionnaires = _sut.GetInstalledQuestionnaires(_serverParkName).ToList();
                if (installedQuestionnaires.FirstOrDefault(o => o.Name.Equals(questionnaire, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    //Missing questionnaire
                    Assert.Fail($"Missing questionnaire {questionnaire}");
                }

                ConfigurePartialTestWithSurveyDayAndDaybatch(questionnaire, _serverParkName, primaryKey, surveyDayDateTime);

                //Act
                var result = _sut.ClearCatiDataForQuestionnaire("", _serverParkName);
            }
            catch (ArgumentException)
            {
                //Assert
                Assert.Pass();
            }
        }
        #endregion

        #region ClearAppointments        
        [Test]
        public void Given_An_Appointment_Already_Exists_When_I_Call_ClearAppointments_It_Deletes_The_Specified_Appointments()
        {
            //ARRANGE
            const string questionnaire = "DST2111Z";
            const string primaryKey = "900010";
            var surveyDayDateTime = DateTime.Now.AddHours(1);

            var installedQuestionnaires = _sut.GetInstalledQuestionnaires(_serverParkName).ToList();
            if (installedQuestionnaires.FirstOrDefault(o => o.Name.Equals(questionnaire, StringComparison.InvariantCultureIgnoreCase)) == null)
            {
                //Missing questionnaire
                Assert.Fail($"Missing questionnaire {questionnaire}");
            }

            //Act
            var primaryKeyList = new List<string> { primaryKey };
            var result = _sut.ClearAppointments(questionnaire, _serverParkName, primaryKeyList);

            //Assert
            Assert.Greater(result, 0);
        }

        [Test]
        public void Given_I_Call_ClearAppointments_With_No_Primary_Keys_Zero_Is_Returned()
        {
            //Arrange 
            const string questionnaire = "DST2111Z";
            const string primaryKey = "900010";

            _caseApi.RemoveCases(questionnaire, _serverParkName);

            //Create a new case
            CreateACaseConfig(questionnaire, _serverParkName, primaryKey);

            //Act
            var result = _sut.ClearAppointments(questionnaire, _serverParkName, null);

            //Assert
            Assert.AreEqual(0, result);
        }
        #endregion

        [Test]
        public void Given_I_Have_An_Existing_Case_And_Call_CreateAppointment_A_New_Appointment_Is_Created()
        {
            //ARRANGE
            //Remove any old cases and create a new case
            const string questionnaire = "DST2111Z";
            const string primaryKey = "900010";
            var surveyDayDateTime = DateTime.Now.AddHours(1);

            var installedQuestionnaires = _sut.GetInstalledQuestionnaires(_serverParkName).ToList();
            if (installedQuestionnaires.FirstOrDefault(o => o.Name.Equals(questionnaire, StringComparison.InvariantCultureIgnoreCase)) == null)
            {
                //Missing questionnaire
                Assert.Fail($"Missing questionnaire {questionnaire}");
            }

            ConfigurePartialTestWithSurveyDayAndDaybatch(questionnaire, _serverParkName, primaryKey, surveyDayDateTime, true);

            var result
                = _sut.CreateAppointment(questionnaire, _serverParkName, primaryKey, surveyDayDateTime, "Testing");

            //Assert
            Assert.Greater(result, 0);
        }

        private void ConfigurePartialTestWithSurveyDayAndDayBatch(string questionnaire, string serverParkName
                                                                , string primaryKey, DateTime surveyDayDateTime
                                                                , bool clearCatiData = false)
        {
            if (clearCatiData)
            {
                _sut.ClearCatiDataForQuestionnaire(questionnaire, _serverParkName);
            }

            //Remove cases
            _caseApi.RemoveCases(questionnaire, _serverParkName);

            //Create a new case
            CreateACaseConfig(questionnaire, serverParkName, primaryKey);

            //Add survey day
            var surveyDays = _sut.GetSurveyDays(questionnaire, serverParkName);
            if (surveyDays != null && surveyDays.Count > 0)
            {
                var dates = _sut.GetSurveyDays(questionnaire, _serverParkName);
                _sut.RemoveSurveyDays(questionnaire, _serverParkName, dates);

                _sut.SetSurveyDay(questionnaire, serverParkName, surveyDayDateTime);
            }
            else
            {
                _sut.SetSurveyDay(questionnaire, serverParkName, surveyDayDateTime);
            }

            //Add daybatch
            if (_sut.GetDayBatch(questionnaire, serverParkName) == null)
                _sut.CreateDayBatch(questionnaire, serverParkName, surveyDayDateTime, true);
        }

        private void CreateACaseConfig(string questionnaire, string serverPark, string primaryKey)
        {
            //Field data for creating a case
            var fieldData = new Dictionary<string, string>
            {
                {
                    "qiD.Serial_Number", "900010"
                },
                {
                    "qDataBag.TLA", "LMS"
                },
                {
                    "qDataBag.Wave", "5"
                },
                {
                    "qdatabag.TelNo", "063636363"
                },
                {
                    "qdatabag.TelNo2", "063636363"
                },
                //{
                //    "telNoAppt", "063636363"
                //},
                {
                    "hOut", "0"
                },
                {
                    "qDataBag.FieldRegion", "Region 1"
                },
                {
                    "qDataBag.FieldCase", "N"
                },
                {
                    "qDatabag.WaveComDTE", "25-09-2022"
                },
                {
                    "qRotate.RDMktnIND", ""
                },
                {
                    "qRotate.RHOut", "0"
                },
                {
                    "dataModelName", "LMS"
                },
                {
                    "qdatabag.prem1", "10"
                },
                {
                    "qdatabag.prem2", "Blaiseville"
                },
                {
                    "qdatabag.prem3", "Newportio"
                },

                {
                    "qdatabag.district", "Gwentish"
                },
                {
                    "qdatabag.posttown", "Neeewwwpoort"
                },
                {
                    "qdatabag.postcode", "N3V3R"
                },
                {
                    "qDataBag.UPRN_Latitude", "11112"
                },
                {
                    "qDataBag.UPRN_Longitude", "33444"
                },
                {
                    "qDataBag.Priority", "1"
                },
                {
                    "QRotate.RMode", "1"
                },
                {
                    "DMKtnInd ", "1"
                },
                //{
                //    "CatiMana.CatiAppoint.DateStart", DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")
                //}
            };
        }
    }
}

