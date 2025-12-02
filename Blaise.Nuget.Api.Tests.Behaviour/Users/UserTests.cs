namespace Blaise.Nuget.Api.Tests.Behaviour.Users
{
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Api;
    using NUnit.Framework;

    public class UserTests
    {
        private readonly string _userName;

        private readonly string _password;

        private BlaiseUserApi _sut;

        public UserTests()
        {
            _userName = string.Empty;
            _password = string.Empty;
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
            // act
            var result = _sut.ValidateUser(_userName, _password);

            // assert
            Assert.That(result, Is.True);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_Name_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            // act
            var result = _sut.ValidateUser("meh", _password);

            // assert
            Assert.That(result, Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_Password_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            // act
            var result = _sut.ValidateUser(_userName, "meh");

            // assert
            Assert.That(result, Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Invalid_User_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            // act
            var result = _sut.ValidateUser("meh", "meh");

            // assert
            Assert.That(result, Is.False);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_User_When_I_Call_UpdateServerParks_Then_The_Users_Server_Parks_Are_Updated()
        {
            // arrange
            const string UserName = "jamie123";
            const string Password = "password123";
            const string Role = "DST";
            const string DefaultServerPark = "gusty";
            var serverParkList = new List<string> { "gusty" };
            _sut.AddUser(UserName, Password, Role, serverParkList, DefaultServerPark);

            const string CmaServerPark = "cma";
            serverParkList.Add(CmaServerPark);

            // act
            _sut.UpdateServerParks(UserName, serverParkList, CmaServerPark);
            var result = _sut.GetUser(UserName);

            // assert
            Assert.That(result.Name, Is.EqualTo(UserName));
            Assert.That(result.ServerParks.Count, Is.EqualTo(2));

            foreach (var serverPark in serverParkList)
            {
                Assert.That(result.ServerParks, Does.Contain(serverPark));
            }

            // clear down
            _sut.RemoveUser(UserName);
        }
    }
}
