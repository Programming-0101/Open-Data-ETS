using EdmontonTransit.Common;
using EdmontonTransit.OpenData.ApiModels;
using System;

namespace EdmontonTransit.OpenData.Adapters
{
    public class RouteAdapter : AbstractTextFileAdapter<Route>
    {
        public RouteAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine) : base(fileFormat, filePath, skipFirstLine)
        {
        }

        protected override Route ParseRow(string row)
        {
            /*
             * route_id,route_short_name,route_long_name,route_description,route type,route_url
             * 140,140,Northgate - Downtown - Lago Lindo,,,
             */
            string[] csvData = row.Split(',');
            return new Route()
            {
                RouteId = int.Parse(csvData[0]),
                ShortName = csvData[1],
                LongName = csvData[2]
            };
        }
    }
}
