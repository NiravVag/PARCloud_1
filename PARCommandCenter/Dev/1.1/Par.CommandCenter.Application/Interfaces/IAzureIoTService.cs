using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    /// <summary>
    ///     A service used to  devices in Azure IoT hub.
    /// </summary>
    public interface IAzureIoTService
    {
        /// <summary>
        ///     Asynchronously add azure IoT device.
        /// </summary>
        /// <param name="deviceId">The azure IoT device Id.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<string> AddDeviceAsync(string deviceId, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously remove azure IoT device.
        /// </summary>
        /// <param name="deviceId">The azure IoT device Id.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<bool> RemoveDeviceAsync(string deviceId, CancellationToken cancellationToken);
    }
}
