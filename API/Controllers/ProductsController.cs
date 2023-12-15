using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository productRepository, IMapper mapper ){
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(){
            var products = await _productRepository.GetProductsAsync();
            var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productsToReturn);         
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string name){ 
            var product = await _productRepository.GetProductByNameAsync(name);
            return _mapper.Map<ProductDto>(product);
        }
        

        // [HttpGet("{id}")]
        // public async Task<ActionResult<AppUser>> GetUserId(int id){
        //     return await _userRepository.GetUserByIdAsync(id);
        // }
    }
}