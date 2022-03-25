using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventMVC.Models;

namespace EventMVC.Models
{
    public class EventBookingContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public EventBookingContext(DbContextOptions options) : base(options)
        {

        }
        //public DbSet<EventMVC.Models.ModelView> ModelView { get; set; }
    }
}
