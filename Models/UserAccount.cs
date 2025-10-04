using System;
using System.Collections.Generic;
namespace CNPMNC.Models
{
    public class UserAccount
    {
          
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Passport { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Quan hệ 1-n với vé
        public List<Ticket>? Tickets { get; set; }
    }

    public class Ticket
    {
        public int Id { get; set; }
        public string FlightCode { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
   
