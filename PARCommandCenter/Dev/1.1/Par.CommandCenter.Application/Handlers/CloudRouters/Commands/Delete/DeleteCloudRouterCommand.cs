using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Delete
{
    public class DeleteCloudRouterCommand : IRequest<DeleteCloudRouterResponse>, IMap<Router>
    {

        public string RouterAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteCloudRouterCommand, Router>();
        }
    }
}
