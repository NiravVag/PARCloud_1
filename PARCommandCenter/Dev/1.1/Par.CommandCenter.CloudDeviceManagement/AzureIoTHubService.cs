using JetBrains.Annotations;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.CloudDeviceManagement.ConfigurationOptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.CloudDeviceManagement
{
    public class AzureIoTHubService : IAzureIoTService
    {
        [NotNull]
        private readonly AzureIoTHubConfigurationOptions azureIoTHubSetting;
        private readonly RegistryManager registryManager = null;

        public AzureIoTHubService([NotNull] AzureIoTHubConfigurationOptions azureIoTSetting)
        {
            this.azureIoTHubSetting = azureIoTSetting ?? throw new ArgumentNullException(nameof(azureIoTSetting));
            registryManager = RegistryManager.CreateFromConnectionString(this.azureIoTHubSetting.AzureIoTHubConnectionString);
        }

        public async Task<string> AddDeviceAsync([NotNull] string deviceId, CancellationToken cancellationToken)
        {
            var newDevice = new Device(deviceId);
            newDevice.Authentication = new AuthenticationMechanism();
            newDevice.Authentication.Type = AuthenticationType.SelfSigned;
            newDevice.Authentication.X509Thumbprint = new X509Thumbprint();
            newDevice.Authentication.X509Thumbprint.PrimaryThumbprint = azureIoTHubSetting.DeviceX509Thumbprint;
            newDevice.Authentication.X509Thumbprint.SecondaryThumbprint = azureIoTHubSetting.DeviceX509Thumbprint;


            newDevice = await registryManager.AddDeviceAsync(newDevice, cancellationToken).ConfigureAwait(false);

            // Try to locate the newly added device.
            var device = await registryManager.GetDeviceAsync(newDevice.Id).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(device.Id))
            {
                throw new DeviceNotFoundException($"The device with Id {deviceId} is not found after registration DeviceId", new Exception("We couldn't locate the device after registration"));
            }

            return device.Id;
        }

        public async Task<bool> RemoveDeviceAsync([NotNull] string deviceId, CancellationToken cancellationToken)
        {
            // find the device.
            var device = await registryManager.GetDeviceAsync(deviceId).ConfigureAwait(false);
            if (device == null)
            {
                throw new DeviceNotFoundException($"The device with Id {deviceId} is not found");
            }

            await registryManager.RemoveDeviceAsync(device, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}
