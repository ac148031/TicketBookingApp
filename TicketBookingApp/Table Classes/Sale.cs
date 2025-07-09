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

    public class FullSale : Sale
    {
        public FullCustomer SaleCustomer { get; set; }
        public FullConcert SaleConcert { get; set; }
        [Obsolete("", true)] public new int CustomerId { get; set; }
        [Obsolete("", true)] public new int ConcertId { get; set; }

        public FullSale(int saleId, FullCustomer fullCustomer, FullConcert fullConcert, int saleQuantity)
            : base(saleId, 0, 0, saleQuantity)
        {
            SaleCustomer = fullCustomer;
            SaleConcert = fullConcert;
        }
    }
}
