using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class ProductParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;  
        }

        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 1000000000;
        public string? SearchTerm { get; set; } = string.Empty;
        
        // public string OrderBy { get; set; } ="Created"; // 161
    }
}