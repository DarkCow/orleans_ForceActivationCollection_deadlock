using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using Silo;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mri.Web.Common.Test
{
    public class SiloFactory : IDisposable
    {
        private readonly Service _service;

        public SiloFactory()
        {
            _service = new Service();

            _service.StartAsync().Wait();
        }

        public async Task<IClusterClient> CreateClientAsync()
        {
            IClientBuilder clientBuilder = new ClientBuilder();

            clientBuilder = clientBuilder
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(Interfaces.ITestGrain).Assembly).WithReferences();
                } );

            var client = clientBuilder.Build();
            await client.Connect();

            return client;
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }

    [CollectionDefinition( "Fixture" )]
    public class EveryFixture : ICollectionFixture<SiloFactory>
    {
    }
}
