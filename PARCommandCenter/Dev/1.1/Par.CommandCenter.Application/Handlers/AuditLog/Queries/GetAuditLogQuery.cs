using MediatR;
using Newtonsoft.Json;
using Par.CommandCenter.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Par.CommandCenter.Application.Handlers.AuditLog.Queries
{
    public class GetAuditLogQuery : IRequest<GetAuditLogResponse>
    {
        [Required]
        public bool AllUsers { get; set; }

        [Required]
        public bool AllTenants { get; set; }

        [Required]
        public AuditLogType AuditLogTypes { get; set; }

        [Required]
        public AuditLogEntityType AuditLogEntityTypes { get; set; }

        public int? UserId { get; set; }

        public int? TenantId { get; set; }

        [Required]
        public DateRangeFilterType DateRangeFilter { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
