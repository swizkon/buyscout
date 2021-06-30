using System.Threading.Tasks;

namespace BuyScout.API
{
    public interface ITestHubClient
    {
        Task ReceiveMessage(string user, string message);

        Task Broadcast(string user, string message);
    }
}