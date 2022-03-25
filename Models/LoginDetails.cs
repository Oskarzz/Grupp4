using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Models
{
    public class LoginDetails
    {
        public int AccessLevel { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
