﻿using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Api.ServerPark
{
    public class BlaiseServerParkApiTests
    {
        private Mock<IServerParkService> _parkServiceMock;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseServerParkApi _sut;

        public BlaiseServerParkApiTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _parkServiceMock = new Mock<IServerParkService>();
            _sut = new BlaiseServerParkApi(
                _parkServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseServerParkApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseServerParkApi(new ConnectionModel()));
        }

        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "serverParkName";
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverParkName)).Returns(serverParkMock.Object);

            //act
            _sut.GetServerPark(serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, serverParkName), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_ServerPark_Is_Returned()
        {
            //arrange
            var serverParkName = "Park1";
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverParkName)).Returns(serverParkMock.Object);

            //act
            var result = _sut.GetServerPark(serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(serverParkMock.Object, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetServerPark(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerPark(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            //act
            _sut.GetServerParks();

            //assert
            _parkServiceMock.Verify(v => v.GetServerParks(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_ServerPark_Is_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            //act
            var result = _sut.GetServerParks();

            //assert
            Assert.NotNull(result);
            Assert.AreSame(serverParkItems, result);
        }

        [Test]
        public void When_I_Call_GetNamesOfServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetNamesOfServerParks();

            //assert
            _parkServiceMock.Verify(v => v.GetServerParkNames(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetNamesOfServerParks_Then_The_Expected_Server_Park_Names_Are_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };

            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(serverParksNames);

            //act
            var result = _sut.GetNamesOfServerParks();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.ServerParkExists(serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.ServerParkExists(_connectionModel, serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, serverParkName)).Returns(serverParkExists);

            //act
            var result = _sut.ServerParkExists(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ServerParkExists(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RegisterMachineOnServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";
            var machineName = "Gusty01";
            var logicalRootName = "Default";
            var roles = new List<string> { "Web", "Cati" };

            _parkServiceMock.Setup(p => p.RegisterMachineOnServerPark(_connectionModel, It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()));

            //act
            _sut.RegisterMachineOnServerPark(serverParkName, machineName, logicalRootName, roles);

            //assert
            _parkServiceMock.Verify(v => v.RegisterMachineOnServerPark(_connectionModel, serverParkName,
                machineName, logicalRootName, roles), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var machineName = "Gusty01";
            var logicalRootName = "Default";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(string.Empty,
                machineName, logicalRootName, roles));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var machineName = "Gusty01";
            var logicalRootName = "Default";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(null,
                machineName, logicalRootName, roles));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_MachineName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var logicalRootName = "Default";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                string.Empty, logicalRootName, roles));
            Assert.AreEqual("A value for the argument 'machineName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_MachineName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var logicalRootName = "Default";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                null, logicalRootName, roles));
            Assert.AreEqual("machineName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_LogicalRootName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var machineName = "Gusty01";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                machineName, string.Empty, roles));
            Assert.AreEqual("A value for the argument 'logicalRootName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_LogicalRootName_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var machineName = "Gusty01";
            var roles = new List<string> { "Web", "Cati" };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                machineName, null, roles));
            Assert.AreEqual("logicalRootName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_List_Of_Roles_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var machineName = "Gusty01";
            var logicalRootName = "Default";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                machineName, logicalRootName, new List<string>()));
            Assert.AreEqual("A value for the argument 'roles' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_List_Of_Roles_When_I_Call_RegisterMachineOnServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";
            var machineName = "Gusty01";
            var logicalRootName = "Default";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RegisterMachineOnServerPark(serverParkName,
                machineName, logicalRootName, null));
            Assert.AreEqual("roles", exception.ParamName);
        }
    }
}
