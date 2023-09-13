using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.AzureServiceBus.ConfigurationOptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Par.CommandCenter.AzureServiceBus.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using Par.CommandCenter.Domain.Model;
using System.Linq;

namespace Par.CommandCenter.AzureServiceBus.Services
{
    public class AzureServiceBusService : IAzureServiceBusService
    {
        const string TopicName = "batchjob";

        [NotNull]
        private readonly AzureServiceBusConfigurationOptions _configurationOptions;

        [NotNull]
        private readonly IAzureServiceBusFactory _messageBusFactory;

        [NotNull]
        private readonly ILogger _logger;

        public AzureServiceBusService([NotNull] IAzureServiceBusFactory messageBusFactory, [NotNull] AzureServiceBusConfigurationOptions configurationOptions, [NotNull] ILogger<AzureServiceBusService> logger)
        {
            this._messageBusFactory = messageBusFactory ?? throw new ArgumentNullException(nameof(messageBusFactory));
            this._configurationOptions = configurationOptions ?? throw new ArgumentNullException(nameof(configurationOptions));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<bool> PublishJobQueueEntry(Guid entryId, int jobId, int tenantId, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Publishing job queue entry");

            if (_configurationOptions.IntegrationServiceBus.ConnectionString == null)
                throw new Exception("Service bus connection string not defined");

            TopicClient topicClient = new TopicClient(_configurationOptions.IntegrationServiceBus.ConnectionString, TopicName);

            // Get message properties

            int messageLabel = 0;
            int sessionId = jobId;
            string messageBody = entryId.ToString();

            _logger.LogDebug("Publishing job queue entry {id}", messageBody);

            Message message = new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                SessionId = sessionId.ToString(),
                Label = messageLabel.ToString()
            };

            message.UserProperties.Add("TenantId", tenantId);

            // Must send message on separate thread wrapped in new transaction to prevent the TopicClient
            // from trying to join any existing transactions

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Suppress))
            {
                Task task = Task.Run(async () =>
                {
                    await topicClient.SendAsync(message);
                });

                task.Wait();
                scope.Complete();
            }

            await topicClient.CloseAsync().ConfigureAwait(false);

            return true;
        }

        public async Task<string> RestartCloudRouterAsync(string routerAddress, string machineName, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Start sending queue message to restart cloud router address {routerAddress}");

            if (string.IsNullOrWhiteSpace(routerAddress))
            {
                throw new ArgumentNullException(nameof(routerAddress));
            }

            if (string.IsNullOrWhiteSpace(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            // 1. Construct the service bus message.
            var dataParam = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("scriptParameters", $"parcloudrouter{routerAddress}")
            };
         
            String replyToSessionId = Guid.NewGuid().ToString();
            int commandTimeoutSeconds = 30;

            var restartMessage = GetRunCommandOnVMMessage("restartcloudrouter", machineName, dataParam, commandTimeoutSeconds, replyToSessionId);

            // 2. Get a client instance
            var client = _messageBusFactory.GetClient(
                _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                );

            _logger.LogInformation($"Sending message to VM {machineName}");

            // 3. Publish the message
            await client.PublishMessageAsync(restartMessage, machineName);

            _logger.LogInformation($"Awaiting response on session {replyToSessionId}");

            ServiceBusSessionReceiver receiver = await _messageBusFactory.GetSessionReceiver(
                 _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                , replyToSessionId);

            // the received message is a different type as it contains some service set properties
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(new TimeSpan(0, 0, commandTimeoutSeconds));

            return receivedMessage == null || receivedMessage.Body == null
                ? throw new Exception("Timeout " + commandTimeoutSeconds.ToString() + "s waiting for ReceiveMessageAsync")
                : Encoding.UTF8.GetString(receivedMessage.Body);
        }

        public async Task<PingCloudControllerResponse> PingControllerAsync(string controllerAddress, string machineName, CancellationToken cancellationToken, int? networkPort = null)
        {
            _logger.LogDebug($"Start sending queue message to ping controller {controllerAddress}");

            if (string.IsNullOrWhiteSpace(controllerAddress))
            {
                throw new ArgumentNullException(nameof(controllerAddress));
            }

            if (string.IsNullOrWhiteSpace(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            // 1. Construct the service bus message.
            var dataParam = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ip", controllerAddress)
            };

            String replyToSessionId = Guid.NewGuid().ToString();
            int commandTimeoutSeconds = 30;

            var pingMessage = GetRunCommandOnVMMessage("ping", machineName, dataParam, commandTimeoutSeconds, replyToSessionId);

            // 2. Get a client instance
            var client = _messageBusFactory.GetClient(
                _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                );

            _logger.LogInformation($"Sending message to VM {machineName}");

            // 3. Publish the message
            await client.PublishMessageAsync(pingMessage, machineName);

            _logger.LogInformation($"Awaiting response on session {replyToSessionId}");

            ServiceBusSessionReceiver receiver = await _messageBusFactory.GetSessionReceiver(
                 _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                , replyToSessionId);

            // the received message is a different type as it contains some service set properties
            ServiceBusReceivedMessage receivedMessage = 
                await receiver.ReceiveMessageAsync(new TimeSpan(0, 0, commandTimeoutSeconds)) 
                ?? throw new Exception($"Ping controller error {controllerAddress}. No response from the VM Agent.");

            var response = Encoding.UTF8.GetString(receivedMessage.Body);

            var responseObject = 
                JsonConvert.DeserializeObject<VmAgentCommandResult>(response) 
                ?? throw new Exception($"Ping controller error {controllerAddress}. Unknown error occurred.");

            ///Request was handled successfully
            if (responseObject.Result?.ToLower() != "ok")
            {
                return new PingCloudControllerResponse()
                {
                    PingSucceeded = false,
                    TcpTestSucceeded = false,
                    RemoteAddress = controllerAddress,
                    Message = responseObject.Message ?? "Error ping the cloud controller"
                };
            }            

            return new PingCloudControllerResponse()
            {
                PingSucceeded = true,
                RemoteAddress = controllerAddress,
                Message = $"Ping Succeed. {responseObject.Message}",
            };
        }

        public async Task<bool> DeleteCloudRouterAsync(string routerAddress, string machineName, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Start sending queue message to delete cloud router {routerAddress}");

            if (string.IsNullOrWhiteSpace(routerAddress))
            {
                throw new ArgumentNullException(nameof(routerAddress));
            }

            if (string.IsNullOrWhiteSpace(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            // 1. Construct the service bus message.
            var routerServiceName = $"parcloudrouter{routerAddress}";
            var dataParam = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("scriptParameters", routerServiceName)
            };

            String replyToSessionId = Guid.NewGuid().ToString();
            int commandTimeoutSeconds = 30;

            var pingMessage = GetRunCommandOnVMMessage("DeleteCloudRouter", machineName, dataParam, commandTimeoutSeconds, replyToSessionId);

            // 2. Get a client instance
            var client = _messageBusFactory.GetClient(
                _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                );

            _logger.LogInformation($"Sending message to VM {machineName}");

            // 3. Publish the message
            await client.PublishMessageAsync(pingMessage, machineName);

            _logger.LogInformation($"Awaiting response on session {replyToSessionId}");

            ServiceBusSessionReceiver receiver = await _messageBusFactory.GetSessionReceiver(
                 _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                , replyToSessionId);

            // the received message is a different type as it contains some service set properties
            ServiceBusReceivedMessage receivedMessage =
                await receiver.ReceiveMessageAsync(new TimeSpan(0, 0, commandTimeoutSeconds))
                ?? throw new Exception($"Delete cloud router error {routerAddress}. No response from the VM Agent.");

            var response = Encoding.UTF8.GetString(receivedMessage.Body);

            var responseObject =
                JsonConvert.DeserializeObject<VmAgentCommandResult>(response)
                ?? throw new Exception($"Delete cloud router error {routerAddress}. Unknown error occurred.");

            DeleteCloudRouterResponse responseContent;
            ///Request was handled successfully
            if (responseObject.Result?.ToLower() != "ok")
            {
                responseContent = new DeleteCloudRouterResponse()
                {
                    Success = false,
                    Message = responseObject.Message ?? "An error occured while deleting cloud router service"
                };

                return responseContent.Success;
            }

            responseContent = new DeleteCloudRouterResponse()
            {
                Success = true,
                Message = $"The router service delete operation completed successfully. {responseObject.Message}",
            };

            return responseContent.Success;
        }


        public async Task<string> InstallCloudRouterAsync(string routerAddress, string serviceName, string serviceDisplayName, string machineName, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Start sending queue message to install new cloud router address {routerAddress}");

            if (string.IsNullOrWhiteSpace(routerAddress))
            {
                throw new ArgumentNullException(nameof(routerAddress));
            }

            if (string.IsNullOrWhiteSpace(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            // 1. Construct the service bus message.
            var dataParam = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("scriptParameters", $"-routeraddress {routerAddress.Trim()}")
            };

            if (string.IsNullOrWhiteSpace(routerAddress))
            {
                dataParam = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(
                        "scriptParameters",
                        $"-routeraddress {routerAddress.Trim()} -servicename {serviceName.Trim()} -servicedisplayname {serviceDisplayName.Trim()}"
                        )
                };                
            }
            String replyToSessionId = Guid.NewGuid().ToString();
            int commandTimeoutSeconds = 30;

            var restartMessage = GetRunCommandOnVMMessage("InstallCloudRouter", machineName, dataParam, commandTimeoutSeconds, replyToSessionId);

            // 2. Get a client instance
            var client = _messageBusFactory.GetClient(
                _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                );

            _logger.LogInformation($"Sending message to VM {machineName}");

            // 3. Publish the message
            await client.PublishMessageAsync(restartMessage, machineName);

            _logger.LogInformation($"Awaiting response on session {replyToSessionId}");

            ServiceBusSessionReceiver receiver = await _messageBusFactory.GetSessionReceiver(
                 _configurationOptions.IntegrationServiceBus.ConnectionString,
                _configurationOptions.IntegrationServiceBus.RunCommandOnVmQueueName
                , replyToSessionId);

            // the received message is a different type as it contains some service set properties
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync(new TimeSpan(0, 0, commandTimeoutSeconds));

            return receivedMessage == null || receivedMessage.Body == null 
                ? throw new Exception("Timeout " + commandTimeoutSeconds.ToString() + "s waiting for ReceiveMessageAsync")
                : Encoding.UTF8.GetString(receivedMessage.Body);
        }

        private static Dictionary<string, string> GetRunCommandOnVMMessage(string command, string machineName, IEnumerable<KeyValuePair<string, string>> dataParam, int timeoutSeconds, string replyToSessionId)
        {
            var dataDictionary = new Dictionary<string, string>
            {   
                ["vm"] = machineName,
                ["command"] = command,
            };    
            
            if(dataParam != null && dataParam.Any()) {
                
                foreach (var item in dataParam)
                {
                    dataDictionary.Add(item.Key, item.Value);
                }
            }

            var controlDictionary = new Dictionary<string, string>
            {
                { "replyToSessionId", replyToSessionId },
                { "vm", machineName },
                { "timeoutSeconds", timeoutSeconds.ToString() }
            };

            var messageDictionary = new Dictionary<string, string>
            {
                { "control", JsonConvert.SerializeObject(controlDictionary) },
                { "data", JsonConvert.SerializeObject(dataDictionary) }
            };

            return messageDictionary;
        }        
    }
}

