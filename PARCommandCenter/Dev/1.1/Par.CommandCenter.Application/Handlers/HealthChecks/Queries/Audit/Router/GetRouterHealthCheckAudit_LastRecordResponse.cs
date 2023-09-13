using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.Audit.Router
{
    public class GetRouterHealthCheckAudit_LastRecordResponse
    {
        public List<GetRouterHealthCheckAudit_LastRecordResponseModel> HealthChecksLastRecords { get; set; }
    }

    public class GetRouterHealthCheckAudit_LastRecordResponseModel : IMap<HealthCheckRouterAudit_LastRecord>
    {
        public int Audit_HealthCheckRouterId { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAction { get; set; }

        public int HealthCheckRouterId { get; set; }

        public int RouterId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset? LastCommunication { get; set; }

        public DateTimeOffset? LastReboot { get; set; }

        public DateTime? NotificationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckRouterAudit_LastRecord, GetRouterHealthCheckAudit_LastRecordResponseModel>();
        }
    }
}
