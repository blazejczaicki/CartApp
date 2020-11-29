using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    /// <summary>
    /// Class to store account data from database. Not used.
    /// </summary>
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
