using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Interfaces
{
    /// <summary>
    ///     A service used to make http calls to azure functions.
    /// </summary>
    public interface IAzureFunctionsClient
    {    
        /// <summary>
        ///     Asynchronously Request Bin Measurement.
        /// </summary>
        /// <param name="binId">The ID of the bin to measure.</param>       
        /// <param name="referenceWeight">The reference weight to use when calculating the bin quantity. Optional.</param>      
        /// <param name="cancellationToken">
        ///     The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.
        /// </param>        
        Task<(decimal? Weight, int? Quantity)> RequestBinMeasureAsync(int binId, decimal? referenceWeight, CancellationToken cancellationToken);        
    }
}