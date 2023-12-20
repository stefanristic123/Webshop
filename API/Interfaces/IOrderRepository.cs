using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(int userId);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByUserAndStatusAsync(int userId, string status);
        Task<bool> RemoveOrderItem(int userId, int orderId, int itemId);
        Task<bool> AddItemToOrder(int userId, int orderId, int productId);
    }
}