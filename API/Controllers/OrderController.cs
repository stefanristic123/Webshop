using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateOrder(int userId){
        var order = await _orderRepository.CreateOrder(userId);
        var orderDto = _mapper.Map<OrderDto>(order);
        return Ok(orderDto); 
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserAsync(userId);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{userId:int}/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(int userId, string status)
        {
            var orders = await _orderRepository.GetOrdersByUserAndStatusAsync(userId, status);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpDelete("remove-item/{orderId}/{itemId}")]
        public async Task<IActionResult> RemoveOrderItem(int userId, int orderId, int itemId){
            var success = await _orderRepository.RemoveOrderItem(userId, orderId, itemId);
            if (!success) return NotFound("Order item not found or does not belong to the user.");
            return Ok();
        }

         [HttpPost("add-item/{orderId}")]
        public async Task<IActionResult> AddItemToOrder(int userId,int orderId, int productId)
        {
            var success = await _orderRepository.AddItemToOrder(userId, orderId, productId);
            if (!success) return BadRequest("Could not add item to order.");
            return Ok();
        }

    }
}