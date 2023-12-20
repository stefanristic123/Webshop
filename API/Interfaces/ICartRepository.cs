using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<bool> AddItemToCart(int userId, int productId);
        Task<bool> RemoveItemFromCart(int userId, int cartItemId);
        Task<bool> SaveAllAsync();
    }
}