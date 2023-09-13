using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsSummary
{
    public class GetTenantsSummaryResponse
    {
        public IEnumerable<TenantSummaryModel> TenantsSummary { get; set; }
    }


    public class TenantVmSummaryModel
    {
        public int Id { get; set; }

        public string ComputerName { get; set; }

        public int TotalRouters { get; set; }
    }


    public class TenantSummaryModel : IMap<Tenant>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TotalFacilities { get; set; }

        public int TotalLocations { get; set; }

        public int TotalRouters { get; set; }

        public int TotalControllers { get; set; }

        public int ReplenishControllers { get; set; }


        public int ChargeControllers { get; set; }

        public int TotalScales { get; set; }

        public int OfflineRouters { get; set; }

        public int OfflineControllers { get; set; }

        public int OfflineScales { get; set; }

        public IEnumerable<TenantVmSummaryModel> AzureVmsSummary { get; set; }

        public string AzureVmsSummaryDisplay => AzureVmsSummary.Any() ? string.Join(", ", this.AzureVmsSummary.Select(x => x.ComputerName + " (" + x.TotalRouters + ")")) : string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tenant, TenantSummaryModel>();          
        }
    }
}
