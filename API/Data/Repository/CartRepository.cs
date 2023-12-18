using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CartRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.ProductPhotos)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);
        }
    }
}