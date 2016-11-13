using EdmontonTransit.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmontonTransit.Database
{
    internal class EdmontonTransitContext : DbContext
    {
        public EdmontonTransitContext() : base("name=EtsDb")
        {

        }

        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<ServiceChange> ServiceChanges { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<ScheduledStop> ScheduledStops { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<CityLandmark> CityLandmarks { get; set; }
        public DbSet<BusTransfer> BusTransfers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
