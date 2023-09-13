using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using Par.Data.Model.CloudRouter;
//using Par.Data.Model.Router;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Upsert
{
    public class UpsertCloudRouterCommand : IRequest<UpsertCloudRouterResponse>, IMap<Router>
    {
        public string RouterAddress { get; set; }

        public IEnumerable<Port> Ports { get; set; }

        public int TenantId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertCloudRouterCommand, Router>();
        }
    }
}
