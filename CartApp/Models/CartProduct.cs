using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    public class CartProduct
    {
        public int Id { get; set; }     
        public string UserId { get; set; }      
        public int ProductId { get; set; }      
        public int Count { get; set; }
        public bool IsOrdered { get; set; }
    }
}
