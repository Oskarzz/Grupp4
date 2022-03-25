using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Models
{
    public class Booking
    {
        
        public int Id { get; set; }
        [DisplayName("Antal")]
        public int Quantity { get; set; }
        [DisplayName("Summa")]
        public decimal Amount { get; set; }
        [DisplayName("Event")]
        public int EventId { get; set; }
        public Guid BookingRefId { get; set; }
        public Event Event { get; set; }
    }
}
