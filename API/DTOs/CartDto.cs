using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public ICollection<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}