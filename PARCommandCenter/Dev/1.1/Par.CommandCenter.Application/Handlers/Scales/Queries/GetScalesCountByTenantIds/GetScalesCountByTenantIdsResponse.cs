using AutoMapper;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Common;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesCountByTenantIds
{
    public class GetScalesCountByTenantIdsResponse
    { 
        public IEnumerable<TenantScalesCountModel> TenantScalesCount { get; set; }
    }

    public class TenantScalesCountModel
    {
        public int TenantId { get; set; }

        public int ScalesCount { get; set; }
    }
}
