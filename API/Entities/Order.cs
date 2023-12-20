using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Order{
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Shipped", "Delivered"
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}