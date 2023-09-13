using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.GeneratePcRouterConfigFiles
{
    public class GeneratePcRouterConfigFilesCommand : IRequest<GeneratePcRouterConfigFilesResponse>, IMap<Router>
    {
        public IEnumerable<int> ControllerIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GeneratePcRouterConfigFilesCommand, Router>();
        }
    }
}
