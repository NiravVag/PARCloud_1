using Par.CommandCenter.Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    /// <summary>
    ///     A service used to publish messages to the azure service bus.
    /// </summary>
    public interface IAzureServiceBusService
    {
        /// <summary>
        ///     Asynchronously publish Job Queue Entry to the azure service bus.
        /// </summary>
        /// <param name="entryId">The job queue Id.</param>       
        /// /// <param name="jobId">The job Id.</param>       
        /// /// <param name="tenantId">The job queue Id.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<bool> PublishJobQueueEntry(Guid entryId, int jobId, int tenantId,  CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously send a messge to the run command on VM azure service bus queue to restart a router.
        /// </summary>
        /// <param name="routerAddress">The cloud router address.</param>       
        /// <param name="machineName">The machine/VM name where the CR is running on.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<string> RestartCloudRouterAsync(string routerAddress, string machineName, CancellationToken cancellationToken);


        /// <summary>
        ///     Asynchronously ping cloud controller from inside Virtual Machine.
        /// </summary>
        /// <param name="controllerAddress">The controller address from the database.</param>       
        /// <param name="machineName">The machine name where the cloud router is running.</param>  
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<PingCloudControllerResponse> PingControllerAsync(string controllerAddress, string machineName, CancellationToken cancellationToken, int? networkPort = null);

        /// <summary>
        ///     Asynchronously delete cloud router on the routers application Virtual Machine.
        /// </summary>
        /// <param name="routerAddress">The router address from the database.</param>       
        /// <param name="vmMachineName">The VM machine name where the cloud router is running from the database.</param>  
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<bool> DeleteCloudRouterAsync(string routerAddress, string vmMachineName, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously install cloud router on the routers application Virtual Machine.
        /// </summary>        
        /// <param name="routerAddress">The router address from the database.</param>       
        /// <param name="serviceName">The windows service name for the cloud router to install.</param>       
        /// <param name="serviceDisplayName">The windows service display name for the cloud router to install.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<string> InstallCloudRouterAsync(string routerAddress, string serviceName, string serviceDisplayName, string machineName, CancellationToken cancellationToken);
    }
}
