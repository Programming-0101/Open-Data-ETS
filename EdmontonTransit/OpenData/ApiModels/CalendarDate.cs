using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.OpenData.ApiModels
{
    public enum MonthOfYear { JAN = 1, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC }

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

        // Derived items from spliting the ServiceId
        public int BusRouteId { get { return int.Parse(ServiceId.Split('-')[0]); } }
        public DateTime ChangeDate
        {
            get
            {
                var parts = ServiceId.Split('-');
                int day = int.Parse(parts[2]);
                int year = int.Parse(parts[3].Substring(3)) + 2000;
                int month = (int)(MonthOfYear)Enum.Parse(typeof(MonthOfYear), parts[3].Substring(0, 3));
                return new DateTime(year,month, day);
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                int day = int.Parse(Date.Substring(6,2));
                int year = int.Parse(Date.Substring(0, 4));
                int month = int.Parse(Date.Substring(4,2));
                return new DateTime(year, month, day);
            }
        }
    }

}
