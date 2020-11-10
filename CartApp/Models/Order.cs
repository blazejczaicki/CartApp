using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<CartProduct> Products { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string StreetAdress { get; set; }
        public string Email { get; set; }
    }
}
