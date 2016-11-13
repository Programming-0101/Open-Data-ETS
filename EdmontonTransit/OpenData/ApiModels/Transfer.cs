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
    /// Based upon data from
    /// https://data.edmonton.ca/Transit/ETS-Bus-Schedule-GTFS-Data-Feed-Transfers/hnhf-yaps
    /// </remarks>
    public class Transfer
    {
        //from_stop_id,to_stop_id,transfer_type,min_transfer_time
        /// <summary>ID that identifies a stop or station where a connection between routes begins</summary>
        public int FromStopId { get; set; }
        /// <summary>ID that identifies a stop or station where a connection between routes ends</summary>
        public int ToStopId { get; set; }
        /// <summary>Type of connection for the specified pairs. Values for this field: * 0 or (empty) - . 1 - . 2 -  3 - </summary>
        public int TransferType { get; set; }
        /// <summary>Amount of time that must be available in an itinerary to permit a transfer between routes at these stops.</summary>
        public string MinTransferTime { get; set; }
    }

}
