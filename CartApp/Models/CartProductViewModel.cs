using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    /// <summary>
    /// Class to store cart product data from view.
    /// </summary>
    public class CartProductViewModel
    {
        public int Id { get; set; }
        public byte[] image { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int count { get; set; }
        public decimal subtotal { get; set; }
    }
}
