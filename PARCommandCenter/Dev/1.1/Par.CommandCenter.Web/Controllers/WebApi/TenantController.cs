using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Tenants.Commands.Create;
using Par.CommandCenter.Application.Handlers.Tenants.Commands.UpsertTenantApplicationNotificationSettings;
using Par.CommandCenter.Application.Handlers.Tenants.Queries.GetAllTenants;
using Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsApplicationSetting;
using Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsByIds;
using Par.CommandCenter.Application.Handlers.Tenants.Queries.GetTenantsSummary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class TenantController : BaseController
    {
        public TenantController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetAll), "Retreive All Tenants")]
        public async Task<IActionResult> GetAll()
        {
            var response = await Mediator.Send(new GetAllTenantsQuery());

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(GetByIds), "Retrieve tenants by Ids that are supplied in the criteria")]
        public async Task<GetTenantsByIdsResponse> GetByIds([FromBody] GetTenantsByIdsQuery query)
        {
            var response = await Mediator.Send(query);

            return response;
        }

        [HttpPost]
        [OpenApiOperation(nameof(Create), "Create new Tenant")]
        public async Task<IActionResult> Create([FromBody] CreateTenantCommand model)
        {
            var response = await Mediator.Send(model);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(SaveTenantNotificationSettings), "Save tenants application notification settings")]
        public async Task<UpsertTenantApplicationNotificationSettingsResponse> SaveTenantNotificationSettings([FromBody] IEnumerable<int> tenantIds)
        {
            var response = await Mediator.Send(new UpsertTenantApplicationNotificationSettingsCommand() { TenantIds = tenantIds });

            return response;
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetAllTenantsApplicationSetting), "Retreive All Tenants Application Setting")]
        [Route("{activeOnly}")]
        public async Task<IActionResult> GetAllTenantsApplicationSetting(bool activeOnly = false)
        {
            var response = await Mediator.Send(new GetTenantsApplicationSettingQuery() { ActiveOnly = activeOnly });

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(TenantsSummary), "Retreive tenants information and summary of devices count, locations counts, and more. ")]
        public async Task<GetTenantsSummaryResponse> TenantsSummary()
        {
            var response = await Mediator.Send(new GetTenantsSummaryQuery());

            return response;
        }
    }
}
