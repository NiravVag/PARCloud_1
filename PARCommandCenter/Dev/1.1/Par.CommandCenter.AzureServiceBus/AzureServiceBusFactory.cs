using Azure.Messaging.ServiceBus;
using Par.CommandCenter.AzureServiceBus.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Par.CommandCenter.AzureServiceBus
{
    public class AzureServiceBusFactory : IAzureServiceBusFactory
    {
        private readonly object _lockObject = new object();

        private readonly ConcurrentDictionary<string, ServiceBusClient> _clients = new ConcurrentDictionary<string, ServiceBusClient>();

        private readonly ConcurrentDictionary<string, ServiceBusSender> _senders = new ConcurrentDictionary<string, ServiceBusSender>();

        public IAzureServiceBus GetClient(string connectionString, string senderName)
        {
            var key = $"{connectionString}-{senderName}";

            if (this._senders.ContainsKey(key) && !this._senders[key].IsClosed)
            {
                return AzureServiceBus.Create(this._senders[key]);
            }

            var client = this.GetServiceBusClient(connectionString);

            lock (this._lockObject)
            {
                if (this._senders.ContainsKey(key) && this._senders[key].IsClosed)
                {
                    if (this._senders[key].IsClosed)
                    {
                        this._senders[key].DisposeAsync().GetAwaiter().GetResult();
                    }

                    return AzureServiceBus.Create(this._senders[key]);
                }

                var sender = client.CreateSender(senderName);

                this._senders[key] = sender;
            }

            return AzureServiceBus.Create(this._senders[key]);
        }

        protected virtual ServiceBusClient GetServiceBusClient(string connectionString)
        {
            var key = $"{connectionString}";

            lock (this._lockObject)
            {
                if (this.ClientDoesntExistOrIsClosed(connectionString))
                {
                    var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
                    {
                        TransportType = ServiceBusTransportType.AmqpTcp
                    });

                    this._clients[key] = client;
                }

                return this._clients[key];
            }
        }

        public async Task<ServiceBusSessionReceiver> GetSessionReceiver(string connectionString, string senderName, string replySessionId)
        {
            var key = $"{connectionString}";

            if (this._clients.ContainsKey(key) && !this._clients[key].IsClosed)
            {   
                var sessionReceiver = await this._clients[key].AcceptSessionAsync(senderName, replySessionId);
                return sessionReceiver;
            }

            return null;
        }

        private bool ClientDoesntExistOrIsClosed(string connectionString)
        {
            return !this._clients.ContainsKey(connectionString) || this._clients[connectionString].IsClosed;
        }
    }
}
