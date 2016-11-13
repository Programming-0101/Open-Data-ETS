using EdmontonTransit.OpenData.ApiModels;
using EdmontonTransit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdmontonTransit.OpenData;
using EdmontonTransit.OpenData.Adapters;
using System.IO;

namespace EdmontonTransit
{
    class Program
    {
        static void Main(string[] args)
        {
            //BoardingType type = (BoardingType)2;
            //Console.WriteLine(type.ToDescription());
            Console.Write("Enter the path to the ETS data files: ");
            string basePath = Console.ReadLine();

            var stopTimes = LoadThroughAdapter<StopTime>(new StopTimeAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.StopTimes), true));
            Console.WriteLine($"Loaded {stopTimes.Count} Stop Times");

            var routes = LoadThroughAdapter<Route>(new RouteAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Routes), true));
            Console.WriteLine($"Loaded {routes.Count} Routes");

            var landmarks = LoadThroughAdapter<Landmark>(new LandmarkAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Landmarks), true));
            Console.WriteLine($"Loaded {landmarks.Count} Landmarks");

            var transfers = LoadThroughAdapter<Transfer>(new TransferAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Transfers), true));
            Console.WriteLine($"Loaded {transfers.Count} Transfers");

            var calendarDates = LoadThroughAdapter<CalendarDate>(new CalendarDateAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.CalendarDates), true));
            Console.WriteLine($"Loaded {calendarDates.Count} Calendar Dates");
        }

        static IList<T> LoadThroughAdapter<T>(AbstractTextFileAdapter<T> adapter) where T : class
        {
            IList<T> result = adapter.Load() as IList<T>;
            return result;
        }
    }
}
