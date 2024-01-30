using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repository;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Entities;
using API.DTOs;
using API.Data;

namespace API.Controllers
{
    public class LikesProductController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesProductsRepository _likesproductRepository;
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context; 

        public LikesProductController(
            IUserRepository userRepository, 
            ILikesProductsRepository likesproductRepository, 
            IProductRepository productRepository,
            DataContext context
            ){
            _productRepository = productRepository;
            _userRepository = userRepository;
            _likesproductRepository = likesproductRepository;
            _context = context;
        }


        [HttpPost("{productId}")]
        public async Task<ActionResult> AddLike(int productId){
            var sourceUserId = User.GetUserId(); // Ensure this method correctly gets the user's ID
            var likedProduct = await _productRepository.GetProductByIdAsync(productId);

            if (likedProduct == null) return NotFound();

            var existingLike = await _likesproductRepository.GetProductLike(sourceUserId, likedProduct.Id);

            if (existingLike != null)
            {
                return BadRequest("You already like this product");
            }

            var productLike = new ProductLike
            {
                SourceUserId = sourceUserId,
                LikedProductId = likedProduct.Id
            };

            // Using the context directly to add the ProductLike
            _context.ProductLikes.Add(productLike);

            // Save changes in the context
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }

            return BadRequest("Failed to like product");
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductLikes(string predicate){
            var products = await _likesproductRepository.GetProductLikes(predicate, User.GetUserId());

            return Ok(products);
        }

    }
}