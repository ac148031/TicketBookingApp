﻿using System;
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
        public bool CustomerIsAdmin { get; set; }

        public Customer(int customerId, string customerFirstName, string customerLastName, string customerPhone, string customerEmail, string customerUsername, string customerPassword, bool customerIsAdmin)
        {
            CustomerId = customerId;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            CustomerUsername = customerUsername;
            CustomerPassword = customerPassword;
            CustomerIsAdmin = customerIsAdmin;
        }

        public Customer(int customerId, string customerFirstName, string customerLastName, string customerPhone, string customerEmail, string customerUsername, string customerPassword)
        {
            CustomerId = customerId;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            CustomerUsername = customerUsername;
            CustomerPassword = customerPassword;
            CustomerIsAdmin = false;
        }
    }

    public class FullCustomer : Customer
    {
        public List<FullCustomerAddress> CustomerAddresses { get; set; }

        public FullCustomer(
            int customerId,
            string customerFirstName,
            string customerLastName,
            string customerPhone,
            string customerEmail,
            string customerUsername,
            string customerPassword,
            bool customerIsAdmin,
            List<FullCustomerAddress>? customerAddresses = null)
        : base(
            customerId,
            customerFirstName,
            customerLastName,
            customerPhone,
            customerEmail,
            customerUsername,
            customerPassword,
            customerIsAdmin)
        {
            CustomerAddresses = customerAddresses ?? new();
        }
    }
}
