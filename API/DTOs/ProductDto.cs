using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductPhotoUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<PhotoDto> ProductPhotos { get; set; } = new();      
    }
}