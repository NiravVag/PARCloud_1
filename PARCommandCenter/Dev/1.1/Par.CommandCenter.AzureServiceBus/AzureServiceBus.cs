using Azure.Messaging.ServiceBus;
using Par.CommandCenter.AzureServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Par.CommandCenter.AzureServiceBus
{
    internal class AzureServiceBus : IAzureServiceBus
    {
        private readonly ServiceBusSender _serviceBusSender;

        internal AzureServiceBus(ServiceBusSender serviceBusSender)
        {
            this._serviceBusSender = serviceBusSender;
        }

        public async Task PublishMessageAsync<T>(T message, string? sessionId = null)
        {
            var jsonString = JsonSerializer.Serialize(message);

            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonString));

            if(sessionId != null)
            {
                serviceBusMessage.SessionId = sessionId;
            }

            await this._serviceBusSender.SendMessageAsync(serviceBusMessage);
        }     

        internal static IAzureServiceBus Create(ServiceBusSender sender)
        {
            return new AzureServiceBus(sender);
        }
    }
}
