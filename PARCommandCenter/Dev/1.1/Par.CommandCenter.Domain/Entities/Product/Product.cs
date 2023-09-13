using System.Collections.Generic;

namespace Par.CommandCenter.Domain.Entities.Product
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductIdentifier { get; set; }

        public int? ProductManufacturerId { get; set; }

        public string ManufacturerNumber { get; set; }

        public int ProductCategoryId { get; set; }

        public int? UNSPSCCommodityId { get; set; }

        public int UNSPSCSegmentId { get; set; }

        public int UNSPSCFamilyId { get; set; }

        public int UNSPSCClassId { get; set; }

        public string BrandName { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public IEnumerable<Tenant>? Tenants { get; set; }

        public IEnumerable<Item>? Items { get; set; }

        public ProductManufacturer? Manufacturer { get; set; }

        public ProductCategory? Category { get; set; }

        public ProductUNSPSCCommodity? Commodity { get; set; }

        public ProductUNSPSCSegment? CommoditySegment { get; set; }

        public ProductUNSPSCFamily? CommodityFamily { get; set; }

        public ProductUNSPSCClass? CommodityClass { get; set; }
    }
}