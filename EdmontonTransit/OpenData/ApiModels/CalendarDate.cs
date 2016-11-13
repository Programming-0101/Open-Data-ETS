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
    /// https://data.edmonton.ca/Transit/ETS-Bus-Schedule-GTFS-Data-Feed-Calendar-Dates/f2sy-bth7
    /// </remarks>
    public class CalendarDate
    {
        // service_id,date,exception_type
        public int Id { get; set; }
        /// <summary>ID that uniquely identifies a set of dates when a service exception is available for one or more routes</summary>
        public string ServiceId { get; set; }
        /// <summary>Particular date when service availability is different than the norm</summary>
        public string Date { get; set; }
        /// <summary>Indicates whether service is available on the date specified in the date field.</summary>
        public int ExceptionType { get; set; }
    }

}
