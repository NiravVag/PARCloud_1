using System;

namespace Par.CommandCenter.Domain.Entities.Users
{
    public class User
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserName { get; set; }

        public string AzureAdObjectId { get; set; }

        public bool Deleted { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CreatedUserId { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
