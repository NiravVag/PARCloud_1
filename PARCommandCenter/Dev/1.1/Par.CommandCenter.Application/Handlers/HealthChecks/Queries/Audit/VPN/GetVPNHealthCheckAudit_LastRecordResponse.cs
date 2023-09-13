using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.HealthChecks.Queries.Audit.VPN
{
    public class GetVPNHealthCheckAudit_LastRecordResponse
    {
        public List<GetVPNHealthCheckAudit_LastRecordResponseModel> HealthChecksLastRecords { get; set; }
    }

    public class GetVPNHealthCheckAudit_LastRecordResponseModel : IMap<HealthCheckVPNAudit_LastRecord>
    {
        public int Audit_HealthCheckVPNId { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAction { get; set; }

        public int HealthCheckVPNId { get; set; }

        public int? TenantId { get; set; }

        public string Status { get; set; }

        public DateTime? NotificationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HealthCheckVPNAudit_LastRecord, GetVPNHealthCheckAudit_LastRecordResponseModel>();
        }
    }
}
