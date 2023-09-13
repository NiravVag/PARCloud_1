using Par.CommandCenter.Domain.Entities.Email;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using Par.CommandCenter.Domain.Model;
using System.Collections.Generic;

namespace Par.CommandCenter.Notifications
{
    public class Notification
    {
        public Tenant Tenant { get; set; }

        /// <summary>
        ///     Gets or sets the user friendly name in the email.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the application reference URL for this notification.
        /// </summary>
        public string ReferenceURL
        {
            get;
            set;
        }

        public string TemplateName { get; set; }

        public IEnumerable<EmailAddress> Emails { get; set; }

        public string EmailSubject { get; set; }


        public IEnumerable<HealthCheckVPN> HealthCheckVPNs { get; set; }

        ////public IEnumerable<HealthCheckRouter> HealthCheckRouters { get; set; }

        public IEnumerable<HealthCheckController> HealthCheckControllers { get; set; }

        public IEnumerable<HealthCheckServerOperation> HealthCheckServerOperations { get; set; }

        public IEnumerable<HealthCheckInterface> HealthCheckInterfaces { get; set; }
    }
}
