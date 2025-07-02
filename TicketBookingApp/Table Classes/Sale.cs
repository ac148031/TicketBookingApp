using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public int ConcertId { get; set; }
        public int SaleQuantity { get; set; }

        public Sale(int saleId, int customerId, int concertId, int saleQuantity)
        {
            SaleId = saleId;
            CustomerId = customerId;
            ConcertId = concertId;
            SaleQuantity = saleQuantity;
        }
    }
}
