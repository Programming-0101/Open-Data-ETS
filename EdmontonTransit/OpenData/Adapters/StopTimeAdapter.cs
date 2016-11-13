using EdmontonTransit.Common;
using EdmontonTransit.Database.Entities;
using EdmontonTransit.OpenData.ApiModels;
using System;

namespace EdmontonTransit.OpenData.Adapters
{
    public class StopTimeAdapter : AbstractTextFileAdapter<StopTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopTimeAdapter"/> class.
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <param name="filePath"></param>
        public StopTimeAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine) : base(fileFormat, filePath, skipFirstLine)
        {
        }

        protected override StopTime ParseRow(string row)
        {
            /*
             * trip_id,arrival_time,departure_time,stop_id,stop_sequence,stop_headsign,pickup_type,drop_off_type
             * 9638083,06:28:00,06:28:00,5009,1,1 Capilano,0,1
             */
            string[] csvData = row.Split(',');
            return new StopTime()
            {
                TripId = int.Parse(csvData[0]),
                Arrival = csvData[1],
                ArrivalTime = ParseTimeSpan(csvData[1]),
                Departure = csvData[2],
                DepartureTime = ParseTimeSpan(csvData[2]),
                StopId = int.Parse(csvData[3]),
                StopSequence = int.Parse(csvData[4]),
                StopHeadsign = csvData[5],
                PickupType = (BoardingType)int.Parse(csvData[6]),
                DropOffType = (BoardingType)int.Parse(csvData[7])
            };
        }
        private TimeSpan ParseTimeSpan(string text)
        {
            string[] parts = text.Split(':');
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            int seconds = int.Parse(parts[2]);
            return new TimeSpan(hours % 24, minutes, seconds);
        }
    }
}
