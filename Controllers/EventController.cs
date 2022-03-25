using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventMVC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

namespace EventMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly EventBookingContext _context;

        public EventController(EventBookingContext context)
        {
            _context = context;
        }

        // GET: Event
        
        public async Task<IActionResult> Index()
        {
            
                List<Event> EventList = new List<Event>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://193.10.202.74/EventAPI1/api/Events"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        EventList = JsonConvert.DeserializeObject<List<Event>>(apiResponse);
                    }

                }
            
                ViewBag.Category = (from c in _context.Events select c.Category).Distinct();
                return View(EventList);
           
           

            //return View(await _context.Events.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(Event eventFromView)
        {
            ViewBag.Category = (from c in _context.Events select c.Category).Distinct();
            var model = from c in _context.Events orderby c.Category where c.Category == eventFromView.Category select c;
            return View(model);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            //ToDO arrangör

            return View(@event);
        }
      



        // GET: Event/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (@event.UploadedPic != null)
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await @event.UploadedPic.CopyToAsync(memoryStream);
                    @event.Picture = memoryStream.ToArray();
                }
            }
            else
            {

                @event.Picture = System.IO.File.ReadAllBytes("./otherdata/default.png");

            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }
        //using (var httpClient = new HttpClient())
        //{
        //    if (@event.UploadedPic != null)
        //    {
        //        using (var memoryStream = new System.IO.MemoryStream())
        //        {
        //            await @event.UploadedPic.CopyToAsync(memoryStream);
        //            @event.Picture = memoryStream.ToArray();
        //        }
        //    }
        //    else
        //    {

        //        @event.Picture = System.IO.File.ReadAllBytes("./otherdata/default.png");

        //    }

        //   httpClient.


        //    StringContent content = new StringContent(JsonConvert.SerializeObject(@event), Encoding.UTF8, "application/json");

        //        using (var response = await httpClient.PostAsJsonAsync<Event>("http://193.10.202.74/EventAPI1/api/Events", content))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            _event = JsonConvert.DeserializeObject<Event>(apiResponse);
        //        }

        //}
        //return View(_event);


        //    using (var httpclient = new HttpClient())
        //    {
        //        using (var memoryStream = new System.IO.MemoryStream())
        //        {
        //            await @event.UploadedPic.CopyToAsync(memoryStream);
        //            @event.Picture = memoryStream.ToArray();
        //        }

        //        httpclient.BaseAddress = new Uri("http://193.10.202.74/EventAPI1/api/Events");
        //        var posttask = httpclient.PostAsJsonAsync<Event>("Events", @event);
        //        posttask.Wait();

        //        var result = posttask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }



        //        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        //        return View(@event);
        //}









        // GET: Event/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StartTime,EndTime,Location,City,Description,Price,Name,Category,Prioritize,TicketsMax,Picture")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Event/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
