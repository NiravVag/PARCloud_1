using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Interfaces.Commands.Delete
{
    public class DeleteInterfaceEventCommand : IRequest<DeleteInterfaceEventResponse>, IMap<InterfaceEvent>
    {
        public int Id { get; set; }

        public string InterfaceType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteInterfaceEventCommand, Bin>();
        }
    }
}
