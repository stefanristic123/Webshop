using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Data.Repository
{
    public interface ILikesProductsRepository
    {
        Task<ProductLike> GetProductLike(int sourceUserId, int likedProductd); // using for finding individual like
        Task<Product> GetProductWithLikes(int userId); // list of products that one user liked
        Task<IEnumerable<ProductDto>> GetProductLikes(string predicate, int userId); // 
    }
} 