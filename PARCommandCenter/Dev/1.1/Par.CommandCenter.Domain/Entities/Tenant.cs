namespace Par.CommandCenter.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Acronym { get; set; }

        public short DefaultTimeZoneId { get; set; }

        public byte OrderBoxPercentage { get; set; }

        public int EmployeeSecurityTypeId { get; set; }

        public bool IssueAdjustments { get; set; }

        public bool ParMobileAllowRememberMe { get; set; }

        public TimeZone TimeZone { get; set; }

        public bool Deleted { get; set; }

        public bool IsTest { get; set; }
    }
}
