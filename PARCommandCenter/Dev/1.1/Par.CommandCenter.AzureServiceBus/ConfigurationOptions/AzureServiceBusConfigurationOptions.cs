namespace Par.CommandCenter.AzureServiceBus.ConfigurationOptions
{
    public class AzureServiceBusConfigurationOptions
    {
        public IntegrationServiceBus IntegrationServiceBus { get; set; }
    }

    public class IntegrationServiceBus
    {
        public string ConnectionString { get; set; }

        public string RunCommandOnVmQueueName { get; set; }
    }
}
