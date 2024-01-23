using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(int userId){
            // Fetch the cart
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (cart == null || !cart.Items.Any()){
                return null; 
            }

            // Check for an existing 'Pending' order
            var existingOrder = await _context.Orders
                .Where(o => o.AppUserId == userId && o.Status == "Pending")
                .Include(o => o.Items)
                .FirstOrDefaultAsync();

            if (existingOrder != null){
                // Add items to the existing 'Pending' order
                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Price = cartItem.Product.Price,
                    };
                    existingOrder.Items.Add(orderItem);
                }
            } else {
                // Create a new order if no 'Pending' order exists
                existingOrder = new Order
                {
                    AppUserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    Items = cart.Items.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Price = ci.Product.Price,
                    }).ToList()
                };

                _context.Orders.Add(existingOrder);
            }

            UpdateOrderTotalPrice(existingOrder);
            await _context.SaveChangesAsync();

            // Optionally clear the cart after creating/updating the order
            cart.Items.Clear();
            await _context.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId){
            return await _context.Orders
                .Where(o => o.AppUserId == userId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.ProductPhotos)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAndStatusAsync(int userId, string status){
            return await _context.Orders
                .Where(o => o.AppUserId == userId && o.Status == status)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductPhotos)
                .ToListAsync();
        }

         public async Task<bool> RemoveOrderItem(int userId, int orderId, int itemId){
            var orderItem = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == itemId);

            if (orderItem == null || orderItem.Order.AppUserId != userId){
                return false; // Item not found or does not belong to the user
            }

            _context.OrderItems.Remove(orderItem);
            UpdateOrderTotalPrice(orderItem.Order);
            return await _context.SaveChangesAsync() > 0;
        }



        public async Task<bool> AddItemToOrder(int userId, int orderId, int productId){
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.AppUserId == userId);

            if (order == null) return false;

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            var orderItem = new OrderItem
            {
                ProductId = productId,
                Price = product.Price // Assuming price is a property of Product
            };

            order.Items.Add(orderItem);
            UpdateOrderTotalPrice(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrder(int userId, int orderId){
            var order = await _context.Orders.Where(o => o.Id == orderId && o.AppUserId == userId).SingleOrDefaultAsync();

            if (order == null){
                return false; 
            }

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        private void UpdateOrderTotalPrice(Order order){
            order.TotalPrice = order.Items.Sum(item => item.Price);
        }
    }
}