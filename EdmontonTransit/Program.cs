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
using EdmontonTransit.Database.Entities;
using EdmontonTransit.Database;

namespace EdmontonTransit
{
    class Program
    {
        private static IList<Route> _Routes;
        private static IList<StopTime> _StopTimes;
        private static IList<Landmark> _Landmarks;
        private static IList<Transfer> _Transfers;
        private static IList<CalendarDate> _CalendarDates;
        static void Main(string[] args)
        {
            LoadDataFiles();
            SaveData();
        }

        private static void SaveData()
        {
            using (var context = new EdmontonTransitContext())
            {

                var cityLandmarks = from city in _Landmarks
                                    group city by new { city.LandmarkName, city.Address } into place
                                    select new CityLandmark
                                    {
                                        Address = place.Key.Address,
                                        Name = place.Key.LandmarkName
                                    };
                var busStops = from place in _Landmarks
                               group place by new { place.StopId, place.Longitude, place.Latitude, place.Location } into stop
                               select new BusStop
                               {
                                   BusStopId = stop.Key.StopId,
                                   Longitude = stop.Key.Longitude,
                                   Latitude = stop.Key.Latitude,
                                   Location = stop.Key.Location,
                                   CityLandmarks = cityLandmarks.Where(x => stop.Any(place => x.Name == place.LandmarkName && place.Address == x.Address))
                               };
                context.BusStops.AddRange(busStops);
                //var temp = (from data in busStops
                //           group data by data.BusStopId into grp
                //           where grp.Count() > 1
                //           select grp).ToList();
                //Console.WriteLine(temp.Count());
                var busRoutes = from route in _Routes
                                select new BusRoute
                                {
                                    BusRouteId = route.RouteId,
                                    ShortName = route.ShortName,
                                    LongName = route.LongName
                                };
                context.BusRoutes.AddRange(busRoutes);
                var trips = from stop in _StopTimes
                            where stop.RouteId.HasValue
                            group stop by new { stop.TripId, stop.RouteId } into trip
                            select new Trip
                            {
                                TripId = trip.Key.TripId,
                                BusRouteId = trip.Key.RouteId
                            };
                context.Trips.AddRange(trips);
                var otherbusStops = from item in _StopTimes
                                    group item by item.StopId into stop
                                    where !busStops.Any(x => x.BusStopId == stop.Key)
                                    select new BusStop
                                    {
                                        BusStopId = stop.Key
                                    };
                context.BusStops.AddRange(otherbusStops);
                var scheduledStops = from stop in _StopTimes
                                     select new ScheduledStop
                                     {
                                         BusStopId = stop.StopId,
                                         //BusStop = busStops.Any(x => x.BusStopId == stop.StopId)
                                         //        ? busStops.Single(x => x.BusStopId == stop.StopId)
                                         //        : new BusStop
                                         //        {
                                         //            BusStopId = stop.StopId
                                         //        },
                                         ArrivalTime = stop.Arrival,
                                         DepartureTime = stop.Departure,
                                         DropOffType = stop.DropOffType,
                                         PickupType = stop.PickupType,
                                         Sequence = (short)stop.StopSequence,
                                         TripId = stop.TripId
                                         //,
                                         //Trip = trips.SingleOrDefault(x => x.BusRouteId.HasValue && x.BusRouteId == stop.RouteId)
                                     };
                context.ScheduledStops.AddRange(scheduledStops);
                var busTransfers = from transfer in _Transfers
                                   select new BusTransfer
                                   {
                                       ToStopId = transfer.ToStopId,
                                       //ToStop = busStops.Single(x => x.BusStopId == transfer.ToStopId),
                                       FromStopId = transfer.FromStopId,
                                       //FromStop = busStops.Single(x => x.BusStopId == transfer.FromStopId),
                                       MinimumTransferTime = transfer.MinTransferTime,
                                       TransferType = (TransferConnectionType)transfer.TransferType
                                   };
                context.BusTransfers.AddRange(busTransfers);

                var serviceChanges = from change in _CalendarDates
                                     select new ServiceChange
                                     {
                                         BusRouteId = change.BusRouteId,
                                         //BusRoute = busRoutes.Single(x => x.BusRouteId == change.BusRouteId),
                                         ExceptionType = (ServiceExceptionType)change.ExceptionType,
                                         CreatedOn = change.CreatedDate,
                                         Date = change.ChangeDate
                                     };
                context.ServiceChanges.AddRange(serviceChanges);

                context.SaveChanges();
            }
            Console.WriteLine("Object graphs created");
        }

        private static void LoadDataFiles()
        {
            Console.Write("Enter the path to the ETS data files: ");
            string basePath = Console.ReadLine();

            _StopTimes = LoadThroughAdapter<StopTime>(new StopTimeAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.StopTimes), true));
            //Console.WriteLine($"Loaded {_StopTimes.Count} Stop Times");

            _Routes = LoadThroughAdapter<Route>(new RouteAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Routes), true));
            //Console.WriteLine($"Loaded {routes.Count} Routes");

            _Landmarks = LoadThroughAdapter<Landmark>(new LandmarkAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Landmarks), true));
            //Console.WriteLine($"Loaded {landmarks.Count} Landmarks");

            _Transfers = LoadThroughAdapter<Transfer>(new TransferAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.Transfers), true));
            //Console.WriteLine($"Loaded {transfers.Count} Transfers");

            _CalendarDates = LoadThroughAdapter<CalendarDate>(new CalendarDateAdapter(FileFormat.CSV, Path.Combine(basePath, EtsDataFiles.CalendarDates), true));
            //Console.WriteLine($"Loaded {calendarDates.Count} Calendar Dates");
        }
        static IList<T> LoadThroughAdapter<T>(AbstractTextFileAdapter<T> adapter) where T : class
        {
            IList<T> result = adapter.Load() as IList<T>;
            return result;
        }
    }
}
