using System;
using System.Threading.Tasks;
using Interfaces;
using Mri.Web.Common.Test;
using Orleans;
using Xunit;

namespace IntegrationTest
{
    [Collection( "Fixture" )]
    public class TestGrainTest
    {
        private readonly SiloFactory _siloFactory;
        private readonly IClusterClient _clusterClient;

        public TestGrainTest(SiloFactory siloFactory)
        {
            _siloFactory = siloFactory;

            _clusterClient = _siloFactory.CreateClientAsync().Result;
        }

        [Fact]
        public async Task TestRandomNumberAsync()
        {
            var grain = _clusterClient.GetGrain<ITestGrain>( 123 );
            var randomNumber = await grain.GetARandomNumberAsync();

            Assert.IsType(typeof(int), randomNumber); // useless test because reasons
        }

        [Fact]
        public async Task TestRandomStringAsync()
        {
            var grain = _clusterClient.GetGrain<ITestGrain>( 123 );
            var randomString = await grain.GetARandomStringAsync();

            Assert.IsType(typeof(string), randomString); // useless test because reasons
        }
    }
}
