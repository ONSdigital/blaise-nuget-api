﻿using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class FluentApiCaseTests
    {
        private readonly ConnectionModel _connectionModel;

        public FluentApiCaseTests()
        {
            _connectionModel = new ConnectionModel();
        }

		[Test]
        public void Given_I_Use_With_Connection_And_A_Case_That_Exists_When_I_Call_Exists_True_Is_Returned()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            var result = sut
                .WithConnection(_connectionModel)
				.WithInstrument("OPN2004A")
                .WithServerPark("TEL")
                .Case
                .WithPrimaryKey("2951121")
                .Exists;

            //assert
			Assert.True(result);
        }

        [Test]
        public void Given_A_Case_That_Exists_When_I_Call_Exists_True_Is_Returned()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            var result = sut
                .WithConnection(_connectionModel)
				.WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey("11000011")
                .Exists;

            //assert
            Assert.True(result);
        }

		[TestCase(FieldNameType.Completed, true)]
        [TestCase(FieldNameType.Processed, true)]
        [TestCase(FieldNameType.WebFormStatus, true)]
        [TestCase(FieldNameType.CaseId, true)]
        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.NotSpecified, false)]
		public void Given_A_Case_That_Exists_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            var dataRecord = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey("11000011")
                .Get();

            var result = sut
                .Case
                .WithDataRecord(dataRecord)
                .HasField(fieldNameType);

			//assert
			Assert.AreEqual(exists, result);
        }

        [TestCase(110)]
        [TestCase(210)]
        [TestCase(310)]
        public void Given_A_Case_That_Exists_With_A_Hout_Value_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned(int hOut)
        {
            //arrange
            var primaryKey = "99000001";

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var payload = new Dictionary<string, string>
            {
                {"QHAdmin.HOut", hOut.ToString()}
            };

            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .WithData(payload)
                .Add();

            //act
            var dataRecord = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Get();

            var result = sut
                .Case
                .WithDataRecord(dataRecord)
                .HOut;

            //assert
            Assert.AreEqual(hOut, result);

            //cleanup
            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Remove();
        }

        [Test]
        public void Given_A_Case_That_Exists_Without_A_Hout_Value_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var primaryKey = "99000001";

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var payload = new Dictionary<string, string>
            {
                {"QID.CaseID", "54"}
            };

            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .WithData(payload)
                .Add();

            //act
            var dataRecord = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Get();

            var result = sut
                .Case
                .WithDataRecord(dataRecord)
                .HOut;

            //assert
            Assert.AreEqual(0, result);

            //cleanup
            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Remove();
        }

        [TestCase(WebFormStatusType.NotProcessed)]
		[TestCase(WebFormStatusType.Partial)]
		[TestCase(WebFormStatusType.Complete)]
        public void Given_A_Case_That_Exists_With_A_WebFormStatus_Value_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned(WebFormStatusType statusType)
        {
            //arrange
            var primaryKey = "99000001";

			IFluentBlaiseApi sut = new FluentBlaiseApi();

			var payload = new Dictionary<string, string>
			{
				{"WebFormStatus", $"{(int)statusType}"}
			};

            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .WithData(payload)
                .Add();

			//act
			var dataRecord = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Get();

            var result = sut
                .Case
                .WithDataRecord(dataRecord)
                .WebFormStatus;

            //assert
            Assert.AreEqual(statusType, result);

            //cleanup
            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Remove();
		}

        [Test]
        public void Given_A_Case_That_Exists_Without_A_WebFormStatus_Value_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var primaryKey = "99000001";

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var payload = new Dictionary<string, string>
            {
                {"QID.CaseID", "54"}
            };

            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .WithData(payload)
                .Add();

            //act
            var dataRecord = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Get();

            var result = sut
                .Case
                .WithDataRecord(dataRecord)
                .WebFormStatus;

            //assert
            Assert.AreEqual(WebFormStatusType.NotProcessed, result);

            //cleanup
            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Remove();
        }

        [Test]
        public void Given_Valid_Values_When_I_Call_Add_A_New_Case_Is_Added()
        {
            //arrange
            var primaryKey = "91000001";

			IFluentBlaiseApi sut = new FluentBlaiseApi();

            var payload = new Dictionary<string, string>
            {
		        {"QID.CaseID", "54"},
		        {"QID.Case_ID", "54"},
		        {"QID.Quota", null},
		        {"QID.Address", "4"},
		        {"QID.HHold", null},
		        {"QID.Issue", "1"},
		        {"QID.Mode", "TEL"},
		        {"qdatabag.Mode", "TEL"},
		        {"qdatabag.Quota", "11000"},
		        {"qdatabag.Address", "4"},
		        {"qdatabag.Hhld", "1"},
		        {"qdatabag.SurveyYear", "2019"},
		        {"qdatabag.Year", "2019"},
		        {"qdatabag.Month", "1"},
		        {"qdatabag.StageAttempt", "81D"},
		        {"qdatabag.TLA", "OPN"},
		        {"qdatabag.SubSample", "1"},
		        {"qdatabag.OldQuota", "11000"},
		        {"qdatabag.AddressNo", ""},
		        {"qdatabag.Prem1", "4 ONS STREET"},
		        {"qdatabag.Prem2", ""},
		        {"qdatabag.Prem3", ""},
		        {"qdatabag.Prem4", ""},
		        {"qdatabag.District", "ONSSHIRE"},
		        {"qdatabag.PostTown", "ONS TOWN"},
		        {"qdatabag.PostCode", "ON5 0NS"},
		        {"qdatabag.AddressKey", "1"},
		        {"qdatabag.MO", ""},
		        {"qdatabag.DivAddInd", "1"},
		        {"qdatabag.SampArea", "11000"},
		        {"qdatabag.Telno", "01234 567890"},
		        {"qdatabag.OSGridRef", ""},
		        {"qdatabag.LEA", "908"},
		        {"qdatabag.MajorStrat", ""},
		        {"qdatabag.GOR", "10"},
		        {"qdatabag.GORA", ""},
		        {"qdatabag.CountryCode", "E"},
		        {"qdatabag.GFFMU", "1"},
		        {"qdatabag.Wave", "5"},
		        {"qdatabag.PrevIssSerNo", "1019201101"},
		        {"qdatabag.OldSerial", ""},
		        {"qdatabag.Name", "Mr Person 4"},
		        {"qdatabag.PIDNo", ""},
		        {"qdatabag.ChkLet", "a"},
		        {"qdatabag.Rand", ""},
		        {"qdatabag.serial_number", primaryKey},
		        {"qdatabag.UAC1", "7896"},
		        {"qdatabag.OSWard", ""},
		        {"qdatabag.OSHlthAu", ""},
		        {"qdatabag.Ctry", ""},
		        {"qdatabag.PCon", ""},
		        {"qdatabag.TECLEC", ""},
		        {"qdatabag.TTWA", ""},
		        {"qdatabag.PCT", ""},
		        {"qdatabag.NUTS", "E05009186"},
		        {"qdatabag.PSED", "15UFFH03"},
		        {"qdatabag.WardC91", "16FBFH"},
		        {"qdatabag.WardO91", "15UFFH"},
		        {"qdatabag.Ward98", "15UFFH"},
		        {"qdatabag.StatsWard", "15UFFT"},
		        {"qdatabag.OACode", "E00095852"},
		        {"qdatabag.OAInd", "0"},
		        {"qdatabag.CASWard", "15UFFT"},
		        {"qdatabag.SOA1", "E01018976"},
		        {"qdatabag.DZone1", "E99999999"},
		        {"qdatabag.SOA2", "E02003948"},
		        {"qdatabag.URIndEW", ""},
		        {"qdatabag.URIndSc", ""},
		        {"qdatabag.DZone2", "E99999999"},
		        {"qdatabag.OAC", ""},
		        {"qdatabag.EER", ""},
		        {"qdatabag.HRO", ""},
		        {"qdatabag.OPNRef", "OPN11003"},
		        {"qdatabag.SampLFSPer", "1"},
		        {"qdatabag.SampAge", "79"},
		        {"qdatabag.SampTitle", "Mr"},
		        {"qdatabag.SampFName", "Person"},
		        {"qdatabag.SampSName", "4"},
		        {"qdatabag.RefDte", "21212"},
		        {"qdatabag.HInd", "1"},
		        {"qdatabag.UAC2", "4785"},
		        {"qdatabag.UAC3", "1456"},
		        {"qdatabag.Country", "1"},
		        {"qdatabag.Teleph", "1"},
		        {"qdatabag.ThisCQtr", ""},
		        {"qdatabag.ThisOQtr", ""},
		        {"qdatabag.LstHO", "20"},
		        {"qdatabag.LstNC", ""},
		        {"qdatabag.LstHRPId1", ""},
		        {"qdatabag.LstHRPID2", ""},
		        {"qdatabag.LstHRPID3", ""},
		        {"qdatabag.LstHRPID4", ""},
		        {"qdatabag.LstHRPID5", ""},
		        {"qdatabag.LstHRPID6", ""},
		        {"qdatabag.LstHRPID7", ""},
		        {"qdatabag.LstHRPID8", ""},
		        {"qdatabag.LstHRPID9", ""},
		        {"qdatabag.LstHRPID10", ""},
		        {"qdatabag.LstHRPID11", ""},
		        {"qdatabag.LstHRPID12", ""},
		        {"qdatabag.LstHRPID13", ""},
		        {"qdatabag.LstHRPID14", ""},
		        {"qdatabag.LstHRPID15", ""},
		        {"qdatabag.LstHRPID16", ""},
		        {"qdatabag.Telno2", ""}
			};

			//act
			sut
                .WithConnection(_connectionModel)
				.WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .WithData(payload)
                .Add();


            var result = sut
                .WithConnection(_connectionModel)
				.WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Exists;

            //assert
            Assert.True(result);

            //cleanup
            sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey(primaryKey)
                .Remove();
        }
    }
}
