using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using CCAuditLog = Par.CommandCenter.Domain.Model.AuditLog.AuditLog;



namespace Par.CommandCenter.Application.Handlers.AuditLog.Queries
{
    public class GetAuditLogResponse
    {
        public List<AuditLogModel> AuditLogs { get; set; }

    }

    public class AuditLogModel : IMap<CCAuditLog>
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string EntityType { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public Tenant Tenant { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CCAuditLog, AuditLogModel>();
        }
    }
}
