using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Providers
{
    public class DataInterfaceProviderTests
    {
        private DataInterfaceProvider _sut;

        public DataInterfaceProviderTests()
        {
            _sut = new DataInterfaceProvider(new DataInterfaceFactory());
        }

        [Test]
        public void Given_I_Call_CreateCatiSqlDataInterface_Then_A_Cati_Interface_File_Is_Created()
        {
            //arrange

            //act
            _sut.CreateCatiSqlDataInterface("test");
            //assert
        }
    }
}
