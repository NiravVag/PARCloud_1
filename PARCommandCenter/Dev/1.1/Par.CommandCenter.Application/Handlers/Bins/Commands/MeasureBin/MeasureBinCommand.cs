using AutoMapper;
using MediatR;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;

namespace Par.CommandCenter.Application.Handlers.Bins.Commands.MeasureBin
{
    public class MeasureBinCommand : IRequest<MeasureBinCommandResponse>, IMap<Bin>
    {
        /// <summary>
        /// The ID of the bin to measure.
        /// </summary>
        public int BinId { get; set; }

        /// <summary>
        /// Number of seconds to wait for the request to complete.
        /// </summary>
        public int Timeout { get; set; } = 60;

        /// <summary>
        /// The reference weight to use when calculating the bin quantity. Optional.
        /// </summary>
        public decimal? ReferenceWeight { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MeasureBinCommand, Bin>();
        }
    }
}
