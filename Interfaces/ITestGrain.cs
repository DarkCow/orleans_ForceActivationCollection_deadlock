using System.Threading.Tasks;
using Orleans;

namespace Interfaces
{
    public interface ITestGrain : IGrainWithIntegerKey
    {
        Task<int> GetARandomNumberAsync();
        Task<string> GetARandomStringAsync();
    }
}