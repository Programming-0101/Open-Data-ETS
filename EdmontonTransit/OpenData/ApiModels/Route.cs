using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.OpenData.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Based upon data from
    /// https://data.edmonton.ca/Transit/ETS-Bus-Schedule-GTFS-Data-Feed-Routes/d577-xky7
    /// </remarks>
    public class Route
    {
        /// <summary>ID that uniquely identifies a route</summary>
        public int RouteId { get; set; }
        /// <summary>Short name of a route</summary>
        public string ShortName { get; set; }
        /// <summary>Full name of a route. This name is generally more descriptive than the route_short_name and will often include the route's destination or stop.</summary>
        public string LongName { get; set; }

        // TODO: Not implemented properties, because data file has empty fields
        //public string Description { get; set; }
        //public int? RouteType { get; set; }
        //public string RouteUrl { get; set; }
    }
}
