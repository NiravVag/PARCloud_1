namespace Par.CommandCenter.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public int TenantId { get; set; }


        public string Name { get; set; }

        public string Number { get; set; }

        public int ProductId { get; set; }

        public int IssueItemUnitId { get; set; }

        public string? VendorNumber { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual Location Location { get; set; }

        public virtual InventoryTrackingType InventoryTrackingType { get; set; }

        public virtual ItemUnit ItemUnit { get; set; }

        public virtual LocationItem LocationItem { get; set; }

    }
}
