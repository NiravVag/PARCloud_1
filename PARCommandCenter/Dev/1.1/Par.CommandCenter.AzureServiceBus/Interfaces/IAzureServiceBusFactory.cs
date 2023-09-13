using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace Par.CommandCenter.AzureServiceBus.Interfaces
{
    public interface IAzureServiceBusFactory
    {
        IAzureServiceBus GetClient(string connectionString, string sender);

        Task<ServiceBusSessionReceiver> GetSessionReceiver(string connectionString, string sender, string replySessionId);
    }
}
