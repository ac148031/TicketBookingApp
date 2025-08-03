using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class Concert
    {
        public int ConcertId { get; set; }
        public string ConcertName { get; set; }
        public string ConcertDescription { get; set; }
        public DateOnly ConcertDate { get; set; }
        public TimeOnly ConcertTime { get; set; }
        public int ConcertAvailTickets { get; set; }
        public decimal ConcertTicketPrice { get; set; }
        public int LocationId { get; set; }

        public Concert(int concertId, string concertName, string concertDescription, DateOnly concertDate, TimeOnly concertTime, int concertAvailTickets, decimal concertTicketPrice, int locationId)
        {
            ConcertId = concertId;
            ConcertName = concertName;
            ConcertDescription = concertDescription;
            ConcertDate = concertDate;
            ConcertTime = concertTime;
            ConcertAvailTickets = concertAvailTickets;
            ConcertTicketPrice = concertTicketPrice;
            LocationId = locationId;
        }
    }

    public class FullConcert : Concert
    {
        public FullLocation ConcertLocation { get; set; }
        public List<Genre> GenreList { get; set; }
        [Obsolete("", true)] public new int LocationId { get; set; }

        public FullConcert(int concertId, string concertName, string concertDescription, DateOnly concertDate, TimeOnly concertTime, int concertAvailTickets, decimal concertTicketPrice)
            : base(concertId, concertName, concertDescription, concertDate, concertTime, concertAvailTickets, concertTicketPrice, 0)
        {
            ConcertLocation = null;
            GenreList = new List<Genre>();
        }
    }

    public class ConcertRevenue
    {
        public int ConcertId { get; set; }
        public string ConcertName { get; set; }
        public int TotalTicketSold { get; set; }
        public decimal TotalRevenue { get; set; }

        public ConcertRevenue(int concertId, string concertName, int totalTicketSold, decimal totalRevenue)
        {
            ConcertId = concertId;
            ConcertName = concertName;
            TotalTicketSold = totalTicketSold;
            TotalRevenue = totalRevenue;
        }

        public static List<ConcertRevenue> FromFullSales(List<FullSale> sales)
        {
            return sales.GroupBy(s => s.SaleConcert.ConcertId).Select(group => new ConcertRevenue(
                    group.Key,
                    group.FirstOrDefault().SaleConcert.ConcertName,
                    group.Sum(s => s.SaleQuantity),
                    group.Sum(s => s.SaleQuantity * group.FirstOrDefault().SaleConcert.ConcertTicketPrice)
                )).ToList();
        }
    }
}
