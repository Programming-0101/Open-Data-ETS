# Open-Data-ETS

Results of running program:

```text
Loading data from files.....
Enter the path to the ETS data files: C:\Users\Dan\Documents\GitHub\(Programming-0101)\Open Data ETS\NoTrack
Saving data to database.....
        Saved 25 Transit Centers
        Saved 407 BusRoutes
        Saved 11 BusRoutes
        Saved 2764 BusTransfers
        Saved 6581 BusStops
        Saved 2970 CityLandmarks
        Updated 3489 BusStops (0 duplicates) and added 256 new BusStops
        Updated 12736 BusStops for CityLandmarks
                with 0 orphaned BusStops
                and 0 unknown CityLandmarks
        Created 37946 Trip Items (before database insert)
                Saved 37946 Trip Items to the database in a bulk insert

        Saved 149 TripDestinations to database
                Generated 2011551 SheduledStops (before bulk insert)
        Saved 2011551 SheduledStops to database
        Saved 10349 ServiceChanges
Object graphs created - Done Database Setup

Total Running Time: 588.4801577
Press any key to continue . . .
```

Database table counts:

```text
Rows of BusRoutes
418

Rows of BusStopLandmarks
12736

Rows of BusStops
6837

Rows of BusTransfers
2764

Rows of CityLandmarks
2970

Rows of ScheduledStops
2011551

Rows of ServiceChanges
10349

Rows of TransitCenters
25

Rows of TripDestinations
149

Rows of Trips
37946
```

---

```sql
select count(*) as 'Rows of BusRoutes' from BusRoutes
select count(*) as 'Rows of BusStopLandmarks' from BusStopLandmarks
select count(*) as 'Rows of BusStops' from BusStops
select count(*) as 'Rows of BusTransfers' from BusTransfers
select count(*) as 'Rows of CityLandmarks' from CityLandmarks
select count(*) as 'Rows of ScheduledStops' from ScheduledStops
select count(*) as 'Rows of ServiceChanges' from ServiceChanges
select count(*) as 'Rows of TransitCenters' from TransitCenters
select count(*) as 'Rows of TripDestinations' from TripDestinations
select count(*) as 'Rows of Trips' from Trips
```
