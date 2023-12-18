using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int CartId { get; set; } // Optionally include this if you need a reference back to the cart
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Name of the product for display
        public decimal ProductPrice { get; set; } // Price of the product
        public string ProductPhotoUrl { get; set; } 
    }
}