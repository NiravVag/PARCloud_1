using System;

namespace Par.CommandCenter.Domain.Entities.Users
{
    /// <summary>
    /// Utility class to return a user's name, tenant id and user id from the database context
    /// </summary>
    public class SessionUser : IDisposable
    {
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The tenant id or null if support user and impersonation is not active
        /// </summary>
        public int? TenantId { get; set; }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }
    }
}
