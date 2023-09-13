using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class ProductController : BaseController
    {
        public ProductController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        [OpenApiOperation(nameof(GetAll), "Retreive all products information and for a tenant")]
        public async Task<IActionResult> GetAll([FromQuery] GetTenantProductsQuery query)
        {
            query.IncludeItems = true;

            var response = await Mediator.Send(query);

            return Ok(response);
        }        
    }
}
