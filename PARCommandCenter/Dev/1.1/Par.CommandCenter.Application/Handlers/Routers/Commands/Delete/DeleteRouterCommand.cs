using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Delete
{
    public class DeleteRouterCommand : IRequest<DeleteRouterResponse>, IMap<Router>
    {
        public string Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteRouterCommand, Router>();
        }
    }
}
