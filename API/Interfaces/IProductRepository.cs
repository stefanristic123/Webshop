 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);
        Task<bool> SaveAllAsync();

        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);

        Task<Product> GetProductByIdAsync(int id);

        Task<Product> GetProductByNameAsync(string name);
    }
}