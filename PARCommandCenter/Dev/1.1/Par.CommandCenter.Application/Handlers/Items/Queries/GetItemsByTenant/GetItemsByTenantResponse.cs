using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Items.Queries.GetItemsByTenant
{
    public class GetItemsByTenantResponse
    {
        public List<ItemModel> Items { get; set; }
    }

    public class ItemModel
    {
        public int Id { get; internal set; }

        public string ItemName { get; set; }

        public string ItemNumber { get; set; }

        public string FacilityName { get; set; }

        public string LocationName { get; set; }

        public string TenantName { get; set; }

        public string ScaleAddress { get; set; }

        public int? Quantity { get; set; }

        public decimal? ReferenceWeight { get; set; }

        public string ItemType { get; set; }
    }
}
