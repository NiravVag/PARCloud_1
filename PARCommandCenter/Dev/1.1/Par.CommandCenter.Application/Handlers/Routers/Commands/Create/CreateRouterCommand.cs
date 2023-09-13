using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Create
{
    public class CreateRouterCommand : IRequest<CreateRouterResponse>, IMap<Router>
    {
        public string Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRouterCommand, Router>();
        }
    }
}
