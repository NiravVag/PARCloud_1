using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Delete;
using Par.CommandCenter.Application.Handlers.CloudRouters.Commands.Upsert;
using Par.CommandCenter.Application.Handlers.Routers.Commands.Create;
using Par.CommandCenter.Application.Handlers.Routers.Commands.Delete;
using Par.CommandCenter.Application.Handlers.Routers.Commands.DeleteService;
using Par.CommandCenter.Application.Handlers.Routers.Commands.GeneratePcRouterConfigFiles;
using Par.CommandCenter.Application.Handlers.Routers.Commands.Install;
using Par.CommandCenter.Application.Handlers.Routers.Commands.Register;
using Par.CommandCenter.Application.Handlers.Routers.Commands.Restart;
using Par.CommandCenter.Application.Handlers.Routers.Queries.GetAllRoutersUnassigned;
using Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class RouterController : BaseController
    {
        public readonly IApplicationDbContext _dbContext;
        public RouterController(IMediator mediator, IApplicationDbContext dbContext) : base(mediator)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetAllRoutersUnassigned), "Retreive All Routers that do not have a Tenant Id", "Retreive All Routers that do not have an assigned Tenant Id")]

        public async Task<IActionResult> GetAllRoutersUnassigned()
        {
            var response = await Mediator.Send(new GetAllRoutersUnassignedQuery());

            return Ok(response);
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetByTenantId), "Retreive All Routers for a tenant by Tenant Id", "Retreive All Routers for a tenant by Tenant Id")]
        [Route("{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var response = await Mediator.Send(new GetRoutersByTenantQuery() { TenantId = tenantId });

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Create), "Create router in Azure Iot Hub", "Create Router in Azure IoT Hub")]
        public async Task<IActionResult> Create([FromBody] CreateRouterCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(CreateCloudRouter), "Create Cloud Router (CDC) record in the Database and send a message to the config cdc queue ")]
        public async Task<IActionResult> CreateCloudRouter([FromBody] UpsertCloudRouterCommand command)
        {
            try
            {
                // var response =  await Mediator.Send(CreateUpdateCloudRouterRequest(model))

                var response = await Mediator.Send(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [OpenApiOperation(nameof(DeleteCloudRouter), "Logical Delete Cloud Router (CDC) record in the Database and send a message to the delete the Cloud Router CDC binding")]
        public async Task<IActionResult> DeleteCloudRouter([FromBody] DeleteCloudRouterCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                return Ok(response);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [OpenApiOperation(nameof(Register), "Register router in the database", "Register router in the database")]
        public async Task<IActionResult> Register([FromBody] RegisterRouterCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpPut]
        [OpenApiOperation(nameof(InstallCloudRouterService), "Install cloud router Windows Service on the Routers Application VM", "Install cloud router Windows Service on the Routers Application VM")]
        public async Task<IActionResult> InstallCloudRouterService([FromBody] InstallCloudRouterCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [OpenApiOperation(nameof(Delete), "Delete a router")]
        public async Task<IActionResult> Delete([FromBody] DeleteRouterCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [OpenApiOperation(nameof(DeleteService), "Delete the router service from the App Server")]
        public async Task<IActionResult> DeleteService([FromBody] DeleteRouterServiceCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpPost]
        [OpenApiOperation(nameof(Restart), "Restart the cloud router on the applicaiton server")]
        public async Task<IActionResult> Restart([FromBody] RestartRouterCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }


        [HttpGet]        
        [OpenApiOperation(nameof(DownloadPcRouterConfigZipFile), "Generate and download PC configuration files in one zip file.")]
        public async Task<FileContentResult> DownloadPcRouterConfigZipFile([FromQuery] GeneratePcRouterConfigFilesCommand command)
        {
            var response = await Mediator.Send(command);           

            return await Task.FromResult( new FileContentResult(response.CloudRouterConfigZipFile.Content, response.CloudRouterConfigZipFile.MimeType)
            {
                FileDownloadName = response.CloudRouterConfigZipFile.FileName + "." + response.CloudRouterConfigZipFile.FileExtension,// "excelfromByte1.txt",
                ////ContentType = response.CloudRouterConfigZipFile.MimeType,
                FileContents = response.CloudRouterConfigZipFile.Content,
            });
        }
    }

}
