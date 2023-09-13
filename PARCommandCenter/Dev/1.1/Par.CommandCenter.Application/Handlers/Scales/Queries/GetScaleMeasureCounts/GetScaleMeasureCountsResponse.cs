using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Scales.Queries.GetScaleMeasureCounts
{
    public class GetScaleMeasureCountsResponse
    {
        public List<TenantScaleMeasureCountsModel> TenantScaleMeasureCounts { get; set; }
    }

    public class TenantScaleMeasureCountsModel : IMap<TenantScaleMeasureCounts>
    {
        public int TenantId { get; set; }

        public string TenantName { get; set; }


        /// <summary>
        /// The scale measure count in the first hour. The first hour normally at midnight (12:00AM)
        /// </summary>
        public int Hour1Total { get; set; }

        /// <summary>
        /// The scale measure count in the second hour since midnight (01:00AM)
        /// </summary>
        public int Hour2Total { get; set; }

        /// <summary>
        /// The scale measure count in the third hour since midnight (02:00AM)
        /// </summary>
        public int Hour3Total { get; set; }

        /// <summary>
        /// The scale measure count in the 4th hour since midnight (03:00AM)
        /// </summary>
        public int Hour4Total { get; set; }

        /// <summary>
        /// The scale measure count in the 5th hour since midnight (04:00AM)
        /// </summary>
        public int Hour5Total { get; set; }

        /// <summary>
        /// The scale measure count in the 6th hour since midnight (05:00AM)
        /// </summary>
        public int Hour6Total { get; set; }

        /// <summary>
        /// The scale measure count in the 7th hour since midnight (06:00AM)
        /// </summary>
        public int Hour7Total { get; set; }

        /// <summary>
        /// The scale measure count in the 8th hour since midnight (07:00AM)
        /// </summary>
        public int Hour8Total { get; set; }

        /// <summary>
        /// The scale measure count in the 9th hour since midnight (08:00AM)
        /// </summary>
        public int Hour9Total { get; set; }

        /// <summary>
        /// The scale measure count in the 10th hour since midnight (09:00AM)
        /// </summary>
        public int Hour10Total { get; set; }

        /// <summary>
        /// The scale measure count in the 11 hour since midnight (10:00AM)
        /// </summary>
        public int Hour11Total { get; set; }

        /// <summary>
        /// The scale measure count in the 12 hour since midnight (11:00AM)
        /// </summary>
        public int Hour12Total { get; set; }

        /// <summary>
        /// The scale measure count in the 13 hour since midnight (12:00PM)
        /// </summary>
        public int Hour13Total { get; set; }

        /// <summary>
        /// The scale measure count in the 14 hour since midnight (01:00PM)
        /// </summary>
        public int Hour14Total { get; set; }

        /// <summary>
        /// The scale measure count in the 15 hour since midnight (02:00PM)
        /// </summary>
        public int Hour15Total { get; set; }

        /// <summary>
        /// The scale measure count in the 16 hour since midnight (03:00PM)
        /// </summary>
        public int Hour16Total { get; set; }

        /// <summary>
        /// The scale measure count in the 17 hour since midnight (04:00PM)
        /// </summary>
        public int Hour17Total { get; set; }

        /// <summary>
        /// The scale measure count in the 18 hour since midnight (05:00PM)
        /// </summary>
        public int Hour18Total { get; set; }

        /// <summary>
        /// The scale measure count in the 19 hour since midnight (06:00PM)
        /// </summary>
        public int Hour19Total { get; set; }

        /// <summary>
        /// The scale measure count in the 20th hour since midnight (07:00PM)
        /// </summary>
        public int Hour20Total { get; set; }

        /// <summary>
        /// The scale measure count in the 21 hour since midnight (08:00PM)
        /// </summary>
        public int Hour21Total { get; set; }

        /// <summary>
        /// The scale measure count in the 22nd hour since midnight (09:00PM)
        /// </summary>
        public int Hour22Total { get; set; }

        /// <summary>
        /// The scale measure count in the 23rd since midnight (10:00PM)
        /// </summary>
        public int Hour23Total { get; set; }

        /// <summary>
        /// The scale measure count in the 24th since midnight (11:00PM)
        /// </summary>
        public int Hour24Total { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TenantScaleMeasureCounts, TenantScaleMeasureCountsModel>();
        }
    }
}
