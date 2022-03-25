using EventMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public async Task<IActionResult> IndexAsync()
        {
            await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme);  //loggar ut användaren och skickar användaren till Home index sidan

            return RedirectToAction("Index", "Event");
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Loggain(string returnUrl)
        {
            ViewBag.ReturUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<dynamic> Loggain(string username, string password, string returnUrl)
        {
            try
            {

                var url = "http://193.10.202.75/LoginAPI/" + $"Authenticate?Username={username}&Password={password}";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Login failed");
                    return View();
                }
                
                
                var content = await response.Content.ReadAsStringAsync();
                var recievedLogin = JsonConvert.DeserializeObject<LoginDetails>(content);
                Debug.WriteLine("Login successful");
                //göra inloggning

                // Allt stämmer, logga in användaren
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, recievedLogin.Username));



                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Event");
                }

                return Redirect(returnUrl);
                

            }
             
            catch (Exception e)
            {
                Debug.WriteLine("Login failed: " + e.StackTrace);
                return null;
            }
           
        }


    }



}

