using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class LikesProductRepository : ILikesProductsRepository
    {
        private readonly DataContext _context;
        public LikesProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductLike> GetProductLike(int sourceUserId, int likedProductId)
        {
            return await _context.ProductLikes.FindAsync(sourceUserId, likedProductId);
        }

        public async Task<IEnumerable<ProductDto>> GetProductLikes(string predicate, int userId)
        {
            var products = _context.Products.OrderBy(u => u.Name).AsQueryable();
            var likes = _context.ProductLikes.AsQueryable();

            if(predicate == "liked"){
                likes = likes.Where(like => like.SourceUserId == userId);
                products = likes.Select(like => like.LikedProduct);
            } 

            return await products.Select(product => new ProductDto {
                Id = product.Id,
                Name = product.Name,
                ProductPhotoUrl = product.ProductPhotos.FirstOrDefault(p => p.IsMain).Url,
                Description = product.Description,
                Price = product.Price
            }).ToListAsync();
        }

        public async Task<Product> GetProductWithLikes(int userId)
        {
            return await _context.Products
                    .Include(x => x.ProductLikes)
                    .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}