namespace Par.CommandCenter.Domain.Entities
{
    public class LocationItem
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int ItemId { get; set; }

        public int LocationId { get; set; }

        public int InventoryTrackingTypeId { get; set; }

        public int? Quantity { get; set; }

    }
}
