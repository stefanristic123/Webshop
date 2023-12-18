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
    public class CartController : BaseApiController
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<CartDto>> GetCart(int userId) {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return _mapper.Map<CartDto>(cart);
        }
    }
}