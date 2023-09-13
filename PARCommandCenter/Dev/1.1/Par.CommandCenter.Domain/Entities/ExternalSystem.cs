namespace Par.CommandCenter.Domain.Entities
{
    public class ExternalSystem
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; }

        public int ExternalSystemTypeId { get; set; }
    }
}
