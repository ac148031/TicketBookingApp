using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingApp.Table_Classes
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerPassword { get; set; }

        public Customer(int customerId, string customerFirstName, string customerLastName, string customerPhone, string customerEmail, string customerUsername, string customerPassword)
        {
            CustomerId = customerId;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            CustomerUsername = customerUsername;
            CustomerPassword = customerPassword;
        }
    }
}
