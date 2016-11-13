using EdmontonTransit.Common;
using EdmontonTransit.OpenData.ApiModels;
using System;

namespace EdmontonTransit.OpenData.Adapters
{
    public class LandmarkAdapter : AbstractTextFileAdapter<Landmark>
    {
        public LandmarkAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine) : base(fileFormat, filePath, skipFirstLine)
        {
        }

        protected override Landmark ParseRow(string row)
        {
            /*
             * LANDMARK_NAME,ADDRESS,STOP_ID,LATITUDE,LONGITUDE,LOCATION
             * #1 Field Ambulance Edmonton Garrison,Edmonton Garrison,7404,53.680786285493298,-113.48305992153099,"(53.6807862854933, -113.483059921531)"
             */
            // Special handling of exceptional cases
            if (row.Contains(", The \""))
                row = row.Replace(", The \"", "").Replace("\"", "The ");
            if (row.Contains(", The\""))
                row = row.Replace(", The\"", "").Replace("\"", "The ");
            if (row.Contains("Balwin,Steele Heights"))
                row = row.Replace("Balwin,Steele Heights", "Balwin/Steele Heights");
            if (row.Contains("Castledowns,Beaumaris"))
                row = row.Replace("Castledowns,Beaumaris", "Castledowns/Beaumaris");
            if (row.Contains(", Kingsway NW"))
                row = row.Replace(", Kingsway NW", "- Kingsway NW");

                string[] csvData = row.Split(',');
            return new Landmark()
            {
                LandmarkName = csvData[0],
                Address = csvData[1],
                StopId = int.Parse(csvData[2]),
                Latitude = double.Parse(csvData[3]),
                Longitude = double.Parse(csvData[4]),
                Location = csvData[5]
            };
        }
    }
}
