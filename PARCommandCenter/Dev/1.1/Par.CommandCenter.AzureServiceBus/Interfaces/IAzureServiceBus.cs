using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Par.CommandCenter.AzureServiceBus.Interfaces
{
    public interface IAzureServiceBus   
    {
        Task PublishMessageAsync<T>(T message, string? sessionId = null);        
    }
}
