using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.Create
{
    public class CreateTenantCommand : IRequest<CreateTenantResponse>, IMap<Tenant>
    {
        public string Name { get; set; }

        public string Acronym { get; set; }

        public short DefaultTimeZoneId { get; set; }

        public byte OrderBoxPercentage { get; set; }

        public bool IssueAdjustments { get; set; }

        public bool ParMobileAllowRememberMe { get; set; }

        public int CreatedUserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTenantCommand, Tenant>();
        }
    }
}
