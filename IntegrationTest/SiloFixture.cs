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

        public async Task ResetAsync()
        {
            using(var clusterClient = await CreateClientAsync())
            {
                var managementGrain = clusterClient.GetGrain<IManagementGrain>(0);

                // In my use case for integration testing, I need to deactivate all grains in the cluster
                // to get a clean slate between certain tests. One way to do that is to Stop/Start the entire cluster
                // but that's pretty slow
                //
                // The below method works on the first invocation, but stalls on the second invocation.
                await managementGrain.ForceActivationCollection(TimeSpan.Zero);
            }
        }

        public async Task<IClusterClient> CreateClientAsync()
        {
            IClientBuilder clientBuilder = new ClientBuilder();

            clientBuilder = clientBuilder
                .UseLocalhostClustering()
                .AddSimpleMessageStreamProvider("SMSProvider")
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(Interfaces.ITestGrain).Assembly).WithReferences();
                });

            var client = clientBuilder.Build();
            await client.Connect();

            return client;
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }

    [CollectionDefinition("Fixture")]
    public class EveryFixture : ICollectionFixture<SiloFactory>
    {
    }
}
