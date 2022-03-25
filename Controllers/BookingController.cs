using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventMVC.Controllers
{
    
    public class BookingController : Controller
    {
        private readonly EventBookingContext _context;

        public BookingController(EventBookingContext context)
        {
            _context = context;
        }

        // GET: Booking
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var eventBookingContext = _context.Bookings.Include(b => b.Event);
            return View(await eventBookingContext.ToListAsync());
        }

        // GET: Booking/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create(int id)
        {
            var eventbooking = new EventBooking();

            var booking = new Booking { EventId = id, BookingRefId = Guid.NewGuid()};
            eventbooking.Booking = booking;
            Event _event = _context.Events.Find(id);
            eventbooking.Event = _event;

            return View(eventbooking);
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventBooking eventbooking)
        {

            eventbooking.Booking.Amount = eventbooking.Event.Price * eventbooking.Booking.Quantity;

            
            

              _context.Add(eventbooking.Booking);
              await _context.SaveChangesAsync();
            string urlStart = "http://193.10.202.71/GroupOne_WebClient/Payment/InputForm";
            
            return Redirect(urlStart+"?totalSum="+ Math.Ceiling(eventbooking.Booking.Amount)+"&referense="+eventbooking.Booking.BookingRefId);
            
            

            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", booking.EventId);


        }

        // GET: Booking/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", booking.EventId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,Amount,EventId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", booking.EventId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
