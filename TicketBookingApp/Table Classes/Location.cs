using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    internal class Location
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
}
