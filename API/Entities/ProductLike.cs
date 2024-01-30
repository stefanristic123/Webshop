using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ProductLike
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }

        public Product LikedProduct { get; set; }
        public int LikedProductId { get; set; }
    }
}