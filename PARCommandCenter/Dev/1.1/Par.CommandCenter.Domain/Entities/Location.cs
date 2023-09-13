namespace Par.CommandCenter.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FacilityId { get; set; }

        public int TenantId { get; set; }
    }
}
