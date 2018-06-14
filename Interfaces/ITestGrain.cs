using System.Threading.Tasks;

namespace Test
{
    public interface ITestGrain
    {
        Task<int> GetARandomNumber();
        Task<string> GetARandomString();
    }
}