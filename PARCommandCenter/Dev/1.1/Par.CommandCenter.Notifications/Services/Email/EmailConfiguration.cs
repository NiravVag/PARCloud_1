
namespace Par.CommandCenter.Notifications.Services.Email
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string PopServer { get; }
        int PopPort { get; }
        string PopUsername { get; }
        string PopPassword { get; }

        string CommandCenterEmailAddress { get; }

        string ParITEmailAddress { get; }

        string TechnicalSupportEmailAddress { get; }

        string DevelopmentEmailAddressses { get; }

        string CommandCenterURL { get; }
    }

    public class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }

        public string CommandCenterEmailAddress { get; set; }

        public string ParITEmailAddress { get; set; }

        public string TechnicalSupportEmailAddress { get; set; }

        public string DevelopmentEmailAddressses { get; set; }

        public string CommandCenterURL { get; set; }
    }
}
