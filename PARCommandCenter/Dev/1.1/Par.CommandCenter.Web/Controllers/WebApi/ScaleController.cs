using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Bins.Commands.MeasureBin;
using Par.CommandCenter.Application.Handlers.Scales.Queries.GetScaleMeasureCounts;
using Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByIpAddress;
using Par.CommandCenter.Application.Handlers.Scales.Queries.GetScalesByTenant;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class ScaleController : BaseController
    {
        public ScaleController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retrieve All Scales for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetScalesByTenantQuery() { TenantId = tenantId });

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(RequestBinMeasurement), "Request bin measurement by bin Id")]
        [Route("{binId}")]
        public async Task<IActionResult> RequestBinMeasurement(int binId)
        {
            var response = await Mediator.Send(new MeasureBinCommand() { BinId = binId });

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetTenantScaleMeasureCounts), "Retrieve tenants scales measurements counts")]
        public async Task<IActionResult> GetTenantScaleMeasureCounts()
        {
            var response = await Mediator.Send(new GetScaleMeasureCountsQuery() { });

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByControllerId), "Retrieve Scales by ControllerId")]
        [Route("{controllerId}/{registeredScalesOnly}/{onlineScalesOnly}/{offlineScalesOnly}")]
        public async Task<IActionResult> GetByControllerId(int controllerId, bool registeredScalesOnly = false, bool onlineScalesOnly = false, bool offlineScalesOnly = false)
        {
            var response = await Mediator.Send(new GetScalesByControllerIdQuery()
            {
                ControllerId = controllerId,
                RegisteredScalesOnly = registeredScalesOnly,
                OnlineScalesOnly = onlineScalesOnly,
                OfflineScalesOnly = offlineScalesOnly
            }
            );

            return Ok(response);
        }
    }
}
