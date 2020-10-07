﻿using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class BackupTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _destinationFilePath;
        private readonly string _bucketName;
        private readonly string _folderPath;
        private readonly string _sourceFolderPath;

        private FluentBlaiseApi _sut;

        public BackupTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _destinationFilePath = "FilePath";
            _bucketName = "OpnBucket";
            _folderPath = "FolderPath";
            _sourceFolderPath = "SourceFolderPath";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithConnection_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            //_sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithFile_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            //_sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'ToPath' step needs to be called with a valid value, check that the step has been called with a valid value for the destination file path", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupFilesToBucket(_destinationFilePath, 
                    _bucketName, null), Times.Once);
        }

        [Test]
        public void Given_ToBucket_AndToFolder_Steps_Are_Setup_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName, _folderPath);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupFilesToBucket(_destinationFilePath,
                _bucketName, _folderPath), Times.Once);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithConnection_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            //_sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_ToPath_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            //_sut.Survey.ToPath(_destinationFilePath);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'ToPath' step needs to be called with a valid value, check that the step has been called with a valid value for the destination file path", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_For_Settings_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.Settings.WithSourceFolder(_sourceFolderPath);
            _sut.Settings.ToBucket(_bucketName, _folderPath);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupFilesToBucket(_sourceFolderPath,
                _bucketName, _folderPath), Times.Once);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_For_Settings_But_WithSourceFolder_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.Settings.WithSourceFolder(_sourceFolderPath);
            _sut.Settings.ToBucket(_bucketName, _folderPath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithSourceFolder' step needs to be called with a valid value, check that the step has been called with a valid source path", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_For_Settings_But_ToBucket_Has_Not_Been_Called_When_I_Call_Backup_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Settings.WithSourceFolder(_sourceFolderPath);
           // _sut.Settings.ToBucket(_bucketName, _folderPath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'ToBucket' step needs to be called with a valid value, check that the step has been called with a valid bucket name", exception.Message);
        }

        [Test]
        public void Given_I_Do_Not_Specify_Surveys_or_Settings_When_I_Call_Backup_Then_Correct_Services_Then_A_NotSupportedException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("Backup functionality is only available for surveys and settings", exception.Message);
        }
    }
}
