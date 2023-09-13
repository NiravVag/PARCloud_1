using MediatR;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Commands.UpdateNotificationDate
{
    public class UpdateNotificationDateCommand : IRequest<UpdateNotificationDateResponse>
    {
        public IEnumerable<HealthCheckVPN> VPNHealthChecks { get; set; }

        public IEnumerable<HealthCheckRouter> RouterHealthChecks { get; set; }

        public IEnumerable<HealthCheckController> ControllerHealthChecks { get; set; }

        public IEnumerable<HealthCheckServerOperation> ServerOperationHealthChecks { get; set; }

        public IEnumerable<HealthCheckInventoryInterface> InvenotryInterfaceHealthChecks { get; set; }

        public IEnumerable<HealthCheckOrderInterface> OrderInterfaceHealthChecks { get; set; }

        public bool SetNotificationDateNull { get; set; } = false;
    }
}
