using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.HL7Servers.Commands.Upsert
{
    public class UpsertHL7ServerCommand : IRequest<UpsertHL7ServerResponse>, IMap<HL7Server>
    {
        public int? Id { get; set; }

        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertHL7ServerCommand, HL7Server>();
        }
    }
}
