using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Macs;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Behaviour.Users
{
    public class UserTests
    {
        private readonly string _userName;
        private readonly string _password;

        private BlaiseUserApi _sut;

        public UserTests()
        {
            _userName = "";
            _password = "";
        }

        [SetUp]
        public void SetUpTests()
        {
            _sut = new BlaiseUserApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_A_Valid_User_When_I_Call_ValidateUser_Then_True_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser(_userName, _password);

            //assert
            Assert.IsNotNull(result);
            Assert.True(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_Name_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser("meh", _password);

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_Password_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser(_userName, "meh");

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {

            //act
            var result = _sut.ValidateUser("meh", "meh");

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_User_When_I_Call_Update_serverParks_Then_The_Users_ServerParks_Are_Updated()
        {
            //arrange
            const string userName = "jamie123";
            const string password = "password123";
            const string role = "DST";
            const string defaultServerPark = "gusty";
            var serverParkList = new List<string> { "gusty" };
            _sut.AddUser(userName, password, role, serverParkList, defaultServerPark);

            const string cmaServerPark = "cma";
            serverParkList.Add(cmaServerPark);

            //act
            _sut.UpdateServerParks(userName, serverParkList, cmaServerPark);
            var result = _sut.GetUser(userName);

            //assert
            Assert.AreEqual(userName, result.Name);
            Assert.AreEqual(2, result.ServerParks.Count);

            foreach (var serverPark in serverParkList)
            {
                Assert.IsTrue(result.ServerParks.Contains(serverPark));
            }

            //clear down
            _sut.RemoveUser(userName);
        }
    }
}
