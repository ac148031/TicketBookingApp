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
        public int ConcertTicketPrice { get; set; }
        public int LocationId { get; set; }

        public Concert(int concertId, string concertName, string concertDescription, DateOnly concertDate, TimeOnly concertTime, int concertAvailTickets, int concertTicketPrice, int locationId)
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
}
