using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Ping
{
    public class PingControllerCommand : IRequest<PingControllerResponse>, IMap<Controller>
    {
        public int TenantId { get; set; }

        public string Address { get; set; }

        public int? NetworkPort { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PingControllerCommand, Controller>();
        }
    }
}
