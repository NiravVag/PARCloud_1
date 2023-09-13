using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Users.Commands.UpsertCurrentUserTenantSettings;
using Par.CommandCenter.Application.Handlers.Users.Queries.GetAllCommandCenterUsers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [OpenApiOperation(nameof(SaveUserTenantSettings), "Save user tenants application working setting")]
        public async Task<UpsertCurrentUserTenantSettingsResponse> SaveUserTenantSettings([FromBody] IEnumerable<int> tenantIds)
        {
            var response = await Mediator.Send(new UpsertCurrentUserTenantSettingsCommand() { TenantIds = tenantIds });

            return response;
        }


        [HttpGet]
        [OpenApiOperation(nameof(GetAllCommandCenterUsers), "Retreive Command Center Users")]
        public async Task<GetAllCommandCenterUsersResponse> GetAllCommandCenterUsers()
        {
            var response = await Mediator.Send(new GetAllCommandCenterUsersQuery());

            return response;
        }
    }
}
