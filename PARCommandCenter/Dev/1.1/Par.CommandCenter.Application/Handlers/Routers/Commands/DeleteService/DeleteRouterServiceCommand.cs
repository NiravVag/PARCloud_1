using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.DeleteService
{
    public class DeleteRouterServiceCommand : IRequest<DeleteRouterServiceResponse>, IMap<Router>
    {
        public string Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteRouterServiceCommand, Router>();
        }
    }
}
