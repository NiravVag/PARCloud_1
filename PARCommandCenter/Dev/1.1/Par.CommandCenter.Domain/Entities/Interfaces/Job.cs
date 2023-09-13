namespace Par.CommandCenter.Domain.Entities.Interfaces
{
    public class Job
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; }

        public byte JobTypeId { get; set; }
    }
}
