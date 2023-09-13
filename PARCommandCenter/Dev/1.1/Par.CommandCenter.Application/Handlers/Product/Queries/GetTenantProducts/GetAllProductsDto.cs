using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.Product;
using System.Collections.Generic;
using ProductNamespace = Par.CommandCenter.Domain.Entities.Product;

namespace Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts
{
    public class GetAllProductsDto : IMap<ProductNamespace.Product>
    {
        public int Id { get; set; }

        public string ProductIdentifier { get; set; }

        public int ProductManufacturerId { get; set; }

        public string ManufacturerNumber { get; set; }

        public int ProductCategoryId { get; set; }

        public string BrandName { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public IEnumerable<Tenant> Tenants { get; set; }

        public IEnumerable<Item> Items { get; set; }


        public ProductManufacturer Manufacturer { get; set; }

        public ProductCategory Category { get; set; }

        public ProductUNSPSCCommodity Commodity { get; set; }

        public ProductUNSPSCSegment CommoditySegment { get; set; }

        public ProductUNSPSCFamily CommodityFamily { get; set; }

        public ProductUNSPSCClass CommodityClass { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductNamespace.Product, GetAllProductsDto>()
                 .ForMember(t => t.Tenants, opts => opts.MapFrom(m => m.Tenants))
                 .ForMember(t => t.Items, opts => opts.MapFrom(m => m.Items))
                 .ForMember(t => t.Manufacturer, opts => opts.MapFrom(m => m.Manufacturer))
                 .ForMember(t => t.Category, opts => opts.MapFrom(m => m.Category))
                 .ForMember(t => t.Commodity, opts => opts.MapFrom(m => m.Commodity))
                 .ForMember(t => t.CommoditySegment, opts => opts.MapFrom(m => m.CommoditySegment))
                 .ForMember(t => t.CommodityFamily, opts => opts.MapFrom(m => m.CommodityFamily))
                 .ForMember(t => t.CommodityClass, opts => opts.MapFrom(m => m.CommodityClass));
        }
    }
}
