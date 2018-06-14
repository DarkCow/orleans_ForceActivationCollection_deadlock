using System;
using System.Threading.Tasks;
using Interfaces;
using Orleans;

namespace Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        private readonly Random _random = new Random();

        public Task<int> GetARandomNumberAsync()
        {
            return Task.FromResult(_random.Next());
        }

        public Task<string> GetARandomStringAsync()
        {
            return Task.FromResult(_random.Next().ToString());
        }
    }
}
