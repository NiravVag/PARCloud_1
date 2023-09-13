using MediatR;
using Par.CommandCenter.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts
{
    public class GetTenantProductsQuery : IRequest<GetTenantProductsResponse>
    {
        public int? TenantId { get; set; }

        public bool IncludeItems { get; set; } = false;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
