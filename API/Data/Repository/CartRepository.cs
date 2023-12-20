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

        public async Task<Cart> GetCartByUserIdAsync(int userId){
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.ProductPhotos)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);
        }

        public async Task<bool> AddItemToCart(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (cart == null){
                return false;
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null){
                return false;
            }

            var cartItem = new CartItem{
                ProductId = productId,
                Cart = cart 
            };

            cart.Items.Add(cartItem);

            return await SaveAllAsync();
        }

        public async Task<bool> RemoveItemFromCart(int userId, int cartItemId)
        { 
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (cart == null){
                return false;
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null){
                return false;
            }

            cart.Items.Remove(item);

            return await SaveAllAsync();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}