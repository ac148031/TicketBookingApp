using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class CustomerAddress
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

    public class FullCustomerAddress : CustomerAddress
    {
        public string CityName { get; set; }

        [Obsolete("", true)]
        public new int CityId
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public FullCustomerAddress(int addressId, int customerId, string streetAddress, string postalCode)
            : base(addressId, customerId, streetAddress, 0, postalCode)
        {
            CityName = "";
        }
    }
}
