using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Commands.Reset
{
    public class ResetInterfaceEventCommand : IRequest<ResetInterfaceEventResponse>, IMap<Bin>
    {
        public int Id { get; set; }

        public string InterfaceType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ResetInterfaceEventCommand, Bin>();
        }
    }
}
