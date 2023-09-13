using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Install
{
    public class InstallCloudRouterCommand : IRequest<InstallCloudRouterResponse>, IMap<Router>
    {
        public string Address { get; set; }

        public string ServiceName { get; set; }

        public string ServiceDisplayName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InstallCloudRouterCommand, Router>();
        }
    }
}
