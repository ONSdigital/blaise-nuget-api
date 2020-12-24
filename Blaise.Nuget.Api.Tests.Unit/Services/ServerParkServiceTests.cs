﻿using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class ServerParkServiceTests
    {
        private Mock<IConnectedServerFactory> _connectionFactoryMock;

        private Mock<IServerPark> _serverParkMock;
        private Mock<IConnectedServer> _connectedServerMock;
        private Mock<IServerParkCollection> _serverParkCollectionMock;

        private readonly ConnectionModel _connectionModel;
        private string _serverParkName;

        private ServerParkService _sut;

        public ServerParkServiceTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup server parks
            _serverParkName = "tel";

            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns(_serverParkName);

            var serverParkItems = new List<IServerPark> { _serverParkMock.Object };

            _serverParkCollectionMock = new Mock<IServerParkCollection>();
            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //setup connection
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectedServerMock.Setup(c => c.ServerParks).Returns(_serverParkCollectionMock.Object);
            _connectedServerMock.Setup(c => c.GetServerPark(_serverParkName)).Returns(_serverParkMock.Object);

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_connectedServerMock.Object);

            //setup service under test
            _sut = new ServerParkService(_connectionFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_GetServerParkNames_Then_I_Get_An_IEnumerable_Of_Strings_Returned()
        {
            //act
            var result = _sut.GetServerParkNames(_connectionModel);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
        }

        [Test]
        public void Given_I_Call_GetServerParkNames_Then_I_Get_A_Correct_List_Of_ServerParkNames_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            serverParkMock1.Setup(s => s.Name).Returns("ServerParkName1");
            serverParkMock2.Setup(s => s.Name).Returns("ServerParkName2");

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //act
            var result = _sut.GetServerParkNames(_connectionModel).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains("ServerParkName1"));
            Assert.True(result.Contains("ServerParkName2"));
        }

        [Test]
        public void Given_No_ServerParks_When_I_Call_GetServerParkNames_Then_A_DataNotFoundException_Is_Thrown()
        {
            //arrange
            var serverParkItems = new List<IServerPark>();

            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetServerParkNames(_connectionModel));
            Assert.AreEqual("No server parks found", exception.Message);
        }

        [Test]
        public void Given_A_ServerPark_Exists_When_I_Call_GetServerPark_Then_The_Correct_Service_Is_Called()
        {
            //act
            _sut.GetServerPark(_connectionModel, _serverParkName);

            //assert
            _connectedServerMock.Verify(v => v.GetServerPark(_serverParkName), Times.Once);
        }

        [TestCase("TEL")]
        [TestCase("tel")]
        [TestCase("TEl")]
        [TestCase("tEl")]
        public void Given_A_ServerPark_Exists_When_I_Call_ServerParkExists_Then_True_Is_Returned(string serverParkName)
        {
            //act
            var result = _sut.ServerParkExists(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_A_ServerPark_Does_Not_Exist_When_I_Call_ServerParkExists_Then_False_Is_Returned()
        {
            //arrange
            var serverParkName = "NotFound";

            //act
            var result = _sut.ServerParkExists(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_A_ServerPark_Exists_When_I_Call_GetServerPark_Then_I_Get_A_ServerPark_Returned()
        {
            //act
            var result = _sut.GetServerPark(_connectionModel, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IServerPark>(result);
            Assert.AreSame(_serverParkMock.Object, result);
        }

        [Test]
        public void Given_A_ServerPark_Does_Not_Exist_When_I_Call_GetServerPark_Then_A_DataNotFoundException_Is_Thrown()
        {
            //arrange
            var serverParkName = "NotFound";

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetServerPark(_connectionModel, serverParkName));
            Assert.AreEqual($"Server park '{serverParkName}' not found", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetServerParks_Then_I_Get_A_Correct_List_Of_ServerParkNames_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();
            
            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //act
            var result = _sut.GetServerParks(_connectionModel).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<IServerPark>>(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Given_No_ServerParks_When_I_Call_GetServerParks_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            var serverParkItems = new List<IServerPark>();

            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetServerParks(_connectionModel));
            Assert.AreEqual("No server parks found", exception.Message);
        }

       [Test]
       public void Given_Valid_Arguments_I_Call_RegisterMachineOnServerPark_Then_The_Correct_Method_Is_Called()
        {
            //arrange
            var machineName = "Gusty01";
            var logicalRootName = "Default";
            var roles = new List<string> {"Web", "Cati"};
            var rolesArray = roles.ToArray();

            //act
            _sut.RegisterMachineOnServerPark(_connectionModel, _serverParkName, machineName,
                logicalRootName, roles);

            //assert
            _serverParkMock.Verify(v => v.AddMachine("Gusty01", "Default", rolesArray), Times.Once);
        }
    }
}
