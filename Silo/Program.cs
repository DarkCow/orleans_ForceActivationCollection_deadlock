using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Silo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using( var service = new Service())
            {
                await service.StartAsync();

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
        }
    }
}
