using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.Database.Entities
{
    [Table("BusStops")]
    public class BusStop
    {
        public BusStop()
        {
            CityLandmarks = new HashSet<CityLandmark>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusStopId { get; set; }
        public int? CityLandmarkId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Location { get; set; }

        #region Navivation Properties
        public virtual IEnumerable<CityLandmark> CityLandmarks { get; set; }
        #endregion
    }
    [Table("BusRoutes")]
    public class BusRoute
    {
        public BusRoute()
        {
            Trips = new HashSet<Trip>();
            ServiceChanges = new HashSet<ServiceChange>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusRouteId { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }

        #region NavigationProperties
        public virtual IEnumerable<Trip> Trips { get; set; }
        public virtual IEnumerable<ServiceChange> ServiceChanges { get; set; }
        #endregion
    }
    [Table("BusTransfers")]
    public class BusTransfer
    {
        [Key]
        public int BusTransferId { get; set; }
        public int FromStopId { get; set; }
        public int ToStopId { get; set; }
        public TransferConnectionType TransferType { get; set; }
        public string MinimumTransferTime { get; set; }

        #region Navigation Properties
        public virtual BusStop FromStop { get; set; }
        public virtual BusStop ToStop { get; set; }
        #endregion
    }
    [Table("CityLandmarks")]
    public class CityLandmark
    {
        public CityLandmark()
        {
            BusStops = new HashSet<BusStop>();
        }
        [Key]
        public int CityLandmarkId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        #region Navigation Properties
        public virtual IEnumerable<BusStop> BusStops { get; set; }
        #endregion
    }
    [Table("ServiceChanges")]
    public class ServiceChange
    {
        [Key]
        public int ServiceExceptionId { get; set; }
        public DateTime Date { get; set; }
        public ServiceExceptionType ExceptionType { get; set; }
        public int BusRouteId { get; set; }
        public DateTime CreatedOn { get; set; }

        #region Navigation Properties
        public virtual BusRoute BusRoute { get; set; }
        #endregion
    }
    [Table("ScheduledStops")]
    public class ScheduledStop
    {
        [Key]
        public int ScheduledStopId { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureTime { get; set; }
        public int BusStopId { get; set; }
        public int TripId { get; set; }
        public short Sequence { get; set; }
        public BoardingType PickupType { get; set; }
        public BoardingType DropOffType { get; set; }

        #region Navigation Properties
        public virtual BusStop BusStop { get; set; }
        public virtual Trip Trip { get; set; }
        #endregion
    }
    [Table("Trips")]
    public class Trip
    {
        public Trip()
        {
            ScheduledStops = new HashSet<ScheduledStop>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TripId { get; set; }
        public int? BusRouteId { get; set; }

        #region Navigation Properties
        public virtual BusRoute BusRoute { get; set; }
        public virtual IEnumerable<ScheduledStop> ScheduledStops { get; set; }
        #endregion
    }
}
