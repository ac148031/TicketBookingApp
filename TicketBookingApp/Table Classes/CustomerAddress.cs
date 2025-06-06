using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    internal class CustomerAddress
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string StreetAddress { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }

        public CustomerAddress(int addressId, int customerId, string streetAddress, int cityId, string postalCode)
        {
            AddressId = addressId;
            CustomerId = customerId;
            StreetAddress = streetAddress;
            CityId = cityId;
            PostalCode = postalCode;
        }
    }
}
