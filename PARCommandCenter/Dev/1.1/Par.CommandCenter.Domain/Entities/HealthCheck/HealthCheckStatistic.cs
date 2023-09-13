namespace Par.CommandCenter.Domain.Entities.HealthCheck
{
    public enum HealthCheckType
    {
        VPN = 0,

        Interface = 2,

        Routers = 3,

        Controllers = 4,

        Scales = 5,

        Servers = 6
    }

    public class HealthCheckStatistic
    {
        public HealthCheckType Type { get; set; }

        public int TotalTenantsCount { get; set; }

        public int HealthyTenantsCount { get; set; }
        public int HealthyCount { get; set; }

        public int UnHealthyTenantsCount { get; set; }
        public int UnHealthyCount { get; set; }

        public int WarrningTenantsCount { get; set; }
        public int WarrningCount { get; set; }
    }
}
