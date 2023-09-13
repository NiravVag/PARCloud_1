using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts;
using Par.CommandCenter.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GetAllProductsDto = Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts.GetAllProductsDto;
using IProductsWebAPIClient = Products.Web.API.Client.IProductsWebAPIClient;
using Item = Par.CommandCenter.Domain.Entities.Item;
using ProductNamespace = Par.CommandCenter.Domain.Entities.Product;
using Tenant = Par.CommandCenter.Domain.Entities.Tenant;

namespace Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProductss
{
    public class GetTenantProductsHandler : IRequestHandler<GetTenantProductsQuery, GetTenantProductsResponse>
    {
        private readonly ILogger<GetTenantProductsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductsWebAPIClient _productsWebAPIClient;

        private readonly ICurrentUserService _currentUserService;

        public GetTenantProductsHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IProductsWebAPIClient productsWebAPIClient, IMapper mapper, ILogger<GetTenantProductsHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _productsWebAPIClient = productsWebAPIClient;
            _currentUserService = currentUserService;
        }

        public async Task<GetTenantProductsResponse> Handle(GetTenantProductsQuery request, CancellationToken cancellationToken)
        {
            //var result = await _productsWebAPIClient.ProductsGETAsync(request.TenantId,false, true, request.PageNumber, request.PageSize).ConfigureAwait(false);

            IQueryable<ProductNamespace.Product> query1;
            if (request.TenantId == null || request.TenantId <= 0)
            {
                query1 = from p in _dbContext.Products
                         join pm in _dbContext.ProductManufacturers on p.ProductManufacturerId equals pm.Id into pmx
                         from pm in pmx.DefaultIfEmpty()
                         join pc in _dbContext.ProductCategories on p.ProductCategoryId equals pc.Id into pcx
                         from pc in pcx.DefaultIfEmpty()
                         join pcom in _dbContext.ProductUNSPSCCommodities on p.UNSPSCCommodityId equals pcom.Id into pcomx
                         from pcom in pcomx.DefaultIfEmpty()
                         join pcoms in _dbContext.ProductUNSPSCSegments on p.UNSPSCSegmentId equals pcoms.Id into pcomsx
                         from pcoms in pcomsx.DefaultIfEmpty()
                         join pcomf in _dbContext.ProductUNSPSCFamilies on p.UNSPSCFamilyId equals pcomf.Id into pcomfx
                         from pcomf in pcomfx.DefaultIfEmpty()
                         join pcomc in _dbContext.ProductUNSPSCClasses on p.UNSPSCClassId equals pcomc.Id into pcomcx
                         from pcomc in pcomcx.DefaultIfEmpty()
                         orderby p.ShortDescription
                         select new ProductNamespace.Product()
                         {
                             Id = p.Id,
                             ProductIdentifier = p.ProductIdentifier,
                             ShortDescription = p.ShortDescription,
                             LongDescription = p.LongDescription,
                             BrandName = p.BrandName,
                             ManufacturerNumber = p.ManufacturerNumber,
                             ProductCategoryId = p.ProductCategoryId,
                             ProductManufacturerId = p.ProductManufacturerId,

                             Manufacturer = pm,
                             Category = pc,
                             Commodity = pcom,
                             CommoditySegment = pcoms,
                             CommodityFamily = pcomf,
                             CommodityClass = pcomc,
                         };
            }
            else
            {

                query1 = from p in _dbContext.Products
                         join pm in _dbContext.ProductManufacturers on p.ProductManufacturerId equals pm.Id into pmx
                         from pm in pmx.DefaultIfEmpty()
                         join pc in _dbContext.ProductCategories on p.ProductCategoryId equals pc.Id into pcx
                         from pc in pcx.DefaultIfEmpty()
                         join pcom in _dbContext.ProductUNSPSCCommodities on p.UNSPSCCommodityId equals pcom.Id into pcomx
                         from pcom in pcomx.DefaultIfEmpty()
                         join pcoms in _dbContext.ProductUNSPSCSegments on p.UNSPSCSegmentId equals pcoms.Id into pcomsx
                         from pcoms in pcomsx.DefaultIfEmpty()
                         join pcomf in _dbContext.ProductUNSPSCFamilies on p.UNSPSCFamilyId equals pcomf.Id into pcomfx
                         from pcomf in pcomfx.DefaultIfEmpty()
                         join pcomc in _dbContext.ProductUNSPSCClasses on p.UNSPSCClassId equals pcomc.Id into pcomcx
                         from pcomc in pcomcx.DefaultIfEmpty()
                         join i in _dbContext.Items on p.Id equals i.ProductId into px
                         from i in px.DefaultIfEmpty()
                         where i.TenantId == request.TenantId
                         orderby p.BrandName
                         select new ProductNamespace.Product()
                         {
                             Id = p.Id,
                             BrandName = p.BrandName,
                             ManufacturerNumber = p.ManufacturerNumber,
                             ProductCategoryId = p.ProductCategoryId,
                             ProductIdentifier = p.ProductIdentifier,
                             ProductManufacturerId = p.ProductManufacturerId,
                             ShortDescription = p.ShortDescription,
                             LongDescription = p.LongDescription,
                             Manufacturer = pm,

                             Category = pc,
                             Commodity = pcom,
                             CommoditySegment = pcoms,
                             CommodityFamily = pcomf,
                             CommodityClass = pcomc,
                         };

            }


            if (request.IncludeItems)
            {
                var query3 = from i in _dbContext.Items
                             join t in _dbContext.Tenants on i.TenantId equals t.Id
                             select new
                             {
                                 i.Id,
                                 TenantId = t.Id,
                                 i.Number,
                                 i.ProductId,
                                 i.Name,
                                 i.VendorNumber,
                                 Tenant = new
                                 {
                                     t.Id,
                                     t.Name,
                                 }
                             };

                if (request.TenantId > 0)
                {
                    query3 = from i in query3
                             where i.TenantId == request.TenantId
                             select i;

                }

                query1 = from p in query1
                         join i in query3 on p.Id equals i.ProductId
                         select new ProductNamespace.Product()
                         {
                             Id = p.Id,
                             BrandName = p.BrandName,
                             ManufacturerNumber = p.ManufacturerNumber,
                             ProductCategoryId = p.ProductCategoryId,
                             ProductIdentifier = p.ProductIdentifier,
                             ProductManufacturerId = p.ProductManufacturerId,
                             ShortDescription = p.ShortDescription,
                             LongDescription = p.LongDescription,
                             Tenants = p.Tenants,

                             Manufacturer = p.Manufacturer,
                             Category = p.Category,
                             Commodity = p.Commodity,
                             CommoditySegment = p.CommoditySegment,
                             CommodityFamily = p.CommodityFamily,
                             CommodityClass = p.CommodityClass,

                             Items = query3.Where(x => x.ProductId == p.Id).Select(x => new Item()
                             {
                                 Id = x.Id,
                                 TenantId = x.TenantId,
                                 Number = x.Number,
                                 ProductId = x.ProductId,
                                 Name = x.Name,
                                 VendorNumber = x.VendorNumber,
                                 Tenant = new Tenant() { Id = x.Tenant.Id, Name = x.Tenant.Name }
                             }),
                         };

            }

            var result = await query1
                .ProjectTo<GetAllProductsDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            var response = new GetTenantProductsResponse()
            {
                Products = result.Items,
                PageNumber = result.PageNumber,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                HasPreviousPage = result.HasPreviousPage,
                HasNextPage = result.HasNextPage,
            };

            return response;
        }
    }
}
