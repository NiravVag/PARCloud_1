namespace Par.CommandCenter.CloudDeviceManagement.ConfigurationOptions
{
    public class AzureFunctionsConfigurationOptions
    {
        public string InstallCloudRouterFunctionURL { get; set; }

        public string FaRouterManagmentFunctionsKey { get; set; }

        public string RequestBinMeasureFunctionURL { get; set; }

        public string DeleteCloudRouterFunctionURL { get; set; }

        public string RestartCloudRouterFunctionURL { get; set; }

        public string PingCloudControllerFunctionURL { get; set; }

        public string FaRunCommandOnVmURL { get; set; }

        public string FaRunCommandOnVmFunctionKey { get; set; }

        public string FaRunCommandOnVmMachineName { get; set; }
    }
}
