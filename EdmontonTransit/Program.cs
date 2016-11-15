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
using EntityFramework.BulkInsert.Extensions;
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
            Console.WriteLine("Loading data from files.....");
            LoadDataFiles();
            Console.WriteLine("Saving data to database.....");
            SaveData();
        }

        private static void SaveBusRoutes()
        {
            using (var context = new EdmontonTransitContext())
            {
                var busRoutes = from route in _Routes
                                select new BusRoute
                                {
                                    BusRouteId = route.RouteId,
                                    ShortName = route.ShortName,
                                    LongName = route.LongName
                                };
                context.BusRoutes.AddRange(busRoutes);
                context.SaveChanges();
                Console.WriteLine($"\tSaved {busRoutes.Count()} BusRoutes");
                var otherBusRoutes = from stop in _StopTimes
                                     group stop by stop.RouteId into routes
                                     where routes.Key.HasValue
                                        && !busRoutes.Any(x => x.BusRouteId == routes.Key)
                                     select new BusRoute
                                     {
                                         BusRouteId = routes.Key.Value
                                     };
                context.BusRoutes.AddRange(otherBusRoutes);
                context.SaveChanges();
                Console.WriteLine($"\tSaved {busRoutes.Count()} BusRoutes");
            }
        }

        private static void SaveBusTransfers()
        {
            using (var context = new EdmontonTransitContext())
            {
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
                context.SaveChanges();
                Console.WriteLine($"\tSaved {busTransfers.Count()} BusTransfers");

            }
        }

        private static void SaveBusStops()
        {
            using (var context = new EdmontonTransitContext())
            {
                var otherbusStops = from item in _StopTimes
                                    group item by item.StopId into stop
                                    select new BusStop
                                    {
                                        BusStopId = stop.Key
                                    };
                context.BusStops.AddRange(otherbusStops);
                context.SaveChanges();
                Console.WriteLine($"\tSaved {otherbusStops.Count()} BusStops");
            }
        }

        private static void SaveCityLandmarks()
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
                context.CityLandmarks.AddRange(cityLandmarks);
                context.SaveChanges();
                Console.WriteLine($"\tSaved {cityLandmarks.Count()} CityLandmarks");
            }
        }

        private static void UpdateBusStops()
        {
            using (var context = new EdmontonTransitContext())
            {
                // TODO: Use OpenRowSet to improve performance https://msdn.microsoft.com/en-us/library/ms175915(v=sql.110).aspx
                var busStops = from place in _Landmarks
                               group place by new { place.StopId, place.Longitude, place.Latitude, place.Location } into stop
                               select new BusStop
                               {
                                   BusStopId = stop.Key.StopId,
                                   Longitude = stop.Key.Longitude,
                                   Latitude = stop.Key.Latitude,
                                   Location = stop.Key.Location
                               };
                int updateCount = 0, addCount = 0, duplicate = 0;
                foreach (var stop in busStops)
                {
                    var found = context.BusStops.Find(stop.BusStopId);
                    if(found != null)
                    if (found.Longitude != stop.Longitude || found.Latitude != stop.Latitude || found.Location != stop.Location)
                    {
                        //var attached = context.BusStops.Attach(stop);
                        found.Longitude = stop.Longitude ;
                        found.Latitude = stop.Latitude ;
                        found.Location = stop.Location;
                        var existing = context.Entry<BusStop>(found);
                        existing.State = System.Data.Entity.EntityState.Modified;
                        updateCount++;
                    }
                    else
                        duplicate++;
                    else
                    {
                        context.BusStops.Add(stop);
                        addCount++;
                    }
                }
                context.SaveChanges();
                Console.WriteLine($"\tUpdated {updateCount} BusStops ({duplicate} duplcates) and added {addCount} new BusStops");
            }
        }

        private static void UpdateBusStopsWithLandmarks()
        {
            using (var context = new EdmontonTransitContext())
            {
                // TODO: Use OpenRowSet to improve performance https://msdn.microsoft.com/en-us/library/ms175915(v=sql.110).aspx
                var cityLandmarks = from city in _Landmarks
                                    group city by new { city.LandmarkName, city.Address } into place
                                    select new
                                    {
                                        Address = place.Key.Address,
                                        Name = place.Key.LandmarkName,
                                        BusStopIds = from stop in place
                                                     select stop.StopId
                                    };
                int updateCount = 0, landmarkWithoutStop = 0, unknownLandmark = 0;
                foreach (var landmark in cityLandmarks)
                {
                    var attached = context.CityLandmarks.SingleOrDefault(place => place.Address == landmark.Address && place.Name == landmark.Name);
                    if (attached != null)
                    {
                        foreach (var stopid in landmark.BusStopIds)
                        {
                            var theStop = context.BusStops.Find(stopid);
                            if (theStop != null)
                            {
                                attached.BusStops.Add(theStop);
                                var existing = context.Entry<CityLandmark>(attached);
                                existing.State = System.Data.Entity.EntityState.Modified;
                                updateCount++;
                            }
                            else
                                landmarkWithoutStop++;
                        }
                    }
                    else
                    {
                        unknownLandmark++;
                    }
                    context.SaveChanges();
                }
                Console.WriteLine($"\tUpdated {updateCount} BusStops for CityLandmarks \n\t\twith {landmarkWithoutStop} orphaned BusStops \n\t\tand {unknownLandmark} unknown CityLandmarks");
            }
        }

        private static void SaveTrips()
        {
            using (var context = new EdmontonTransitContext())
            {
                SortedSet<int> lastTripId = new SortedSet<int>();
                int batchCount = 0, itemCount = 0;
                foreach (var stop in _StopTimes)
                {
                    if (! lastTripId.Contains(stop.TripId))
                    {
                        context.Trips.Add(new Trip { TripId = stop.TripId });
                        lastTripId.Add( stop.TripId);
                        batchCount++;
                    }
                    if (batchCount >= 2000)
                    {
                        context.SaveChanges();
                        Console.WriteLine($"\tSaved {batchCount} Trips");
                        batchCount = 0;
                    }
                }
                if (batchCount > 0)
                {
                    context.SaveChanges();
                    Console.WriteLine($"\tSaved {batchCount} Trips");
                }
            }
        }

        private static void SaveTrips2()
        {
            // NOTES: Bulk Insert possible through https://efbulkinsert.codeplex.com/
            //        Special thanks to http://stackoverflow.com/a/21839455/2154662
            using (var context = new EdmontonTransitContext())
            {
                SortedSet<int> lastTripId = new SortedSet<int>();
                IList<Trip> trips = new List<Trip>();

                Trip newTrip = null;
                Dictionary<int, BusRoute> routeList = context.BusRoutes.ToDictionary(x => x.BusRouteId); // Load into memory
                foreach (var stop in _StopTimes)
                {
                    if (!lastTripId.Contains(stop.TripId))
                    {
                        // Add a new trip
                        newTrip = new Trip { TripId = stop.TripId };
                        trips.Add(newTrip);
                        // Track that it's added
                        lastTripId.Add(newTrip.TripId);
                    }
                }
                Console.WriteLine($"Created {trips.Count} Trip Items (before database insert)");
                context.BulkInsert(trips);
                Console.WriteLine($"Saved {trips.Count} Trip Items to the database in a bulk insert");
                Console.WriteLine();
            }
        }

        private static void SaveScheduleStops()
        {
            using (var context = new EdmontonTransitContext())
            {
                IList<ScheduledStop> scheduledStops = new List<ScheduledStop>();
                foreach(var stop in _StopTimes)
                    scheduledStops.Add( new ScheduledStop
                                     {
                                         BusStopId = stop.StopId,
                                         BusRouteId = stop.RouteId,
                                         ArrivalTime = stop.Arrival,
                                         DepartureTime = stop.Departure,
                                         DropOffType = stop.DropOffType,
                                         PickupType = stop.PickupType,
                                         Sequence = (short)stop.StopSequence,
                                         TripId = stop.TripId
                                     });
                Console.WriteLine($"\tGenerated {scheduledStops.Count()} SheduledStops (before bulk insert)");
                context.BulkInsert(scheduledStops,20000);
                Console.WriteLine($"\tSaved {scheduledStops.Count()} SheduledStops to database");
            }
        }
        private static void SaveServiceChanges()
        {
            using (var context = new EdmontonTransitContext())
            {
                var serviceChanges = from change in _CalendarDates
                                     select new ServiceChange
                                     {
                                         BusRouteId = change.BusRouteId,
                                         ExceptionType = (ServiceExceptionType)change.ExceptionType,
                                         CreatedOn = change.CreatedDate,
                                         Date = change.ChangeDate
                                     };
                context.ServiceChanges.AddRange(serviceChanges);
                context.SaveChanges();
                Console.WriteLine($"\tSaved {serviceChanges.Count()} ServiceChanges");
            }
        }

        private static void SaveData()
        {
            SaveBusRoutes();
            SaveBusTransfers();
            SaveBusStops();
            SaveCityLandmarks();
            UpdateBusStops();
            UpdateBusStopsWithLandmarks();
            SaveTrips2();
            SaveScheduleStops();
            Console.WriteLine("Object graphs created - Done Database Setup");
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
