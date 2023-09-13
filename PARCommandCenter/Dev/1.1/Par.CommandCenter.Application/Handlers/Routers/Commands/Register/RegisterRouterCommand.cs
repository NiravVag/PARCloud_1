using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Register
{
    public class RegisterRouterCommand : IRequest<RegisterRouterResponse>, IMap<Router>
    {
        public int TenantId { get; set; }

        public string Address { get; set; }

        public string? ServiceName { get; set; }

        public string? ServiceDisplayName { get; set; }

        public bool IsPcRouter { get; set; } =  false;

        public string? ComputerName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterRouterCommand, Router>();
        }
    }
}
