using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    /// <summary>
    /// Class to store order data from database.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public List<CartProduct> Products { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(40)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(60)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required]
        [StringLength(200)]
        public string StreetAdress { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
