using System.Collections.Generic;

namespace Par.CommandCenter.Domain.Entities.Email
{
    public class EmailMessage
    {
        public EmailMessage()
        {
        }

        public IEnumerable<EmailAddress> ToAddresses { get; set; }
        public IEnumerable<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
