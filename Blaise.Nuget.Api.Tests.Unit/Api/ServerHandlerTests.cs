﻿using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using Blaise.Nuget.Api.Providers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class ServerHandlerTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly string _sourceServerName;
        private readonly string _sourceInstrumentName;
        private readonly string _sourceServerParkName;
        private readonly string _primaryKeyValue;
        private readonly string _destinationServerName;
        private readonly string _destinationInstrumentName;
        private readonly string _destinationServerParkName;

        private readonly ConnectionModel _sourceConnectionModel;
        private readonly ConnectionModel _destinationConnectionModel;

        private IBlaiseApi _sut;

        public ServerHandlerTests()
        {
            _sourceServerName = "Server1";
            _sourceInstrumentName = "Instrument1";
            _sourceServerParkName = "Park1";
            _primaryKeyValue = "Key1";

            _destinationServerName = "Server2";
            _destinationInstrumentName = "Instrument2";
            _destinationServerParkName = "Park2";

            _sourceConnectionModel = new ConnectionModel
            {
                ServerName = _sourceServerName
            };

            _destinationConnectionModel = new ConnectionModel
            {
                ServerName = _destinationServerName
            };
        }
        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _unityProviderMock = new Mock<IIocProvider>();

            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.GetConnectionModel())
                .Returns(_sourceConnectionModel);

            _unityProviderMock.Setup(u => u.Resolve<IDataService>()).Returns(_dataServiceMock.Object);

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _unityProviderMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_Valid_Arguments_ForServer_When_I_Call_CopyCase_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            //act
            _sut.CopyCase(_sourceConnectionModel,_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationConnectionModel, 
                _destinationInstrumentName, _destinationServerParkName);

            //assert
            Assert.AreEqual(_sourceServerName, _sourceConnectionModel.ServerName);
            _unityProviderMock.Verify(v => v.RegisterDependencies(_sourceConnectionModel), Times.Once);
            _dataServiceMock.Verify(v => v.GetDataRecord(_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
            _unityProviderMock.Verify(v => v.RegisterDependencies(_destinationConnectionModel), Times.Once);
            Assert.AreEqual(_destinationServerName, _destinationConnectionModel.ServerName);
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, _destinationInstrumentName, _destinationServerParkName));
        }

        [Test]
        public void Given_A_Null_SourceConnectionModel_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(null, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("The argument 'sourceConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, string.Empty, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, null, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceInstrumentName_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, string.Empty, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'sourceInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceInstrumentName_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, null, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("sourceInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceServerParkName_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, string.Empty,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'sourceServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceServerParkName_ForServer__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, null,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("sourceServerParkName", exception.ParamName);
        }
        [Test]
        public void Given_A_Null_DestinationConnectionModel_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                null, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("The argument 'destinationConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationInstrumentName_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, string.Empty, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'destinationInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationInstrumentName_ForServer__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, null, _destinationServerParkName));
            Assert.AreEqual("destinationInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationServerParkName_ForServer_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationServerParkName_ForServer__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, null));
            Assert.AreEqual("destinationServerParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Server_Arguments_ForServer_When_I_Call_MoveCase_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            //act
            _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationConnectionModel,
                _destinationInstrumentName, _destinationServerParkName);

            //assert
            _unityProviderMock.Verify(v => v.RegisterDependencies(_sourceConnectionModel), Times.Exactly(2));
            _dataServiceMock.Verify(v => v.GetDataRecord(_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
            Assert.AreEqual(_destinationConnectionModel.ServerName, _destinationConnectionModel.ServerName);
            _unityProviderMock.Verify(v => v.RegisterDependencies(_destinationConnectionModel));
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, _destinationInstrumentName, _destinationServerParkName));
            _dataServiceMock.Verify(v => v.RemoveDataRecord(_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_SourceConnectionModel_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(null, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("The argument 'sourceConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, string.Empty, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, null, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceInstrumentName_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, string.Empty, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'sourceInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceInstrumentName_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, null, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("sourceInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceServerParkName_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, string.Empty,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'sourceServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceServerParkName_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, null,
                _destinationConnectionModel, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("sourceServerParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DestinationConnectionModel_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                null, _destinationInstrumentName, _destinationServerParkName));
            Assert.AreEqual("The argument 'destinationConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationInstrumentName_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, string.Empty, _destinationServerParkName));
            Assert.AreEqual("A value for the argument 'destinationInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationInstrumentName_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, null, _destinationServerParkName));
            Assert.AreEqual("destinationInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationServerParkName_ForServer_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationServerParkName_ForServer__When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue,
                _sourceInstrumentName, _sourceServerParkName,
                _destinationConnectionModel, _destinationInstrumentName, null));
            Assert.AreEqual("destinationServerParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveCase_The_Correct_Services_Are_Called()
        {
            //arrange

            //act
            _sut.RemoveCase(_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName);

            //assert
            _dataServiceMock.Verify(v => v.RemoveDataRecord(_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(string.Empty, _sourceInstrumentName, _sourceServerParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(null, _sourceInstrumentName, _sourceServerParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase (_primaryKeyValue, string.Empty, _sourceServerParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(_primaryKeyValue, null, _sourceServerParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveCase(_primaryKeyValue, _sourceInstrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RemoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveCase(_primaryKeyValue, _sourceInstrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
