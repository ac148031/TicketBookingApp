using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int CityId { get; set; }
        public string LocationAddress { get; set; }
        public int LocationCapacity { get; set; }

        public Location(int locationId, string locationName, int cityId, string locationAddress, int locationCapacity)
        {
            LocationId = locationId;
            LocationName = locationName;
            CityId = cityId;
            LocationAddress = locationAddress;
            LocationCapacity = locationCapacity;
        }
    }

    public class FullLocation : Location
    {
        public string CityName { get; set; }
        [Obsolete("", true)] public new int CityId { get; set; }

        public FullLocation(int locationId, string locationName, string locationAddress, string cityName, int locationCapacity)
            : base(locationId, locationName, 0, locationAddress, locationCapacity)
        {
            CityName = cityName;
        }
    }

    public class LocationPopularity
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int TotalSales { get; set; }
        public int TotalConcerts { get; set; }

        public LocationPopularity(int locationId, string locationName, int totalSales, int totalConcerts)
        {
            LocationId = locationId;
            LocationName = locationName;
            TotalSales = totalSales;
            TotalConcerts = totalConcerts;
        }

        public static List<LocationPopularity> FromFullSales(List<FullSale> sales)
        {
            return sales.GroupBy(s => s.SaleConcert.ConcertLocation.LocationId).Select(group => new LocationPopularity(
                    group.Key,
                    group.FirstOrDefault().SaleConcert.ConcertLocation.LocationName,
                    group.Sum(s => s.SaleQuantity),
                    group.GroupBy(s => s.SaleConcert.ConcertId).Count()
                )).ToList();
        }
    }
}
