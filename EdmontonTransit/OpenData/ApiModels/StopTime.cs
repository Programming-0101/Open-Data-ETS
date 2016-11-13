using EdmontonTransit.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.OpenData.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Based upon from data at
    /// https://data.edmonton.ca/Transit/ETS-Bus-Schedule-GTFS-Data-Feed-Stop-Times/ebvt-eg97
    /// </remarks>
    public class StopTime
    {
        /// <summary>ID that identifies a trip - it is unique.</summary>
        public int TripId { get; set; }
        public string Arrival { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string Departure { get; set; }
        /// <summary>Specifies the departure time from a specific stop for a specific trip on a route</summary>
        public TimeSpan DepartureTime { get; set; }
        /// <summary>ID that uniquely identifies a stop. Multiple routes may use the same stop.</summary>
        public int StopId { get; set; }
        /// <summary>Identifies the order of the stops for a particular trip</summary>
        public int StopSequence { get; set; }
        /// <summary>Equivalent to a Route.ShortName</summary>
        public string StopHeadsign { get; set; }
        /// <summary>Indicates if passengers are picked up or pickup at the stop is not available.</summary>
        [Description("Pickup")]
        public BoardingType PickupType { get; set; }
        /// <summary>Indicates if passengers are dropped off or drop-off at stop is not available.</summary>
        [Description("Drop off")]
        public BoardingType DropOffType { get; set; }

        public int? RouteId
        {
            get
            {
                int id;
                if (int.TryParse(StopHeadsign.Split(' ')[0], out id))
                    return id;
                else
                    return null;
            }
        }
    }
}
