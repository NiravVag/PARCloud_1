using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class UpsertControllerCommand : IRequest<UpsertControllerResponse>, IMap<Controller>
    {
        public int? ControllerId { get; set; }

        public int TenantId { get; set; }

        public string PortName { get; set; }

        public int RouterId { get; set; }

        public int ControllerTypeId { get; set; }

        public string IpAddress { get; set; }

        public int? NetworkPort { get; set; }

        public string MACAddress { get; set; }

        public bool ParChargeBatch { get; set; }

        public bool ParChargeMode { get; set; }

        public bool Active { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertControllerCommand, Controller>();
        }
    }
}
