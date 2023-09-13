using JetBrains.Annotations;
using Par.CommandCenter.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    /// <summary>
    ///     A service used to make http calls to azure functions.
    /// </summary>
    public interface IAzureMapAPIWebClient
    {
        /// <summary>
        ///     Asynchronously get address geo location and return a geo coordinate object contain lat and lang.
        /// </summary>
        /// <param name="address">The address to look for using the search api from Azure Map.</param>       
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param> 
        [NotNull]
        Task<GeoCoordinate> GetAddressCoordinates(Address address, CancellationToken cancellationToken);
    }
}
