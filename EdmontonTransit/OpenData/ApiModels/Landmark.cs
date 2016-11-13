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
    /// https://data.edmonton.ca/Transit/ETS-Bus-Stops-by-Landmarks/9j6k-uzig
    /// </remarks>
    public class Landmark
    {
        // LANDMARK_NAME,ADDRESS,STOP_ID,LATITUDE,LONGITUDE,LOCATION
        /// <summary>Name of Land Mark</summary>
        public string LandmarkName { get; set; }
        /// <summary>Street address for land mark</summary>
        public string Address { get; set; }
        /// <summary>Unique id number for bus stop</summary>
        public int StopId { get; set; }
        /// <summary>Latitude coordinates for bus stop</summary>
        public double Latitude { get; set; }
        /// <summary>Longitude coordinates for bus stop</summary>
        public double Longitude { get; set; }
        /// <summary>Geographic coordinates for bus stop</summary>
        public string Location { get; set; }
    }
}
