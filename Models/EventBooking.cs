using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Models
{
    public class EventBooking
    {
        public Event Event { get; set; }
        public Booking Booking { get; set; }

        //public int Id { get; set; }
        //public string Date { get; set; }
        //public string StartTime { get; set; }
        //public string EndTime { get; set; }
        //public string Location { get; set; }
        //public string City { get; set; }
        //public string Description { get; set; }
        //public int Price { get; set; }
        //public string Name { get; set; }
        //public string Category { get; set; }
        //public int Prioritize { get; set; }

        //public int Quantity { get; set; }
        //public List<Booking> Booking { get; set; }

        //[NotMapped]
        //public Microsoft.AspNetCore.Http.IFormFile UploadedPic { get; set; }
    }
}
