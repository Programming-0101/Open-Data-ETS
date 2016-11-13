using EdmontonTransit.Common;
using EdmontonTransit.OpenData.ApiModels;
using System;

namespace EdmontonTransit.OpenData.Adapters
{
    public class CalendarDateAdapter : AbstractTextFileAdapter<CalendarDate>
    {
        public CalendarDateAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine) : base(fileFormat, filePath, skipFirstLine)
        {
        }

        protected override CalendarDate ParseRow(string row)
        {
            /*
             * service_id,date,exception_type
             * 897-Weekday-2-SEP16-1111100,20161125,1
             */
            string[] csvData = row.Split(',');
            return new CalendarDate()
            {
                ServiceId = csvData[0],
                Date = csvData[1],
                ExceptionType = int.Parse(csvData[2])
            };
        }
    }
}
